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
    public class UsuarioRepository
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;
        TransactionScope transaccion;
        private readonly string _cadena;
        int result;
        
        string sqlCliente = "Insert Into Cliente(Nombres, Apellidos, Tipo, Cedula_Nit, Correo, Telefono, Activo) " +
                            "Values(@Nombres, @Apellidos, @Tipo, @Cedula_Nit, @Correo, @Telefono, @Activo);" +
                            "Select SCOPE_IDENTITY() AS Id";
        
        string sqlUsuario = "Insert Into Usuario(Usuario, Contraseña, Activo, ClienteId) " +
                            "Values(@Usuario, @Contraseña, @Activo, @ClienteId)";
        object IdMax;

        public UsuarioRepository(IConfiguration configuracion)
        {
            _cadena = configuracion.GetConnectionString("cadena");
        }

        public async Task CrearUsuario(ClienteModel clienteModel)
        {
            try
            {
                using (transaccion = new TransactionScope())
                {
                    using (conexion = new SqlConnection(_cadena))
                    {
                        await conexion.OpenAsync();

                        comando = new SqlCommand(sqlCliente, conexion);
                        comando.Parameters.AddWithValue("@Nombres", clienteModel.Nombres);
                        comando.Parameters.AddWithValue("@Apellidos", clienteModel.Apellidos);
                        comando.Parameters.AddWithValue("@Tipo", clienteModel.Tipo);
                        comando.Parameters.AddWithValue("@Cedula_Nit", clienteModel.Cedula_Nit);
                        comando.Parameters.AddWithValue("@Correo", clienteModel.Correo);
                        comando.Parameters.AddWithValue("@Telefono", clienteModel.Telefono);
                        comando.Parameters.AddWithValue("@Activo", clienteModel.ActivoCliente);
                        //await comando.ExecuteNonQueryAsync();

                        IdMax = await (comando.ExecuteScalarAsync());

                        comando = new SqlCommand(sqlUsuario, conexion);
                        comando.Parameters.AddWithValue("@Usuario", clienteModel.Usuario);
                        comando.Parameters.AddWithValue("@Contraseña", clienteModel.Contraseña);
                        comando.Parameters.AddWithValue("@Activo", clienteModel.ActivoUsuario);
                        comando.Parameters.AddWithValue("@ClienteId", IdMax);
                        await comando.ExecuteNonQueryAsync();
                    }

                    transaccion.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                transaccion.Dispose();
            }            
        }

        public async Task<UsuarioModel> Loguear(UsuarioModel usuarioModel)
        {
            UsuarioModel usuario = new UsuarioModel();

            using (conexion = new SqlConnection(_cadena))
            {
                await conexion.OpenAsync();

                using (comando = new SqlCommand("Select * From Usuario Where Usuario=@Usuario And Contraseña=@Contraseña", conexion))
                {
                    comando.Parameters.AddWithValue("@Usuario", usuarioModel.Usuario);
                    comando.Parameters.AddWithValue("@Contraseña", usuarioModel.Contraseña);

                    lector = await comando.ExecuteReaderAsync();

                    while (await lector.ReadAsync())
                    {
                        usuario.Usuario = Convert.ToString(lector["Usuario"]);
                        usuario.Contraseña = Convert.ToString(lector["Contraseña"]);
                        usuario.Activo = Convert.ToBoolean(lector["Activo"]);
                        usuario.ClienteId = Convert.ToInt16(lector["ClienteId"]);                        
                    }
                }
            }

            return usuario;
        }

    }
}
