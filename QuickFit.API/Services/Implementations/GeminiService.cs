using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Services.Interfaces;

namespace QuickFit.API.Services.Implementations
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string?> GenerateWorkoutPlanJsonAsync(CreateWorkoutPlanRequest request)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"] ?? "gemini-3.6-flash";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Falta la clave API de Gemini. Configurar en Render:\n" +
                    "1. Ve a Settings → Environment Variables\n" +
                    "2. Agrega: Gemini__ApiKey = tu-clave-aqui\n" +
                    "Para desarrollo local, usa: set Gemini__ApiKey=tu-clave-aqui"
                );
            }

            var prompt = BuildWorkoutPrompt(request);
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    temperature = 0.7
                }
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(payload)
            };

            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(httpRequest);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Gemini respondió con error: {response.StatusCode}. Detalle: {body}");
            }

            using var doc = JsonDocument.Parse(body);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return CleanJsonResponse(text);
        }

        private static string CleanJsonResponse(string? rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                return string.Empty;
            }

            var sanitized = rawText.Trim();
            if (sanitized.StartsWith("```"))
            {
                sanitized = sanitized.Replace("```json", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Replace("```", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Trim();
            }

            return sanitized;
        }

        public async Task<string?> GenerateAIMealPlanJsonAsync(GenerateAIMealPlanRequest request)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"] ?? "gemini-3.6-flash";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Falta la clave API de Gemini. Configurar en Render:\n" +
                    "1. Ve a Settings → Environment Variables\n" +
                    "2. Agrega: Gemini__ApiKey = tu-clave-aqui\n" +
                    "Para desarrollo local, usa: set Gemini__ApiKey=tu-clave-aqui"
                );
            }

            var prompt = BuildMealPrompt(request);
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    temperature = 0.7
                }
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(payload)
            };

            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(httpRequest);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Gemini respondió con error: {response.StatusCode}. Detalle: {body}");
            }

            using var doc = JsonDocument.Parse(body);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return CleanJsonResponse(text);
        }

        private static string BuildMealPrompt(GenerateAIMealPlanRequest request)
        {
            var allergies = string.IsNullOrWhiteSpace(request.Allergies) ? "ninguna" : request.Allergies;
            var dislikedFoods = string.IsNullOrWhiteSpace(request.DislikedFoods) ? "ninguno" : request.DislikedFoods;
            var favoriteFoods = string.IsNullOrWhiteSpace(request.FavoriteFoods) ? "variados" : request.FavoriteFoods;

            return $@"
Eres un nutricionista profesional especializado en planes de nutrición personalizados. Genera un plan de comidas detallado en JSON puro, sin markdown, sin explicaciones extras.

Instrucciones:
- Devuelve solo JSON válido.
- El JSON debe tener esta forma exacta:
{{
  ""name"": ""string"",
  ""description"": ""string"",
  ""goal"": ""string"",
  ""dietType"": ""string"",
  ""dailyCalories"": 2000,
  ""dailyProtein"": 150,
  ""dailyCarbs"": 200,
  ""dailyFats"": 65,
  ""mealsPerDay"": 4,
  ""meals"": [
    {{
      ""mealType"": ""Desayuno"",
      ""name"": ""string"",
      ""description"": ""string"",
      ""timeOfDay"": ""07:00"",
      ""calories"": 500,
      ""protein"": 40,
      ""carbs"": 50,
      ""fats"": 15,
      ""foods"": [
        {{
          ""foodName"": ""string"",
          ""quantity"": ""string"",
          ""calories"": 100,
          ""protein"": 10,
          ""carbs"": 8,
          ""fats"": 3
        }}
      ]
    }}
  ]
}}

Datos del usuario:
- edad: {request.Age}
- sexo biológico: {request.BiologicalSex}
- altura: {request.Height} cm
- peso: {request.Weight} kg
- objetivo: {request.MainGoal}
- nivel de actividad: {request.ActivityLevel}
- tipo de dieta: {request.DietType}
- comidas por día: {request.MealsPerDay}
- alergias: {allergies}
- alimentos no deseados: {dislikedFoods}
- alimentos favoritos: {favoriteFoods}
- tiempo disponible para cocinar: {request.CookingTime}
- presupuesto: {request.Budget}
- issues digestivos: {request.DigestiveIssues}

Reglas:
- El plan debe ser seguro, saludable y adaptado al objetivo.
- Incluye alimentos realistas y fáciles de conseguir.
- Respeta alergias e intolerancias.
- La cantidad total de calorías debe coincidir con el objetivo.
- Cada comida debe ser balanceada nutricionalmente.
- Incluye recetas o instrucciones simples en la descripción.
- La respuesta debe ser JSON válido, sin comentarios ni texto previo.
";
        }

        private static string BuildWorkoutPrompt(CreateWorkoutPlanRequest request)
        {
            var trainingDays = request.TrainingDays != null && request.TrainingDays.Any()
                ? string.Join(", ", request.TrainingDays)
                : "Lunes, Miércoles, Viernes";

            var equipment = request.Equipment != null && request.Equipment.Any()
                ? string.Join(", ", request.Equipment)
                : "peso corporal";

            var disliked = string.IsNullOrWhiteSpace(request.DislikedExercises)
                ? "ninguno"
                : request.DislikedExercises;

            return $@"
Eres un entrenador personal profesional. Genera un plan de entrenamiento estructurado en JSON puro, sin markdown, sin explicaciones extras.

Instrucciones:
- Devuelve solo JSON válido.
- El JSON debe tener esta forma exacta:
{{
  ""name"": ""string"",
  ""description"": ""string"",
  ""goal"": ""string"",
  ""experienceLevel"": ""string"",
  ""durationMinutes"": 30,
  ""daysPerWeek"": 3,
  ""trainingStyle"": ""string"",
  ""trainingPlace"": ""string"",
  ""equipment"": [""string""],
  ""trainingFocus"": ""string"",
  ""sessions"": [
    {{
      ""dayOfWeek"": ""string"",
      ""name"": ""string"",
      ""estimatedCalories"": 180,
      ""difficulty"": ""string"",
      ""exercises"": [
        {{
          ""name"": ""string"",
          ""description"": ""string"",
          ""durationSeconds"": 40,
          ""restSeconds"": 20,
          ""sets"": 3,
          ""reps"": 12,
          ""tips"": ""string""
        }}
      ]
    }}
  ]
}}

Datos del usuario:
- objetivo: {request.Goal}
- nivel: {request.ExperienceLevel}
- estilo: {request.TrainingStyle}
- lugar: {request.TrainingPlace}
- duración por sesión: {request.DurationMinutes} minutos
- días: {trainingDays}
- enfoque: {request.TrainingFocus}
- equipo disponible: {equipment}
- historial médico: {request.MedicalHistory ?? "ninguno"}
- ejercicios no deseados: {disliked}

Reglas:
- Ajusta el plan al objetivo, nivel y días disponibles.
- Usa solo ejercicios realistas y seguros.
- Usa un máximo de 5 ejercicios por sesión.
- La duración total por sesión debe estar acorde a la cantidad de minutos indicada.
- La respuesta debe ser JSON válido, sin comentarios ni texto previo.
";
        }
    }
}
