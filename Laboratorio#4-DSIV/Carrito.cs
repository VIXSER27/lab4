using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Laboratorio_4_DSIV
{
    public partial class Carrito : Form
    {
        private List<ItemCarrito> carrito;
        private Farmacia formFarmacia;

        public Carrito(List<ItemCarrito> carrito, Farmacia farmacia)
        {
            InitializeComponent();
            this.carrito = carrito;
            this.formFarmacia = farmacia;

            MostrarCarrito();
            CalcularTotal();
        }

        private void MostrarCarrito()
        {
            flowLayoutPanelCarrito.Controls.Clear();

            foreach (ItemCarrito item in carrito)
            {
                Panel card = new Panel();
                card.Width = 320;
                card.Height = 160;
                card.Margin = new Padding(10);
                card.BackColor = Color.White;
                card.Padding = new Padding(10);

                // Sombra ligera
                card.Paint += (s, e) =>
                {
                    ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                        Color.LightGray, 2, ButtonBorderStyle.Solid,
                        Color.LightGray, 2, ButtonBorderStyle.Solid,
                        Color.LightGray, 2, ButtonBorderStyle.Solid,
                        Color.LightGray, 2, ButtonBorderStyle.Solid);
                };

                // Hover
                card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 245, 245);
                card.MouseLeave += (s, e) => card.BackColor = Color.White;

                // Nombre
                Label lblNombre = new Label();
                lblNombre.Text = item.Nombre;
                lblNombre.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblNombre.Width = 300;
                lblNombre.Left = 10;
                lblNombre.Top = 10;

                // Precio
                Label lblPrecio = new Label();
                lblPrecio.Text = $"Precio unit.: ${item.PrecioUnitario:F2}";
                lblPrecio.Font = new Font("Segoe UI", 9);
                lblPrecio.ForeColor = Color.FromArgb(0, 120, 215);
                lblPrecio.Left = 10;
                lblPrecio.Top = 45;

                // Cantidad
                Label lblCantidad = new Label();
                lblCantidad.Text = $"Cantidad: {item.Cantidad}";
                lblCantidad.Font = new Font("Segoe UI", 9);
                lblCantidad.Left = 10;
                lblCantidad.Top = 70;

                // Subtotal
                Label lblSubtotal = new Label();
                lblSubtotal.Text = $"Subtotal: ${item.Subtotal:F2}";
                lblSubtotal.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblSubtotal.Left = 10;
                lblSubtotal.Top = 100;

                // Botón eliminar
                Button btnEliminar = new Button();
                btnEliminar.Text = "Eliminar";
                btnEliminar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btnEliminar.Size = new Size(90, 35);
                btnEliminar.Left = 210;
                btnEliminar.Top = 100;
                btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
                btnEliminar.ForeColor = Color.White;
                btnEliminar.FlatStyle = FlatStyle.Flat;
                btnEliminar.FlatAppearance.BorderSize = 0;

                btnEliminar.MouseEnter += (s, e) => btnEliminar.BackColor = Color.FromArgb(190, 40, 55);
                btnEliminar.MouseLeave += (s, e) => btnEliminar.BackColor = Color.FromArgb(220, 53, 69);

                btnEliminar.Click += (s, e) =>
                {
                    carrito.Remove(item);
                    MostrarCarrito();
                    CalcularTotal();
                };

                card.Controls.Add(lblNombre);
                card.Controls.Add(lblPrecio);
                card.Controls.Add(lblCantidad);
                card.Controls.Add(lblSubtotal);
                card.Controls.Add(btnEliminar);

                flowLayoutPanelCarrito.Controls.Add(card);
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0;

            foreach (ItemCarrito item in carrito)
            {
                total += item.Subtotal;
            }

            lblTotal.Text = "Total: $" + total.ToString("0.00");
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            formFarmacia.Show();
            this.Close();
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    foreach (ItemCarrito item in carrito)
                    {
                        string updateQuery =
                            "UPDATE medicamentos " +
                            "SET cantidad_disponible = cantidad_disponible - @cantidad " +
                            "WHERE id = @id";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, conexion.getMiConexion()))
                        {
                            cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                            cmd.Parameters.AddWithValue("@id", item.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Compra realizada correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar stock: " + ex.Message);
                }
            }

            // Vaciar carrito visualmente y en memoria
            carrito.Clear();
            MostrarCarrito();
            CalcularTotal();

            // Volver a farmacia y actualizar productos
            formFarmacia.Show();
            formFarmacia.Focus();
            formFarmacia.RefrescarCatalogo();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            l.ShowDialog();
        }
    }
}
