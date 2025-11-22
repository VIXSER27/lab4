using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratorio_4_DSIV
{
    public partial class Farmacia : Form
    {
        private List<ItemCarrito> carrito = new List<ItemCarrito>();

        public Farmacia()
        {
            InitializeComponent();
            CargarProductos();
        }

        public void RefrescarCatalogo()
        {
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

                    string query = "SELECT id, nombre, imagen, cantidad_disponible, precio_unitario " +
                                   "FROM medicamentos WHERE cantidad_disponible > 0";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Panel panel = CrearPanelProducto(reader);
                            flowLayoutPanelCatalogo.Controls.Add(panel);
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
            int id = Convert.ToInt32(reader["id"]);
            string nombre = reader["nombre"].ToString();
            decimal precio = Convert.ToDecimal(reader["precio_unitario"]);
            int cantidad = Convert.ToInt32(reader["cantidad_disponible"]);
            string imagenPath = reader["imagen"].ToString();

            Panel card = new Panel();
            card.Width = 220;
            card.Height = 310;
            card.Margin = new Padding(10);
            card.BackColor = Color.White;
            card.Padding = new Padding(8);

           
            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.LightGray, 2, ButtonBorderStyle.Solid,
                    Color.LightGray, 2, ButtonBorderStyle.Solid,
                    Color.LightGray, 2, ButtonBorderStyle.Solid,
                    Color.LightGray, 2, ButtonBorderStyle.Solid);
            };

        
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 245, 245);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

       
            PictureBox pic = new PictureBox();
            pic.Width = 200;
            pic.Height = 150;
            pic.Top = 5;
            pic.Left = 5;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.BackColor = Color.FromArgb(250, 250, 250);

            try
            {
                if (!string.IsNullOrWhiteSpace(imagenPath))
                {
             
                    string rutaImagen = Path.Combine(Application.StartupPath, "Img", imagenPath);

                    if (File.Exists(rutaImagen))
                    {
                        pic.Image = Image.FromFile(rutaImagen);
                    }
                    else
                    {
                   
                        pic.Image = Image.FromFile(
                            Path.Combine(Application.StartupPath, "Img", "imagen_no_disponible.png")
                        );
                    }
                }
                else
                {
                    pic.Image = Image.FromFile(
                        Path.Combine(Application.StartupPath, "Img", "imagen_no_disponible.png")
                    );
                }
            }
            catch
            {
                pic.Image = Image.FromFile(
                    Path.Combine(Application.StartupPath, "Img", "imagen_no_disponible.png")
                );
            }

            card.Controls.Add(pic);

         
            Label lblNombre = new Label();
            lblNombre.Text = nombre;
            lblNombre.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblNombre.Width = 200;
            lblNombre.Top = 165;
            lblNombre.Left = 5;
            lblNombre.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblNombre);

           
            Label lblPrecio = new Label();
            lblPrecio.Text = $"${precio:F2}";
            lblPrecio.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPrecio.ForeColor = Color.FromArgb(0, 120, 215);
            lblPrecio.Width = 200;
            lblPrecio.Top = 190;
            lblPrecio.Left = 5;
            lblPrecio.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblPrecio);

           
            Label lblStock = new Label();
            lblStock.Text = $"Stock disponible: {cantidad}";
            lblStock.Font = new Font("Segoe UI", 9);
            lblStock.Width = 200;
            lblStock.Top = 210;
            lblStock.Left = 5;
            lblStock.TextAlign = ContentAlignment.MiddleCenter;
            lblStock.ForeColor = Color.Gray;
            card.Controls.Add(lblStock);

            
            NumericUpDown nudCantidad = new NumericUpDown();
            nudCantidad.Minimum = 1;
            nudCantidad.Maximum = cantidad;
            nudCantidad.Width = 70;
            nudCantidad.Top = 235;
            nudCantidad.Left = 75;
            card.Controls.Add(nudCantidad);

           
            Button btn = new Button();
            btn.Text = "Agregar";
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Width = 180;
            btn.Height = 32;
            btn.Top = 270;
            btn.Left = 20;
            btn.BackColor = Color.FromArgb(0, 150, 90);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

           
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 120, 70);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(0, 150, 90);

            btn.Click += (s, e) =>
            {
                carrito.Add(new ItemCarrito()
                {
                    Id = id,
                    Nombre = nombre,
                    Cantidad = (int)nudCantidad.Value,
                    PrecioUnitario = precio
                });

                MessageBox.Show($"{nudCantidad.Value}x {nombre} agregado al carrito.");
            };

            card.Controls.Add(btn);

            return card;
        }

        private void btnCarrito_Click(object sender, EventArgs e)
        {
            Carrito frm = new Carrito(carrito, this);
            frm.ShowDialog();
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            l.ShowDialog();

        }
    }
}
