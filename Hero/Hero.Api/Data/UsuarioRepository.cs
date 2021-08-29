
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
    public class UsuarioRepository
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;
        SqlTransaction transaccion;
        private readonly string _cadena;
        int result;
        
        string sqlCliente = "Insert Into Cliente(Nombres, Apellidos, Tipo, Cedula_Nit, Correo, Telefono, Activo) " +
                            "Values(@Nombres, @Apellidos, @Tipo, @Cedula_Nit, @Correo, @Telefono, @Activo);" +
                            "Select SCOPE_IDENTITY()";

        string sqlUsuario = "Insert Into Usuario(Usuario, Contraseña, Activo, ClienteId) " +
                            "Values(@Usuario, @Contraseña, @Activo, @ClienteId)";
        decimal maxId;

        public UsuarioRepository(IConfiguration configuracion)
        {
            _cadena = configuracion.GetConnectionString("cadena");
        }

        public async Task CrearUsuario(RegistroClienteDto registroClienteDto)
        {
            using (conexion = new SqlConnection(_cadena))
            {
                conexion.Open();

                transaccion = conexion.BeginTransaction();

                try
                {
                    comando = new SqlCommand(sqlCliente, conexion);
                    comando.Parameters.AddWithValue("@Nombres", registroClienteDto.Nombres);
                    comando.Parameters.AddWithValue("@Apellidos", registroClienteDto.Apellidos);
                    comando.Parameters.AddWithValue("@Tipo", registroClienteDto.Tipo);
                    comando.Parameters.AddWithValue("@Cedula_Nit", registroClienteDto.Cedula_Nit);
                    comando.Parameters.AddWithValue("@Correo", registroClienteDto.Correo);
                    comando.Parameters.AddWithValue("@Telefono", registroClienteDto.Telefono);
                    comando.Parameters.AddWithValue("@Activo", registroClienteDto.ActivoCliente);
                    
                    comando.Transaction = transaccion;
                    maxId = (decimal) await comando.ExecuteScalarAsync();

                    comando = new SqlCommand(sqlUsuario, conexion);
                    comando.Parameters.AddWithValue("@Usuario", registroClienteDto.Usuario);
                    comando.Parameters.AddWithValue("@Contraseña", registroClienteDto.Contraseña);
                    comando.Parameters.AddWithValue("@Activo", registroClienteDto.ActivoUsuario);
                    comando.Parameters.AddWithValue("@ClienteId", maxId);
                    comando.Transaction = transaccion;
                    await comando.ExecuteNonQueryAsync();

                    transaccion.Commit();
                }
                catch (Exception ex)
                {                    
                    transaccion.Rollback();
                }
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
