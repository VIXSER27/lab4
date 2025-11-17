using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratorio_4_DSIV
{
    public partial class Registro : Form
    {
        string connectionString = "TU CADENA SQL AQUI";

        public Registro()
        {
            InitializeComponent();
        }
        private void FormConsultarPedidos_Load(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
        SELECT 
            p.PedidoID,
            p.ClienteNombre,
            p.FechaPedido,
            m.Nombre AS Medicamento,
            d.Cantidad,
            (d.Cantidad * d.PrecioUnitario) AS Subtotal,
            p.Total
        FROM Pedidos p
        INNER JOIN PedidoDetalle d ON p.PedidoID = d.PedidoID
        INNER JOIN Medicamentos m ON d.MedicamentoID = m.ID
        ORDER BY p.PedidoID DESC;";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPedidos.DataSource = dt;
            }
        }

    }

}



