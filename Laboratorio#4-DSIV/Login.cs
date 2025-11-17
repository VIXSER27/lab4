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

            if (usuario.Length != "" || contraseña.Length != "")
            {
                MessageBox.Show("Ingrese los campos ");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();
                    String query = "SELECT rol FROM usuarios WHERE usuario= @usuario AND contraseña=@contraseña";

                }


            using (NpgsqlCommand cmd = new NpgsqlConnection(Query, conexion.getMiconexion()))
                {

                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@contraseña", contraseña);

                    var resulatdo = cmd.ExecuteScalar();

                    if (resulatdo == null)
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos");
                        return;
                    }
                    else
                    {

                        string rol = resultado.ToString().ToLower;
                        switch (rol)
                        {
                            case "admin":
                                MessageBox.Show("Bienvenido Administrador");
                                Administrar administrar = new Administrar();
                                administrar.WindowState = FormWindowState.Maximized;
                                administrar.Show();
                                break;
                            case "user":
                                MessageBox.Show("Bienvenido Usuario");

                                Farmacia farmacia = new Farmacia();
                                farmacia.WindowState = FormWindowState.Maximized;
                                farmacia.Show();
                                break;
                            default:
                                MessageBox.Show("Rol no reconocido");
                                break;
                        }
                        this.Hide();
                    }

                }
                  catch (Exception ex)
                {
                    MessageBox.Show("Error de conexion a la base de datos: " + ex.Message);
                    return;
                }
            }
        }

    }
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            CrearCuenta CrearCuenta = new CrearCuenta();
            CrearCuenta.WindowState = FormWindowState.Maximized;
            CrearCuenta.Show();
            this.Hide();
        }
    }
}
