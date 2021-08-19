using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Models
{
    public class UsuarioModel
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public bool Activo { get; set; }
        public int ClienteId { get; set; }
    }
}
