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
    [Route("Api/Movimientos")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly MovimientosRepository _movimientosRepository;

        public MovimientosController(MovimientosRepository movimientosRepository)
        {
            this._movimientosRepository = movimientosRepository;
        }

        [Route("Transferir")]
        [HttpPost]
        public async Task<ActionResult<bool>> Transferir([FromBody] MovimientosModel movimientosModel)
        {
            try
            {
                bool movimiento = await _movimientosRepository.Transferir(movimientosModel);

                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
