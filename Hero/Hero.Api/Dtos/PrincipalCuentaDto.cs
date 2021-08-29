using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Dtos
{
    public class PrincipalCuentaDto
    {
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public double Saldo { get; set; }
        public int ClienteId { get; set; }
        public int Id { get; set; }
    }
}
