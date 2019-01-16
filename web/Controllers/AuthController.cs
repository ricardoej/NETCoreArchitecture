using data.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using service.services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web.Settings;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUsuarioService usuarioService;
        private readonly AppSettings appSettings;

        public AuthController(IUsuarioService usuarioService, IOptions<AppSettings> appSettings)
        {
            this.usuarioService = usuarioService;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult Authenticate([FromBody]LoginRequest loginRequest)
        {
            var usuario = usuarioService.Autenticar(loginRequest.Login, loginRequest.Senha);

            if (usuario == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(new
            {
                usuario.Id,
                usuario.Login,
                usuario.Nome,
                Token = tokenString
            });
        }
    }
}
