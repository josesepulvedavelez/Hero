using Hero.Api.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Data
{
    public class PrincipalRepository
    {
        SqlConnection conexion;
        SqlCommand comando;
        int result;

        private readonly string _cadena;

        public PrincipalRepository(IConfiguration configuracion)
        {
            _cadena = configuracion.GetConnectionString("cadena");
        }

        public async Task<PrincipalClienteDto> ObtenerCliente(int id)
        { 
            
        }

        public async Task<PrincipalCuentaDto> ObtenerCuentas()
        { 
        
        }

        public async Task<PrincipalMovimientosDto> ObtenerMovimientos()
        { 
        
        }


    }
}
