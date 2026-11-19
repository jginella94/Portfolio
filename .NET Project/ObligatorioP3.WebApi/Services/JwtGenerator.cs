using Microsoft.IdentityModel.Tokens;
using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ObligatorioP3.WebApi.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSettings _settings;

        public JwtGenerator(JwtSettings settings)
        {
            _settings = settings;
        }

        public string GenerateToken(DtoUsuarioAutenticado usuario)
        {
            byte[] key = Encoding.UTF8.GetBytes(_settings.Key);
            if (key.Length < 32)
                throw new ArgumentException("La clave JWT debe tener al menos 32 bytes.");

            Claim[] claims =
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(tokenDescriptor));
        }
    }
}
