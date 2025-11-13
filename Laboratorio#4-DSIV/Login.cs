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
    public partial class Login : Form
    {
        private bool showContraseña = false;

        public Login()
        {
            InitializeComponent();
        }

       

        private void BtmAcceder_Click(object sender, EventArgs e)
        {
            String usuario = txtUsuario.Text;
            String contraseña = txtContraseña.Text;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.Open();
                String query = "SELECT COUNT(1) FROM Usuarios WHERE Usuario=@Usuario AND Contraseña=@Contraseña";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                var tipoUsuario = cmd.ExecuteScalar();
                if (tipoUsuario != null)
                {
                    String tipo = tipoUsuario.ToString();

                    if (tipo == "Farmaceutico")
                    {
                        
                        this.Hide();
                        MessageBox.Show("Bienvenido Administrador");
                        
                    }
                    else if (tipo == "Cliente")
                    {
                       
                        this.Hide();
                        MessageBox.Show("Bienvenido Usuario");
                        
                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas");
                    }

                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
         
          

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            showContraseña = !showContraseña;
            txtContraseña.PasswordChar = '\0';

            if (showContraseña)
            {
                pictureBox1.Image = Properties.Resources.hide;
            }
            else
            {
                txtContraseña.PasswordChar = '*';
                pictureBox1.Image = Properties.Resources.show; 
            }

        }
    }
}
