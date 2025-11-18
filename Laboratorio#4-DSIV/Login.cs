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
        private bool showContraseña = false;

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

                    string query =
                        "SELECT rol FROM usuarios WHERE usuario = @usuario AND contrasena = @contrasena";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contraseña);

                        object resultado = cmd.ExecuteScalar();

                        if (resultado == null)
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.");
                            return;
                        }

                        string rol = resultado.ToString().ToLower();

                        switch (rol)
                        {
                            case "admin":
                                MessageBox.Show("Bienvenido Administrador");
                                Administrar administrar = new Administrar();
                                administrar.WindowState = FormWindowState.Maximized;
                                administrar.Show();
                                break;

                            case "user":
                            case "cliente":
                                MessageBox.Show("Bienvenido Usuario");
                                Farmacia farmacia = new Farmacia();
                                farmacia.WindowState = FormWindowState.Maximized;
                                farmacia.Show();
                                break;

                            default:
                                MessageBox.Show("Rol no reconocido.");
                                return;
                        }

                        this.Hide();
                    }
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