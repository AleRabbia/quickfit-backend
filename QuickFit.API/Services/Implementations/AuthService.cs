using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickFit.API.Data;
using QuickFit.API.Helpers;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Models.DTOs.Responses;
using QuickFit.API.Models.Entities;
using QuickFit.API.Services.Interfaces;
using BCrypt.Net;

namespace QuickFit.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly QuickFitDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(QuickFitDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
		{
			// Verificar si el email ya existe
			var existingUser = await _context.Users
				.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (existingUser != null)
			{
				throw new Exception("El email ya está registrado");
			}

			// Usamos una transacción para asegurar que si algo falla, se deshace todo
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// Crear usuario
				var user = new User
				{
					Name = request.Name,
					Email = request.Email,
					PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
					Role = "user",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				_context.Users.Add(user);
				await _context.SaveChangesAsync();

				// Crear perfil del usuario
				var userProfile = new UserProfile
				{
					UserId = user.Id, // asegúrate que este campo sea la FK correcta
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				_context.UserProfiles.Add(userProfile);
				await _context.SaveChangesAsync();

				// Confirmar la transacción
				await transaction.CommitAsync();

				// Generar token
				var token = _jwtHelper.GenerateToken(user);

				return new AuthResponse
				{
					Token = token,
					User = new UserResponse
					{
						Id = user.Id,
						Name = user.Name,
						Email = user.Email,
						Role = user.Role
					}
				};
			}
			catch (Exception ex)
			{
				// Revertir transacción si algo falla
				await transaction.RollbackAsync();

				// Capturar la excepción interna (la verdadera causa del error)
				var innerMessage = ex.InnerException?.Message ?? ex.Message;

				// Lanzar una excepción más clara para ver el detalle en el frontend
				throw new Exception($"Error al registrar: {innerMessage}");
			}
}
				

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            // Buscar usuario
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("Credenciales inválidas");
            }

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Credenciales inválidas");
            }

            // Generar token
            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                }
            };
        }

        public async Task<UserResponse> GetUserById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}