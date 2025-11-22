using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Laboratorio_4_DSIV
{
    public partial class FormConsultarPedidos : Form
    {
        Class1 conexion = new Class1();

        public FormConsultarPedidos()
        {
            InitializeComponent();
            // Asegúrate que el evento Load está enlazado (ver abajo en notas)
            this.Load += FormConsultarPedidos_Load;
        }

        private void FormConsultarPedidos_Load(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            try
            {
                conexion.conectar();

                // Consulta simple usando la tabla 'pedidos' que tú creaste
                string query = @"
                    SELECT 
                        id_pedido,
                        cliente,
                        medicamento,
                        cantidad,
                        total,
                        fecha
                    FROM pedidos
                    ORDER BY fecha DESC;
                ";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conexion.getMiConexion());
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Depuración: cuántas filas llegaron
                MessageBox.Show("Filas obtenidas: " + dt.Rows.Count, "Depuración");

                // Asegúrate que el DataGridView en el diseñador se llame exactamente dgvPedidos
                dgvPedidos.AutoGenerateColumns = true;
                dgvPedidos.DataSource = dt;

                // Ajuste visual
                dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pedidos: " + ex.Message);
            }
            finally
            {
                conexion.cerrarConexion();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
            Administrar f = new Administrar();
            f.Show();
        }




    }
}


