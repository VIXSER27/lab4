using Npgsql;
using System;
using System.Collections;
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
        private bool showContraseña = true;

        public Login()
        {
            InitializeComponent();
        }

        private void BtmAcceder_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();

            if (usuario == "" || contraseña == "")
            {
                MessageBox.Show("Ingrese todos los campos.");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    // 1️⃣ VALIDAMOS SI EL USUARIO Y CONTRASEÑA EXISTEN
                    string queryLogin =
                        "SELECT id FROM usuarios WHERE usuario = @usuario AND contrasena = @contrasena";

                    int usuarioId = -1;

                    using (NpgsqlCommand cmd = new NpgsqlCommand(queryLogin, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contraseña);

                        object res = cmd.ExecuteScalar();
                        usuarioId = res == null ? -1 : Convert.ToInt32(res);
                    }

                    if (usuarioId == -1)
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.");
                        return;
                    }

                    // 2️⃣ AHORA BUSCAMOS EL ROL POR SEPARADO
                    string queryRol = "SELECT rol FROM usuarios WHERE id = @id";

                    string rol = "";

                    using (NpgsqlCommand cmdRol = new NpgsqlCommand(queryRol, conexion.getMiConexion()))
                    {
                        cmdRol.Parameters.AddWithValue("@id", usuarioId);
                        rol = cmdRol.ExecuteScalar().ToString().ToLower();
                    }

                    // 3️⃣ REDIRECCIÓN SEGÚN ROL
                    switch (rol)
                    {
                        case "admin":
                            MessageBox.Show("Bienvenido Administrador");
                            Administrar admin = new Administrar();
                            admin.WindowState = FormWindowState.Maximized;
                            admin.Show();
                            break;

                        case "user":
                        case "cliente":
                            MessageBox.Show("Bienvenido Usuario");
                            Farmacia f = new Farmacia();
                            f.WindowState = FormWindowState.Maximized;
                            f.Show();
                            break;

                        default:
                            MessageBox.Show("Rol no reconocido: " + rol);
                            return;
                    }

                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            showContraseña = !showContraseña;

            if (showContraseña)
            {
                txtContraseña.PasswordChar = '\0';
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