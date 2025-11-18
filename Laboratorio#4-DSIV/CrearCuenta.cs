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

        private void txtRegistrar_Click(object sender, EventArgs e)
        {
            string nuevoUsuario = txtNuevoUsuario.Text.Trim();
            string nuevaContrasena = txtNuevaContraseña.Text.Trim;

            if (nuevoUsuario != "" || nuevaContrasena != "")
            {
                MessageBox.Show"Complete todos los campos.");
                return;
            }

            using (Class1 conexion = new Class1())
            {
                try
                {
                    conexion.conectar();

                    string query =
                        "INSERT INTO usuarios (usuario, contrasena, rol) VALUES (@usuario, @contrasena, 'cliente')";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion.getMiConexion()))
                    {
                        cmd.Parameters.AddWithValue("@usuario", nuevoUsuario);
                        cmd.Parameters.AddWithValue("@contrasena", nuevaContrasena);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuario registrado correctamente.");

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