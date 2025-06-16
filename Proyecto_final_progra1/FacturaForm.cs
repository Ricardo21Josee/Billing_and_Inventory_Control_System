using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Proyecto_final_progra1.Logic;
using Proyecto_final_progra1.Models;

namespace Proyecto_final_progra1
{
    public partial class FacturaForm : Form
    {
        private FacturaManager facturaManager;
        private InventarioManager inventarioManager;
        private List<Producto> productosSeleccionados = new List<Producto>();
        private DataGridView dgvInventario, dgvFactura;
        private TextBox txtNombre, txtDireccion, txtNIT, txtDPI, txtCantidad, txtProductoId;
        private Button btnAgregarProducto, btnEmitirFactura;
        private Label lblTotales;
        private double subtotal = 0, subtotalSinIVA = 0, iva = 0, total = 0;

        public FacturaForm()
        {
            InitializeComponent();
            facturaManager = new FacturaManager();
            inventarioManager = new InventarioManager();
            InicializarControles();
            CargarInventario();
        }

        private void InicializarControles()
        {
            // Datos del cliente
            var lblCliente = new Label { Text = "Datos del Cliente", Top = 10, Left = 10, Width = 200, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Controls.Add(lblCliente);

            txtNombre = new TextBox { Top = 35, Left = 10, Width = 150 };
            txtNombre.Text = "Nombre";
            txtNombre.GotFocus += (s, e) => { if (txtNombre.Text == "Nombre") txtNombre.Text = ""; };
            txtNombre.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtNombre.Text)) txtNombre.Text = "Nombre"; };
            Controls.Add(txtNombre);

            txtDireccion = new TextBox { Top = 35, Left = 170, Width = 200 };
            txtDireccion.Text = "Dirección";
            txtDireccion.GotFocus += (s, e) => { if (txtDireccion.Text == "Dirección") txtDireccion.Text = ""; };
            txtDireccion.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtDireccion.Text)) txtDireccion.Text = "Dirección"; };
            Controls.Add(txtDireccion);

            txtNIT = new TextBox { Top = 35, Left = 380, Width = 100 };
            txtNIT.Text = "NIT";
            txtNIT.GotFocus += (s, e) => { if (txtNIT.Text == "NIT") txtNIT.Text = ""; };
            txtNIT.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtNIT.Text)) txtNIT.Text = "NIT"; };
            Controls.Add(txtNIT);

            txtDPI = new TextBox { Top = 35, Left = 490, Width = 120 };
            txtDPI.Text = "DPI (opcional)";
            txtDPI.GotFocus += (s, e) => { if (txtDPI.Text == "DPI (opcional)") txtDPI.Text = ""; };
            txtDPI.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtDPI.Text)) txtDPI.Text = "DPI (opcional)"; };
            Controls.Add(txtDPI);

            // Inventario
            dgvInventario = new DataGridView
            {
                Top = 70,
                Left = 10,
                Width = 400,
                Height = 200,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(dgvInventario);

            txtProductoId = new TextBox { Top = 280, Left = 10, Width = 50 };
            txtProductoId.Text = "ID";
            txtProductoId.GotFocus += (s, e) => { if (txtProductoId.Text == "ID") txtProductoId.Text = ""; };
            txtProductoId.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtProductoId.Text)) txtProductoId.Text = "ID"; };
            Controls.Add(txtProductoId);

            txtCantidad = new TextBox { Top = 280, Left = 70, Width = 50 };
            txtCantidad.Text = "Cantidad";
            txtCantidad.GotFocus += (s, e) => { if (txtCantidad.Text == "Cantidad") txtCantidad.Text = ""; };
            txtCantidad.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtCantidad.Text)) txtCantidad.Text = "Cantidad"; };
            Controls.Add(txtCantidad);

            btnAgregarProducto = new Button { Text = "Agregar a Factura", Top = 280, Left = 130, Width = 130 };
            btnAgregarProducto.Click += BtnAgregarProducto_Click;
            Controls.Add(btnAgregarProducto);

            // Productos en la factura
            dgvFactura = new DataGridView
            {
                Top = 320,
                Left = 10,
                Width = 600,
                Height = 120,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(dgvFactura);

            // Totales
            lblTotales = new Label { Top = 450, Left = 10, Width = 600, Height = 40, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Controls.Add(lblTotales);

            // Botón emitir factura
            btnEmitirFactura = new Button { Text = "Emitir Factura", Top = 500, Left = 10, Width = 150 };
            btnEmitirFactura.Click += BtnEmitirFactura_Click;
            Controls.Add(btnEmitirFactura);

            this.Height = 600;
            this.Width = 650;
        }

        private void CargarInventario()
        {
            dgvInventario.DataSource = inventarioManager.ObtenerInventario();
        }

        private void BtnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtProductoId.Text, out int id) && int.TryParse(txtCantidad.Text, out int cantidad))
            {
                var producto = inventarioManager.ObtenerProductoPorId(id);
                if (producto != null && producto.Existencia >= cantidad)
                {
                    var prodFactura = new Producto
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Precio = producto.Precio,
                        Existencia = cantidad
                    };
                    productosSeleccionados.Add(prodFactura);
                    ActualizarFacturaGrid();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado o existencia insuficiente.");
                }
            }
        }

        private void ActualizarFacturaGrid()
        {
            dgvFactura.DataSource = null;
            dgvFactura.DataSource = productosSeleccionados;
            subtotal = 0;
            foreach (var p in productosSeleccionados)
                subtotal += p.Precio * p.Existencia;
            subtotalSinIVA = subtotal / 1.12;
            iva = subtotalSinIVA * 0.12;
            total = subtotal;

            lblTotales.Text = $"Subtotal sin IVA: {subtotalSinIVA:F2}   IVA: {iva:F2}   Total: {total:F2}";
        }

        private void BtnEmitirFactura_Click(object sender, EventArgs e)
        {
            if (productosSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto a la factura.");
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            string nit = txtNIT.Text.Trim();
            string dpi = txtDPI.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || nombre == "Nombre" ||
                string.IsNullOrWhiteSpace(direccion) || direccion == "Dirección")
            {
                MessageBox.Show("Ingrese nombre y dirección del cliente.");
                return;
            }

            bool requiereNit = !string.IsNullOrWhiteSpace(nit) && nit != "NIT";
            if (requiereNit && (dpi.Length != 13 || !dpi.All(char.IsDigit)))
            {
                MessageBox.Show("El DPI debe tener exactamente 13 dígitos numéricos.");
                return;
            }

            // Preparar factura
            var factura = new Factura
            {
                NombreEmpresa = "TecnoShop GT",
                DireccionEmpresa = "12 Calle 1-25 Zona 10, Ciudad de Guatemala",
                NitEmpresa = "123456-7",
                Codigo = $"FAC-{DateTimeOffset.Now.ToUnixTimeSeconds()}-{new Random().Next(1000)}",
                ClienteId = 0,
                CfClienteId = 0,
                SubtotalSinIva = subtotalSinIVA,
                Subtotal = subtotal,
                Iva = iva,
                Total = total,
                RequiereNit = requiereNit,
                Productos = new List<Producto>(productosSeleccionados)
            };

            // Guardar factura
            bool exito = facturaManager.GuardarFactura(factura, nombre, nit, direccion, dpi);

            if (exito)
            {
                // Reducir existencias
                foreach (var p in productosSeleccionados)
                {
                    inventarioManager.ReducirExistencias(p.Id, p.Existencia);
                }
                MessageBox.Show($"Factura emitida y guardada con éxito.\nCódigo: {factura.Codigo}");
                productosSeleccionados.Clear();
                ActualizarFacturaGrid();
                CargarInventario();
            }
            else
            {
                MessageBox.Show("Error al guardar la factura.");
            }
        }
    }
}
