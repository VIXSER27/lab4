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
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            if (usuario == "" || contraseña == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña.");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string queryValidacion = "SELECT COUNT(*) FROM usuarios WHERE usuario = 'mayker' AND contraseña = '5577'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(queryValidacion, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contraseña", contraseña);

                        object result = cmd.ExecuteScalar();
                        Console.WriteLine(result);
                        int existe = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                        if (existe == 0)
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.");
                            return;
                        }
                    }

                        string queryRol = "SELECT rol FROM usuarios WHERE usuario = @usuario";
                        using (NpgsqlCommand cmdRo1 = new NpgsqlCommand(queryRol, conexion.getMiConexion()))
                        {
                            cmdRo1.Parameters.AddWithValue("@usuario", usuario);

                            string rol = cmdRo1.ExecuteScalar().ToString().ToLower();


                            if (rol == "admin" || rol == "farmaceutico")
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

        private void button1_Click(object sender, EventArgs e)
        {
            CrearCuenta crearCuenta = new CrearCuenta();
            crearCuenta.WindowState = FormWindowState.Maximized;
            crearCuenta.Show();

        }
    }
}
