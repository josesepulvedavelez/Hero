using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Models
{
    public class ClienteModel
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Tipo { get; set; }
        public string Cedula_Nit { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }
        public int Id { get; set; }
    }
}
