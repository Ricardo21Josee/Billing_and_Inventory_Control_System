using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Proyecto_final_progra1.Logic;

namespace Proyecto_final_progra1
{
    public partial class Form1 : Form
    {
        private InventarioManager inventarioManager;
        private Button btnInventario;
        private Button btnFactura;
        private Button btnSalir;
        private Label lblTitulo;

        public Form1()
        {
            InitializeComponent();
            inventarioManager = new InventarioManager();
            InicializarPantallaInicio();
        }

        private void InicializarPantallaInicio()
        {
            // Título
            lblTitulo = new Label
            {
                Text = "Bienvenido a TechStore",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };
            Controls.Add(lblTitulo);

            // Panel para los botones
            var panelBotones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(0, 30, 0, 0)
            };
            Controls.Add(panelBotones);

            // Botón Gestión de Inventario
            btnInventario = new Button
            {
                Text = "Gestión de inventario",
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Left = 40
            };
            btnInventario.Click += BtnInventario_Click;
            panelBotones.Controls.Add(btnInventario);

            // Botón Emitir Factura
            btnFactura = new Button
            {
                Text = "Emitir Factura",
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Left = btnInventario.Right + 40
            };
            btnFactura.Click += BtnFactura_Click;
            panelBotones.Controls.Add(btnFactura);

            // Botón Salir
            btnSalir = new Button
            {
                Text = "Salir",
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Left = btnFactura.Right + 40
            };
            btnSalir.Click += BtnSalir_Click;
            panelBotones.Controls.Add(btnSalir);

            // Alinear los botones horizontalmente
            int totalWidth = btnInventario.Width + btnFactura.Width + btnSalir.Width + 80;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            btnInventario.Left = startX;
            btnFactura.Left = btnInventario.Right + 40;
            btnSalir.Left = btnFactura.Right + 40;

            // Ajustar el tamaño del formulario
            this.MinimumSize = new Size(650, 250);
        }

        private void BtnInventario_Click(object sender, EventArgs e)
        {
            using (var inventarioForm = new InventarioForm())
            {
                inventarioForm.ShowDialog();
            }
        }

        private void BtnFactura_Click(object sender, EventArgs e)
        {
            using (var facturaForm = new FacturaForm())
            {
                facturaForm.ShowDialog();
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}