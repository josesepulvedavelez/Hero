using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Dtos
{
    public class PrincipalClienteDto
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Tipo { get; set; }
        public int Id { get; set; }
    }
}
