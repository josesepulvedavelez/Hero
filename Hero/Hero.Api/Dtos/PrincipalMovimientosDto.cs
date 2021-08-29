using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Dtos
{
    public class PrincipalMovimientosDto
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
    }
}
