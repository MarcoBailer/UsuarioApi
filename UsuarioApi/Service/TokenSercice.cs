using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuarioApi.Models;

namespace UsuarioApi.Service
{
    public class TokenSercice
    {
        public string GerarToken(Usuario usuario)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("username", usuario.UserName),
                new Claim("id", usuario.Id),
                new Claim(ClaimTypes.DateOfBirth, 
                usuario.DataNascimento.ToString())
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kJBEHfbilubjkbIUBilGIIUG"));

            var sigingCredentials = 
                new SigningCredentials(chave,SecurityAlgorithms.HmacSha256);  

            var token = new JwtSecurityToken
                (
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials : sigingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}