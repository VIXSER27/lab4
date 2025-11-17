using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Laboratorio_4_DSIV
{
    public partial class FormConsultarPedidos : Form
    {
        // Usamos tu clase de conexión
        Class1 conexion = new Class1();

        public FormConsultarPedidos()
        {
            InitializeComponent();
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

                string query = @"
                    SELECT 
                        p.id_pedido,
                        c.nombre AS cliente,
                        m.nombre AS medicamento,
                        d.cantidad,
                        d.total,
                        p.fecha
                    FROM pedidos p
                    INNER JOIN pedido_detalle d ON p.id_pedido = d.id_pedido
                    INNER JOIN clientes c ON c.id_cliente = p.id_cliente
                    INNER JOIN medicamentos m ON m.id_medicamento = d.id_medicamento
                    ORDER BY p.fecha DESC;
                ";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conexion.getMiConexion());
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPedidos.DataSource = dt;
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
            Administrar f = new Administrar(); // Tu formulario principal del farmacéutico
            f.Show();
        }
    }
}
