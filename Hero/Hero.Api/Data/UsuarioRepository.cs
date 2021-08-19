using Hero.Api.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Data
{
    public class UsuarioRepository
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;
        private readonly string _cadena;
        private int result;

        public UsuarioRepository(IConfiguration configuracion)
        {
            _cadena = configuracion.GetConnectionString("cadena");
        }

        public async Task<UsuarioModel> Loguear(UsuarioModel usuarioModel)
        {
            UsuarioModel usuario = new UsuarioModel();

            using (SqlConnection conexion = new SqlConnection(_cadena))
            {
                await conexion.OpenAsync();

                using (SqlCommand comando = new SqlCommand("Select * From Usuario Where Usuario=@Usuario And Contraseña=@Contraseña", conexion))
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
