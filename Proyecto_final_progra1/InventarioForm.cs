using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_final_progra1.Logic;
using Proyecto_final_progra1.Models;

namespace Proyecto_final_progra1
{
    public partial class InventarioForm : Form
    {
        private InventarioManager inventarioManager;
        private DataGridView dgvInventario;
        private Button btnRecargar, btnBuscar, btnAgregar, btnActualizar;
        private TextBox txtBuscarId, txtNombre, txtDescripcion, txtPrecio, txtExistencia, txtActualizarId, txtNuevaExistencia;

        public InventarioForm()
        {
            InitializeComponent();
            inventarioManager = new InventarioManager();
            InicializarControles();
            CargarInventario();
        }

        private void InicializarControles()
        {
            // DataGridView
            dgvInventario = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 250,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(dgvInventario);

            // Recargar
            btnRecargar = new Button { Text = "Recargar Inventario", Top = 260, Left = 10, Width = 150 };
            btnRecargar.Click += (s, e) => CargarInventario();
            Controls.Add(btnRecargar);

            // Buscar por ID
            txtBuscarId = new TextBox { Top = 260, Left = 180, Width = 50 };
            txtBuscarId.Text = "ID";
            txtBuscarId.GotFocus += (s, e) => { if (txtBuscarId.Text == "ID") txtBuscarId.Text = ""; };
            txtBuscarId.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtBuscarId.Text)) txtBuscarId.Text = "ID"; };
            btnBuscar = new Button { Text = "Buscar", Top = 260, Left = 240, Width = 80 };
            btnBuscar.Click += BtnBuscar_Click;
            Controls.Add(txtBuscarId);
            Controls.Add(btnBuscar);

            // Agregar producto
            txtNombre = new TextBox { Top = 300, Left = 10, Width = 100 };
            txtNombre.Text = "Nombre";
            txtNombre.GotFocus += (s, e) => { if (txtNombre.Text == "Nombre") txtNombre.Text = ""; };
            txtNombre.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtNombre.Text)) txtNombre.Text = "Nombre"; };

            txtDescripcion = new TextBox { Top = 300, Left = 120, Width = 120 };
            txtDescripcion.Text = "Descripción";
            txtDescripcion.GotFocus += (s, e) => { if (txtDescripcion.Text == "Descripción") txtDescripcion.Text = ""; };
            txtDescripcion.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtDescripcion.Text)) txtDescripcion.Text = "Descripción"; };

            txtPrecio = new TextBox { Top = 300, Left = 250, Width = 60 };
            txtPrecio.Text = "Precio";
            txtPrecio.GotFocus += (s, e) => { if (txtPrecio.Text == "Precio") txtPrecio.Text = ""; };
            txtPrecio.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtPrecio.Text)) txtPrecio.Text = "Precio"; };

            txtExistencia = new TextBox { Top = 300, Left = 320, Width = 60 };
            txtExistencia.Text = "Existencia";
            txtExistencia.GotFocus += (s, e) => { if (txtExistencia.Text == "Existencia") txtExistencia.Text = ""; };
            txtExistencia.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtExistencia.Text)) txtExistencia.Text = "Existencia"; };

            btnAgregar = new Button { Text = "Agregar", Top = 300, Left = 390, Width = 80 };
            btnAgregar.Click += BtnAgregar_Click;
            Controls.Add(txtNombre);
            Controls.Add(txtDescripcion);
            Controls.Add(txtPrecio);
            Controls.Add(txtExistencia);
            Controls.Add(btnAgregar);

            // Actualizar existencias
            txtActualizarId = new TextBox { Top = 340, Left = 10, Width = 50 };
            txtActualizarId.Text = "ID";
            txtActualizarId.GotFocus += (s, e) => { if (txtActualizarId.Text == "ID") txtActualizarId.Text = ""; };
            txtActualizarId.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtActualizarId.Text)) txtActualizarId.Text = "ID"; };

            txtNuevaExistencia = new TextBox { Top = 340, Left = 70, Width = 60 };
            txtNuevaExistencia.Text = "Nueva Existencia";
            txtNuevaExistencia.GotFocus += (s, e) => { if (txtNuevaExistencia.Text == "Nueva Existencia") txtNuevaExistencia.Text = ""; };
            txtNuevaExistencia.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtNuevaExistencia.Text)) txtNuevaExistencia.Text = "Nueva Existencia"; };

            btnActualizar = new Button { Text = "Actualizar Existencia", Top = 340, Left = 140, Width = 150 };
            btnActualizar.Click += BtnActualizar_Click;
            Controls.Add(txtActualizarId);
            Controls.Add(txtNuevaExistencia);
            Controls.Add(btnActualizar);

            this.Height = 430;
        }

        private void CargarInventario()
        {
            try
            {
                dgvInventario.DataSource = inventarioManager.ObtenerInventario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar inventario: " + ex.Message);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtBuscarId.Text, out int id))
            {
                var producto = inventarioManager.ObtenerProductoPorId(id);
                if (producto != null)
                {
                    MessageBox.Show($"ID: {producto.Id}\nNombre: {producto.Nombre}\nDescripción: {producto.Descripcion}\nPrecio: {producto.Precio}\nExistencia: {producto.Existencia}", "Producto encontrado");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || txtNombre.Text == "Nombre" ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) || txtDescripcion.Text == "Descripción" ||
                !double.TryParse(txtPrecio.Text, out double precio) ||
                !int.TryParse(txtExistencia.Text, out int existencia))
            {
                MessageBox.Show("Datos inválidos para agregar producto.");
                return;
            }

            var producto = new Producto
            {
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Precio = precio,
                Existencia = existencia
            };

            if (inventarioManager.AgregarProducto(producto))
            {
                MessageBox.Show("Producto agregado exitosamente.");
                CargarInventario();
            }
            else
            {
                MessageBox.Show("Error al agregar producto.");
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtActualizarId.Text, out int id) && int.TryParse(txtNuevaExistencia.Text, out int nuevaExistencia))
            {
                if (inventarioManager.ActualizarExistencias(id, nuevaExistencia))
                {
                    MessageBox.Show("Existencia actualizada.");
                    CargarInventario();
                }
                else
                {
                    MessageBox.Show("Error al actualizar existencia.");
                }
            }
        }
    }
}
