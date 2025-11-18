using Npgsql;
using System;
using System.Collections;
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
    public partial class Login : Form
    {
        private bool showContraseña = true;

        public Login()
        {
            InitializeComponent();
        }

        private void BtmAcceder_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();

            if (usuario == "")
            {
                MessageBox.Show("Ingrese el usuario.");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string query = "SELECT rol FROM usuarios WHERE usuario = @usuario";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        object rolObj = cmd.ExecuteScalar();

                        if (rolObj == null)
                        {
                            MessageBox.Show("Usuario no encontrado.");
                            return;
                        }

                        string rol = rolObj.ToString().ToLower();

                        if (rol == "admin")
                        {
                            MessageBox.Show("Bienvenido Administrador");
                            Administrar admin = new Administrar();
                            admin.WindowState = FormWindowState.Maximized;
                            admin.Show();
                        }
                        else
                        {
                            MessageBox.Show("Bienvenido Usuario");
                            Farmacia farmacia = new Farmacia();
                            farmacia.WindowState = FormWindowState.Maximized;
                            farmacia.Show();
                        }

                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
