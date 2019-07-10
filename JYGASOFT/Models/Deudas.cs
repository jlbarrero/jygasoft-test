using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JYGASOFT.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Deudas
    {
        private string nombre;
        private string email;
        private decimal monto_debe;
        private decimal monto_pago;
        private decimal monto_final;

        public Deudas(string nombre, string email, decimal monto_debe, decimal monto_pago, decimal monto_final)
        {
            this.Nombre = nombre;
            this.Email = email;
            this.Monto_debe = monto_debe;
            this.Monto_pago = monto_pago;
            this.Monto_final = monto_final;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Email { get => email; set => email = value; }
        public decimal Monto_debe { get => monto_debe; set => monto_debe = value; }
        public decimal Monto_pago { get => monto_pago; set => monto_pago = value; }
        public decimal Monto_final { get => monto_final; set => monto_final = value; }
    }
}