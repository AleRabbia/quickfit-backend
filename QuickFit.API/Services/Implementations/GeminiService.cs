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
