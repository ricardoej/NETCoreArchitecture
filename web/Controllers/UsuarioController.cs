using domain.request;
using domain.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly InserirUsuarioService inserirUsuarioService;

        public UsuarioController(InserirUsuarioService inserirUsuarioService)
        {
            this.inserirUsuarioService = inserirUsuarioService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]InserirUsuarioRequest request)
        {
            var resposta = inserirUsuarioService.Executar(request);

            if (resposta.IsInserido)
                return Ok(new { resposta.Usuario.Id, resposta.Usuario.Login, resposta.Usuario.Nome });
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
