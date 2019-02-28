using domain.request;
using domain.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web.Settings;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutenticacaoController : Controller
    {
        private readonly AutenticarUsuarioService autenticarUsuarioService;
        private readonly AppSettings appSettings;

        public AutenticacaoController(AutenticarUsuarioService autenticarUsuarioService, IOptions<AppSettings> appSettings)
        {
            this.autenticarUsuarioService = autenticarUsuarioService;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]AutenticarRequest request)
        {
            try
            {
                var resposta = autenticarUsuarioService.Executar(request);

                if (!resposta.IsLogado)
                    return BadRequest(new { message = "Login ou senha incorretos" });

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, resposta.Usuario.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    resposta.Usuario.Id,
                    resposta.Usuario.Login,
                    resposta.Usuario.Nome,
                    Token = tokenString
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
