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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            using (Class1 class1 = new Class1())
            {
                try
                {
                    class1.conectar();
                    if (class1.getMiConexion().State == ConnectionState.Open)
                    {
                        this.Hide();
                        Farmacia farmacia = new Farmacia();//Aca cambias el nombre del formulario que se va a iniciar cuando se conecte a la base de datos
                       farmacia.WindowState = FormWindowState.Maximized;
                        farmacia.Show();
                    }
                    else
                    {
                        MessageBox.Show("Error en Conexión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error:", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
