using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Models
{
    public class CuentaModel
    {
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public double Saldo { get; set; }
        public int ClienteId { get; set; }
        public bool Activo { get; set; }
        public int Id { get; set; }
    }
}
