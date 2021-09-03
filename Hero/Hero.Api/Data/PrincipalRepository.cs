using Hero.Api.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Data
{
    /// <summary>
    /// Class <c>PrincipalRepository</c> 
    /// Contiene toda la funcionalidad para cargar los datos del usuario en sesion, las cuentas y los movimientos.
    /// </summary>
    public class PrincipalRepository
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;
        int result;

        string sqlObtenerCliente = "Select Nombres, Apellidos, Tipo, Id " +
                                   "From Cliente " +
                                   "Where Id=@IdCliente;";

        string sqlObtenerCuentas = "Select Numero, Tipo, Saldo, Activo, ClienteId, Id " +
                                   "From Cuenta " +
                                   "Where ClienteId=@ClienteId";

        string sqlObtenerMovimientos = "Select Fecha, Tipo, Valor, Activo, CuentaOrigen, CuentaDestino, Id " +
                                       "From Movimientos " +
                                       "Where CuentaOrigen=@CuentaOrigen";

        private readonly string _cadena;

        public PrincipalRepository(IConfiguration configuracion)
        {
            _cadena = configuracion.GetConnectionString("cadena");
        }

        /// <summary>
        /// Validar el usuario actual en sesion y cargar datos personales
        /// </summary>
        /// <param name="idCliente">Id del cliente es UNICO</param>
        /// <returns></returns>
        public async Task<PrincipalClienteDto> ObtenerCliente(int idCliente)
        {
            PrincipalClienteDto principalClienteDto = new PrincipalClienteDto();

            using (conexion = new SqlConnection(_cadena))
            {
                await conexion.OpenAsync();

                comando = new SqlCommand(sqlObtenerCliente, conexion);
                comando.Parameters.AddWithValue("@IdCliente", idCliente);
                lector = await comando.ExecuteReaderAsync();

                while (await lector.ReadAsync())
                {
                    principalClienteDto.Nombres = Convert.ToString(lector["Nombres"]);
                    principalClienteDto.Apellidos = Convert.ToString(lector["Apellidos"]);
                    principalClienteDto.Tipo = Convert.ToString(lector["Tipo"]);
                    principalClienteDto.Id = Convert.ToInt16(lector["Id"]);
                }
            }

            return principalClienteDto;
        }

        /// <summary>
        /// Obtener las cuentas del usuario actual en sesion
        /// </summary>
        /// <param name="idCliente">Id del cliente es UNICO</param>
        /// <returns></returns>
        public async Task<List<PrincipalCuentaDto>> ObtenerCuentas(int idCliente)
        {
            List<PrincipalCuentaDto> lstPrincipalCuentaDto = new List<PrincipalCuentaDto>();

            using (conexion = new SqlConnection(_cadena))
            {
                await conexion.OpenAsync();

                using (comando = new SqlCommand(sqlObtenerCuentas, conexion))
                {
                    comando.Parameters.AddWithValue("@ClienteId", idCliente);
                    lector = await comando.ExecuteReaderAsync();

                    while (await lector.ReadAsync())
                    {
                        PrincipalCuentaDto principalCuentaDto = new PrincipalCuentaDto
                        {
                            Numero = Convert.ToString(lector["Numero"]),
                            Tipo = Convert.ToString(lector["Tipo"]),
                            Saldo = Convert.ToDouble(lector["Saldo"]),
                            Activo = Convert.ToBoolean(lector["Activo"]),
                            ClienteId = Convert.ToInt32(lector["ClienteId"]),
                            Id = Convert.ToInt32(lector["Id"])
                        };

                        lstPrincipalCuentaDto.Add(principalCuentaDto);
                    }
                }
            }

            return lstPrincipalCuentaDto;
        }

        /// <summary>
        /// Validar los movimientos del usuario actual en sesion y la cuenta seleccionada
        /// </summary>
        /// <param name="idCliente">Id de la cuenta es UNICO</param>
        /// <returns></returns>
        public async Task<List<PrincipalMovimientosDto>> ObtenerMovimientos(int IdCuenta)
        {
            List<PrincipalMovimientosDto> lstPrincipalMovimientos = new List<PrincipalMovimientosDto>();

            using (conexion = new SqlConnection(_cadena))
            {
                await conexion.OpenAsync();

                using (comando = new SqlCommand(sqlObtenerMovimientos, conexion))
                {
                    comando.Parameters.AddWithValue("@CuentaOrigen", IdCuenta);
                    lector = await comando.ExecuteReaderAsync();

                    while (await lector.ReadAsync())
                    {
                        PrincipalMovimientosDto principalMovimientosDto = new PrincipalMovimientosDto
                        {
                            Fecha = Convert.ToDateTime(lector["Fecha"]),
                            Tipo = Convert.ToString(lector["Tipo"]),
                            Valor = Convert.ToDouble(lector["Valor"]),
                            Activo = Convert.ToBoolean(lector["Activo"]),
                            CuentaOrigen = Convert.ToInt32(lector["CuentaOrigen"]),
                            CuentaDestino = Convert.ToInt32(lector["CuentaDestino"]),
                            Id = Convert.ToInt32(lector["Id"])
                        };

                        lstPrincipalMovimientos.Add(principalMovimientosDto);
                    }
                }
            }

            return lstPrincipalMovimientos;
        }

    }
}
