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

                    string query = "SELECT rol FROM usuarios " +
                        " WHERE usuario = @usuario AND contrasena = @contrasena ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contrasena", contraseña);

                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos.");
                                return;
                            }

                            string rol = dr["rol"].ToString().ToLower();

                            if (rol == "admin" || rol == "farmaceutico")
                            {
                                MessageBox.Show("Bienvenido Administrador");
                                new Administrar().Show();
                            }
                            else
                            {
                                MessageBox.Show("Bienvenido Usuario");

                            
                                Sesion.UsuarioActual = usuario;

                                Farmacia farmacia = new Farmacia();
                                farmacia.WindowState = FormWindowState.Maximized;
                                farmacia.Show();
                            }


                            this.Hide();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);

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
                            Sesion.UsuarioActual = usuario;
                            Farmacia farmacia = new Farmacia();
                            farmacia.WindowState = FormWindowState.Maximized;
                            farmacia.Show();
                        }

                        this.Hide();
                    }
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