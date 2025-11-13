using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_4_DSIV
{
    public partial class Farmacia : Form
    {
        public Farmacia()
        {
            InitializeComponent();
            CargarProductos();
        }
        private void CargarProductos()
        {
            flowLayoutPanelCatalogo.Controls.Clear();

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string query = "SELECT nombre, precio, cantidad, imagen FROM medicamentos WHERE cantidad > 0";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Panel panel = CrearPanelProducto(reader);
                                flowLayoutPanelCatalogo.Controls.Add(panel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }
        }

        private Panel CrearPanelProducto(NpgsqlDataReader reader)
        {
            // Datos del producto
            string nombre = reader["nombre"].ToString();
            decimal precio = Convert.ToDecimal(reader["precio"]);
            int cantidad = Convert.ToInt32(reader["cantidad"]);
            string imagenPath = reader["imagen"].ToString(); // Ruta de la imagen guardada

            // Panel del producto
            Panel panel = new Panel();
            panel.Width = 200;
            panel.Height = 270;
            panel.Margin = new Padding(10);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.BackColor = Color.White;

            // Imagen del producto
            PictureBox pic = new PictureBox();
            try
            {
                pic.Image = Image.FromFile(imagenPath);
            }
            catch
            {
                pic.Image = null;
            }
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Width = 180;
            pic.Height = 120;
            pic.Top = 10;
            pic.Left = 10;
            panel.Controls.Add(pic);

            // Nombre del producto
            Label lblNombre = new Label();
            lblNombre.Text = nombre;
            lblNombre.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblNombre.Top = 140;
            lblNombre.Left = 10;
            lblNombre.Width = 180;
            lblNombre.TextAlign = ContentAlignment.MiddleCenter;
            panel.Controls.Add(lblNombre);

            // Precio
            Label lblPrecio = new Label();
            lblPrecio.Text = $"Precio: ${precio:F2}";
            lblPrecio.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblPrecio.ForeColor = Color.FromArgb(0, 120, 215);
            lblPrecio.Top = 165;
            lblPrecio.Left = 10;
            lblPrecio.Width = 180;
            lblPrecio.TextAlign = ContentAlignment.MiddleCenter;
            panel.Controls.Add(lblPrecio);

            // Selector de cantidad
            NumericUpDown nudCantidad = new NumericUpDown();
            nudCantidad.Minimum = 1;
            nudCantidad.Maximum = cantidad;
            nudCantidad.Top = 190;
            nudCantidad.Left = 60;
            nudCantidad.Width = 80;
            panel.Controls.Add(nudCantidad);

            // Botón agregar
            Button btnAgregar = new Button();
            btnAgregar.Text = "Agregar";
            btnAgregar.BackColor = Color.FromArgb(40, 167, 69);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnAgregar.Top = 225;
            btnAgregar.Left = 25;
            btnAgregar.Width = 150;

            // Efecto hover
            btnAgregar.MouseEnter += (s, e) => btnAgregar.BackColor = Color.FromArgb(30, 140, 60);
            btnAgregar.MouseLeave += (s, e) => btnAgregar.BackColor = Color.FromArgb(40, 167, 69);

            // Evento del botón
            btnAgregar.Click += (s, e) =>
            {
                int cantidadSeleccionada = (int)nudCantidad.Value;
                MessageBox.Show($"Agregado {cantidadSeleccionada}x {nombre} al carrito ✅", "Carrito");
            };

            panel.Controls.Add(btnAgregar);

            return panel;
        }

    }
}
