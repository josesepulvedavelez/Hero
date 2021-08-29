using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Dtos
{
    public class RegistroClienteDto
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Tipo { get; set; }
        public string Cedula_Nit { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool ActivoCliente { get; set; }
        public int Id { get; set; }

        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public bool ActivoUsuario { get; set; }
        public string ClienteId { get; set; }
    }
}
