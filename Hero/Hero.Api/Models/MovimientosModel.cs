using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Models
{
	public class MovimientosModel
	{
		public DateTime Fecha { get; set; }
		public string Tipo { get; set; } 
		public double Valor { get; set; }
		public bool Activo { get; set; }

		public int CuentaOrigen { get; set; }
		public int CuentaDestino { get; set; }
		public int Id { get; set; }

	}
}
