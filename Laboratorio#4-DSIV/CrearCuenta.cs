using Npgsql;
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
    public partial class CrearCuenta : Form
    {
        public CrearCuenta()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string nuevoUsuario = txtNuevoUsuario.Text.Trim();
            string nuevaContrasena = txtNuevaContraseña.Text.Trim();

            // Validación correcta
            if (nuevoUsuario == "" || nuevaContrasena == "")
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    // Primero verificamos si el usuario ya existe
                    string checkQuery = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario";

                    using (NpgsqlCommand checkCmd = new NpgsqlCommand(checkQuery, conexion.getMiConexion()))
                    {
                        checkCmd.Parameters.AddWithValue("@usuario", nuevoUsuario);

                        long existe = (long)checkCmd.ExecuteScalar();

                        if (existe > 0)
                        {
                            MessageBox.Show("El usuario ya existe. Elija otro nombre.");
                            return;
                        }
                    }

                    // Registrar nuevo usuario
                    string insertQuery =
                        "INSERT INTO usuarios (usuario, contraseña, rol) VALUES (@usuario, @contrasena, 'user')";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", nuevoUsuario);
                        cmd.Parameters.AddWithValue("@contrasena", nuevaContrasena);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuario registrado correctamente.");

                    // Redirigir a farmacia
                    Farmacia farmacia = new Farmacia();
                    farmacia.WindowState = FormWindowState.Maximized;
                    farmacia.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar usuario: " + ex.Message);
                }
            }
        }
    }
}