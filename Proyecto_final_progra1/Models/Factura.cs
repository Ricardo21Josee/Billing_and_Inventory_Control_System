// --------------------------------------------------------
// Author: Ricardo Márquez
// GitHub: https://github.com/Ricardo21Josee
// LinkedIn: https://www.linkedin.com/in/ric21marquez
// Instagram: @mar_quez_g
// Threads: @mar_quez_g
// Email: josemarquez21garcia@gmail.com
// --------------------------------------------------------

namespace Proyecto_final_progra1.Models
{
    using System.Collections.Generic;

    public class Factura
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
        public string NitEmpresa { get; set; }
        public int ClienteId { get; set; }
        public int CfClienteId { get; set; }
        public double SubtotalSinIva { get; set; }
        public double Subtotal { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }
        public bool RequiereNit { get; set; }
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}