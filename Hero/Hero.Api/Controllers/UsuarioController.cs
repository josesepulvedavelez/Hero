using Hero.Api.Data;
using Hero.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Controllers
{
    [Route("Api/Usuario")]
    [ApiController]    
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository repository)
        {
            this._usuarioRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Route("Loguear")]
        [HttpPost]
        public async Task<ActionResult<UsuarioModel>> Loguear([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                var logueo = await _usuarioRepository.Loguear(usuarioModel);
                
                return Ok(logueo);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Route("CrearUsuario")]
        [HttpPost]
        public async Task<ActionResult> CrearUsuario([FromBody] ClienteModel clienteModel)
        {
            try
            {
                await _usuarioRepository.CrearUsuario(clienteModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}
