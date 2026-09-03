using System;
using System.Collections.Generic;
using System.Linq;
using QuickFit.API.Models.Entities;

namespace QuickFit.API.Validation
{
    public static class WorkoutPlanConstraints
    {
        public const int NameMaxLength = 100;
        public const int ShortTextMaxLength = 50;
        public const int MinDurationMinutes = 5;
        public const int MaxDurationMinutes = 120;
        public const int MinDaysPerWeek = 1;
        public const int MaxDaysPerWeek = 7;

        public static readonly IReadOnlySet<string> Goals = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "lose_weight", "gain_muscle", "cardio", "maintain", "flexibility", "other"
        };

        public static readonly IReadOnlySet<string> ExperienceLevels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "beginner", "intermediate", "advanced"
        };

        public static readonly IReadOnlySet<string> TrainingStyles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "hiit", "strength", "mixed", "cardio"
        };

        public static readonly IReadOnlySet<string> TrainingPlaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "home", "gym", "outdoor"
        };

        public static readonly IReadOnlySet<string> TrainingFocuses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "full_body", "split"
        };

        public static List<string> ValidateRequest(Models.DTOs.Requests.CreateWorkoutPlanRequest request)
        {
            var errors = ValidateFields(request.Name, request.Goal, request.ExperienceLevel, request.TrainingStyle,
                request.TrainingPlace, request.TrainingFocus, request.DurationMinutes, request.DaysPerWeek,
                requireName: true);

            if (request.TrainingDays != null && request.TrainingDays.Count > MaxDaysPerWeek)
            {
                errors.Add($"TrainingDays no puede contener más de {MaxDaysPerWeek} días.");
            }

            if (request.TrainingDays != null && request.TrainingDays.Count > 0 &&
                request.TrainingDays.Count != request.DaysPerWeek)
            {
                errors.Add("DaysPerWeek debe coincidir con la cantidad de TrainingDays.");
            }

            return errors;
        }

        public static List<string> ValidatePlan(WorkoutPlan plan)
        {
            return ValidateFields(plan.Name, plan.Goal, plan.ExperienceLevel, plan.TrainingStyle,
                plan.TrainingPlace, plan.TrainingFocus, plan.DurationMinutes, plan.DaysPerWeek,
                requireName: true);
        }

        public static List<string> ValidateFields(string? name, string? goal, string? experienceLevel,
            string? trainingStyle, string? trainingPlace, string? trainingFocus, int durationMinutes,
            int daysPerWeek, bool requireName)
        {
            var errors = new List<string>();
            AddRequired(errors, nameof(name), name, requireName);
            AddLength(errors, nameof(name), name, NameMaxLength);
            AddRequired(errors, nameof(goal), goal, true);
            AddLength(errors, nameof(goal), goal, ShortTextMaxLength);
            AddAllowed(errors, nameof(goal), goal, Goals);
            AddRequired(errors, nameof(experienceLevel), experienceLevel, true);
            AddLength(errors, nameof(experienceLevel), experienceLevel, ShortTextMaxLength);
            AddAllowed(errors, nameof(experienceLevel), experienceLevel, ExperienceLevels);
            AddRequired(errors, nameof(trainingStyle), trainingStyle, true);
            AddLength(errors, nameof(trainingStyle), trainingStyle, ShortTextMaxLength);
            AddAllowed(errors, nameof(trainingStyle), trainingStyle, TrainingStyles);
            AddRequired(errors, nameof(trainingPlace), trainingPlace, true);
            AddLength(errors, nameof(trainingPlace), trainingPlace, ShortTextMaxLength);
            AddAllowed(errors, nameof(trainingPlace), trainingPlace, TrainingPlaces);
            AddRequired(errors, nameof(trainingFocus), trainingFocus, true);
            AddLength(errors, nameof(trainingFocus), trainingFocus, ShortTextMaxLength);
            AddAllowed(errors, nameof(trainingFocus), trainingFocus, TrainingFocuses);

            if (durationMinutes < MinDurationMinutes || durationMinutes > MaxDurationMinutes)
            {
                errors.Add($"DurationMinutes debe estar entre {MinDurationMinutes} y {MaxDurationMinutes}.");
            }

            if (daysPerWeek < MinDaysPerWeek || daysPerWeek > MaxDaysPerWeek)
            {
                errors.Add($"DaysPerWeek debe estar entre {MinDaysPerWeek} y {MaxDaysPerWeek}.");
            }

            return errors;
        }

        private static void AddRequired(List<string> errors, string field, string? value, bool required)
        {
            if (required && string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"{field} es obligatorio.");
            }
        }

        private static void AddLength(List<string> errors, string field, string? value, int maxLength)
        {
            if (value?.Length > maxLength)
            {
                errors.Add($"{field} no puede superar {maxLength} caracteres (recibidos: {value.Length}).");
            }
        }

        private static void AddAllowed(List<string> errors, string field, string? value, IReadOnlySet<string> allowed)
        {
            if (!string.IsNullOrWhiteSpace(value) && !allowed.Contains(value))
            {
                errors.Add($"{field} contiene un valor no permitido.");
            }
        }
    }

    public sealed class WorkoutPlanValidationException : Exception
    {
        public WorkoutPlanValidationException(IEnumerable<string> errors)
            : base("Los datos del plan de entrenamiento no son válidos.")
        {
            Errors = errors.ToArray();
        }

        public IReadOnlyList<string> Errors { get; }
    }
}