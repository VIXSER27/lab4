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
using System.Xml;

namespace Laboratorio_4_DSIV
{
    public partial class Administrar : Form
    {
        public Administrar()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            CargarInventario();
        }
        private void ConfigurarDataGridView()
        {
            dgvInventario.Columns.Clear();

            dgvInventario.Columns.Add("id", "ID");
            dgvInventario.Columns.Add("nombre", "Nombre");
            dgvInventario.Columns.Add("imagen", "Imagen");
            dgvInventario.Columns.Add("cantidad", "Cantidad Disponible");
            dgvInventario.Columns.Add("precio", "Precio por Unidad");

            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.ReadOnly = true;
            dgvInventario.AllowUserToAddRows = false;

            dgvInventario.CellClick += dgvInventario_CellClick;
        }
        private void CargarInventario()
        {
            dgvInventario.Rows.Clear();

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string query =
                        "SELECT id, nombre, imagen, cantidad_disponible, precio_unitario FROM medicamentos ORDER BY id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvInventario.Rows.Add(
                                reader["id"],
                                reader["nombre"],
                                reader["imagen"],
                                reader["cantidad_disponible"],
                                reader["precio_unitario"]
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar inventario: " + ex.Message);
                }
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string sql =
                        "INSERT INTO medicamentos (nombre, imagen, cantidad_disponible, precio_unitario) " +
                        "VALUES (@nombre, @imagen, @cantidad, @precio)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@imagen", txtImagen.Text);
                        cmd.Parameters.AddWithValue("@cantidad", (int)nudCantidad.Value);
                        cmd.Parameters.AddWithValue("@precio", nudPrecio.Value);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Medicamento agregado correctamente ");
                    CargarInventario();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar: " + ex.Message);
                }
            }
        }
        private void dgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtId.Text = dgvInventario.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtNombre.Text = dgvInventario.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtImagen.Text = dgvInventario.Rows[e.RowIndex].Cells[2].Value.ToString();

                int cantidad = Convert.ToInt32(dgvInventario.Rows[e.RowIndex].Cells[3].Value);

                if (cantidad > nudCantidad.Maximum)
                    nudCantidad.Maximum = cantidad;

                nudCantidad.Value = cantidad;


                nudPrecio.Value = Convert.ToDecimal(dgvInventario.Rows[e.RowIndex].Cells[4].Value);
            }
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un medicamento para modificar.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "¿Está seguro que desea modificar este medicamento?",
                "Confirmar modificación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes) return;

            int id = Convert.ToInt32(dgvInventario.SelectedRows[0].Cells["id"].Value);
            string nombre = txtNombre.Text.Trim();
            string imagen = txtImagen.Text.Trim();
            int cantidad = int.Parse(nudCantidad.Text);
            decimal precio = decimal.Parse(nudPrecio.Text);

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();
                    string query =
                        "UPDATE medicamentos SET nombre=@nombre, imagen=@imagen, cantidad_disponible=@cantidad, precio_unitario=@precio WHERE id=@id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@imagen", imagen);
                        cmd.Parameters.AddWithValue("@cantidad", cantidad);
                        cmd.Parameters.AddWithValue("@precio", precio);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Medicamento modificado correctamente.");
                    CargarInventario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar: " + ex.Message);
                }
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un medicamento para eliminar.");
                return;
            }

            int id;
            try
            {

                id = Convert.ToInt32(dgvInventario.SelectedRows[0].Cells["id"].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo obtener el ID del medicamento seleccionado.");
                return;
            }


            DialogResult confirm = MessageBox.Show(
               "¿Está seguro que desea eliminar este medicamento?",
                 "Confirmar eliminación",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning
             );

            if (confirm != DialogResult.Yes) return;

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();
                    string query = "DELETE FROM medicamentos WHERE id = @id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Medicamento eliminado correctamente.");

                    CargarInventario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }
        private void btnReabastecer_Click(object sender, EventArgs e)
        {
            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string sql =
                        "UPDATE medicamentos SET cantidad_disponible = cantidad_disponible + @extra WHERE id=@id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@extra", (int)nudCantidad.Value);
                        cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Inventario reabastecido ");
                    CargarInventario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al reabastecer: " + ex.Message);
                }
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtImagen.Clear();
            nudCantidad.Value = 0;
            nudPrecio.Value = 0;
        }
        private void btnVolverLogin_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            FormConsultarPedidos f = new FormConsultarPedidos();
            f.Show();
            this.Hide();

        }
    }
}
