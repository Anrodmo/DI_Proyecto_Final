using DI_Proyecyo_Final.Modelo;
using DI_Proyecyo_Final.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DI_Proyecyo_Final.Services.DataAccess
{
    /// <summary>
    /// Clase que manjeja la comunucación con la BBDD en el proceso de lopgin de los listaUsuarios
    /// </summary>
    internal class LoginService
    {



        /// <summary>
        /// Método que dado un nombre de usuario y un hash de una contraseña los envía a la BBDD y recoje si el login es correcto
        /// </summary>
        /// <param name="usuario"> Nombre de usuario</param>
        /// <param name="passwordHash">  Hash512  de la contraseña</param>
        /// <returns> True: Si y solo si el usuario existe y su contraseña almacenada en la BBDD se correspondecon la facilitada.
        /// False: En caso contrario.</returns>
        static public bool validarLogin(string usuario, string passwordHash)
        {
            bool resultado = false;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand("VerificarContraseña2", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Defino el valor de los parámetros que envío al procedimiento de la BBDD
                        cmd.Parameters.AddWithValue("@p_usuario", usuario);
                        cmd.Parameters.AddWithValue("@p_hash", passwordHash);

                        // Defino el parámetro que recogerá la salida del procedimento de la BBDD
                        MySqlParameter resultadoParam = new MySqlParameter("@p_resultado", MySqlDbType.Bit);
                        resultadoParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(resultadoParam);

                        // ejecuto la consulta a la BBDD
                        cmd.ExecuteNonQuery();

                        // Recojo el resultado de la ejecución del procedimiento de la BBDD
                        resultado = Convert.ToBoolean(cmd.Parameters["@p_resultado"].Value);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show("Error de conexión con la BBDD","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            return resultado;
        }

        /*=================  sql ==========================*/
        /*

                       DELIMITER //

                            CREATE PROCEDURE VerificarContraseña(
                                IN p_usuario VARCHAR(50),
                                IN p_hash VARCHAR(128),
                                OUT p_resultado BOOLEAN
                            )
                            BEGIN
                                DECLARE hash_en_base_datos VARCHAR(128);

                                -- Obtener el hash almacenado en la base de datos para el usuario dado
                                SELECT contraseña INTO hash_en_base_datos
                                FROM usuarios
                                WHERE nombre = p_usuario;

                                -- Comparar el hash proporcionado con el hash almacenado
                                IF hash_en_base_datos IS NOT NULL AND hash_en_base_datos = p_hash THEN
                                    -- Contraseñas coinciden
                                    SET p_resultado = TRUE;
                                ELSE
                                    -- Contraseñas no coinciden
                                    SET p_resultado = FALSE;
                                END IF;
                            END //

                        DELIMITER ;



        */


        /*
         DELIMITER //

                    CREATE PROCEDURE VerificarContraseña2(
                        IN p_usuario VARCHAR(50),
                        IN p_hash VARCHAR(128),
                        OUT p_resultado BOOLEAN
                    )
                    BEGIN
                        DECLARE hash_en_base_datos VARCHAR(128);
                        DECLARE usuario_activo BOOLEAN;

                        -- Obtener el hash almacenado en la base de datos y el estado activo para el usuario dado
                        SELECT contraseña, activo INTO hash_en_base_datos, usuario_activo
                        FROM usuarios
                        WHERE nombre = p_usuario;

                        -- Verificar si el usuario está activo
                        IF usuario_activo = TRUE THEN
                            -- Comparar el hash proporcionado con el hash almacenado
                            IF hash_en_base_datos IS NOT NULL AND hash_en_base_datos = p_hash THEN
                                -- Contraseñas coinciden
                                SET p_resultado = TRUE;
                            ELSE
                                -- Contraseñas no coinciden
                                SET p_resultado = FALSE;
                            END IF;
                        ELSE
                            -- Usuario no está activo
                            SET p_resultado = FALSE;
                        END IF;
                    END //

                    DELIMITER ;

         */



    }
}
