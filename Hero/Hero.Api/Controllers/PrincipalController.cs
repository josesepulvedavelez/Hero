using Hero.Api.Data;
using Hero.Api.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Controllers
{
    [Route("Api/Principal")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        private readonly PrincipalRepository _clienteRepository;

        public PrincipalController(PrincipalRepository repository)
        {
            this._clienteRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Route("ObtenerCliente/{IdCliente}")]
        [HttpGet]
        public async Task<ActionResult<PrincipalClienteDto>> ObtenerCliente(int idCliente)
        {
            try
            {
                var principalClienteDto = await _clienteRepository.ObtenerCliente(idCliente);

                return Ok(principalClienteDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("ObtenerCuentas/{IdCliente}")]
        [HttpGet]
        public async Task<ActionResult<List<PrincipalCuentaDto>>> ObtenerCuentas(int idCliente)
        {
            try
            {
                var lst = await _clienteRepository.ObtenerCuentas(idCliente);

                return Ok(lst);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("ObtenerMovimientos/{IdCuenta}")]
        [HttpGet]
        public async Task<ActionResult<List<PrincipalMovimientosDto>>> ObtenerMovimientos(int idCuenta)
        {
            try
            {
                var lst = await _clienteRepository.ObtenerMovimientos(idCuenta);

                return Ok(lst);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
