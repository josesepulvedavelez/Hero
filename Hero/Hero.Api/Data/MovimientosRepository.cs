using Hero.Api.Dtos;
using Hero.Api.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Hero.Api.Data
{
    /// <summary>
    /// Class <c>MovimientosRepository</c> 
    /// Contiene toda la funcionalidad de movimientos que el usuario puede realizar de una cuenta a otra.
    /// </summary>
    public class MovimientosRepository
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlTransaction transaccion;
        private readonly string _cadena;
        decimal result;

        string sqlMovimiento = "Insert Into " +
                               "    Movimientos(Fecha, Tipo, Valor, Activo, CuentaOrigen, CuentaDestino) " +
                               "Values(@Fecha, @Tipo, @Valor, @Activo, @CuentaOrigen, @CuentaDestino)";

        string sqlCuentaOrigen = "Update Cuenta Set Saldo=Saldo-@Valor Where Id=@CuentaOrigen";

        string sqlCuentaDestino = "Update Cuenta Set Saldo=Saldo+@Valor Where Id=@CuentaDestino";
                
        public MovimientosRepository(IConfiguration configuracion)
        {
            _cadena = configuracion.GetConnectionString("cadena");
        }

        /// <summary>
        /// Transfiere una cantidad de dinero de una cuenta origen a una cuenta destino
        /// </summary>
        /// <param name="movimientosModel">Objeto Json que contiene los campos que van a ser enviados entre las cuentas</param>
        /// <returns></returns>
        public async Task<bool> Transferir(MovimientosModel movimientosModel)
        {
            using (conexion = new SqlConnection(_cadena))
            {
                await conexion.OpenAsync();

                transaccion = (SqlTransaction) await conexion.BeginTransactionAsync();

                try
                {
                    comando = new SqlCommand(sqlMovimiento, conexion);
                    comando.Parameters.AddWithValue("@Fecha", movimientosModel.Fecha);
                    comando.Parameters.AddWithValue("@Tipo", movimientosModel.Tipo);
                    comando.Parameters.AddWithValue("@Valor", movimientosModel.Valor);
                    comando.Parameters.AddWithValue("@Activo", movimientosModel.Activo);
                    comando.Parameters.AddWithValue("@CuentaOrigen", movimientosModel.CuentaOrigen);
                    comando.Parameters.AddWithValue("@CuentaDestino", movimientosModel.CuentaDestino);
                    
                    comando.Transaction = transaccion;
                    result = (decimal) await comando.ExecuteNonQueryAsync();

                    comando = new SqlCommand(sqlCuentaOrigen, conexion);
                    comando.Parameters.AddWithValue("@Valor", movimientosModel.Valor);
                    comando.Parameters.AddWithValue("@CuentaOrigen", movimientosModel.CuentaOrigen);
                    
                    comando.Transaction = transaccion;
                    result = (decimal) await comando.ExecuteNonQueryAsync();

                    comando = new SqlCommand(sqlCuentaDestino, conexion);
                    comando.Parameters.AddWithValue("@Valor", movimientosModel.Valor);
                    comando.Parameters.AddWithValue("@CuentaDestino", movimientosModel.CuentaDestino);

                    comando.Transaction = transaccion;
                    result = (decimal)await comando.ExecuteNonQueryAsync();

                    await transaccion.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                }
            }

            if (result != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
