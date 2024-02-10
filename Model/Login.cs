using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DI_Proyecyo_Final.Services.DataAccess;
using System.Runtime.CompilerServices;
using System.Security;
using System.Runtime.InteropServices;

namespace DI_Proyecyo_Final.Model
{
    /// <summary>
    /// Clase que gestina la lógica del login de usuario, recoge los datos de la Vista y los transmmite al DataAcces y recoge la respuesta
    /// del DataAcces y transmite el resultado a la Vista.
    /// </summary>
    static internal class Login
    {
        

        /// <summary>
        /// Método que gestiona si un login de usuario es correcto o no. gestiona la transfomacion del ScureString de la contraseña
        /// en un hash Sha512 y lo envía a la capa de Servicios/DataAcces para que lo gestione con  la BBDD y recoge y trata su respuesta.
        /// </summary>
        /// <param name="usuario">nombre de usuario </param>
        /// <param name="contraseña">secureString con la contraseña</param>
        /// <returns>True: Si el login es correcto, False caso contrario.</returns>
        public static bool loginUsuario(string usuario, SecureString contraseña)
        {
            bool loginCorrecto = false; // por defecto la respuesta es no.
            string hashContraseña = CalcularHashSHA512(contraseña);

            loginCorrecto = LoginService.validarLogin(usuario, hashContraseña); // comunico con BBDD y recibo respuesta

            if (loginCorrecto)
            {
                Usuario usuarioActivo = UsuarioDataAcces.getUsuario(usuario);
                if(usuarioActivo != null) // será null si hay algún error de conexión.
                {
                    Sesion.inciarSesion(usuarioActivo);                 
                }
                else // Si ha habido algún problema que ha impedido obtener el usuario (i.e:se bloquea el paquete con los datos)
                {
                    loginCorrecto=false; //  se considera el login incorrecto
                }
            }
            else
            {
                //intentos++;
            }
            return loginCorrecto;
        }

        /// <summary>
        /// Metodo que dada una cadena de entrada la convierte en bytes, genera su hash Sha512 y devuelve el hash en una cadena hexadecimal
        /// </summary>
        /// <param name="contraseña"> SecureString del  que se quiere obtener el hash Sha512</param>
        /// <returns> Cadenahexadecinal que es un hash Sha512</returns>
        static internal string CalcularHashSHA512(SecureString contraseña)
        {
            IntPtr puntero = IntPtr.Zero; // inicializo un puntero a cero
            byte[] hashBytes;
            using (SHA512 sha512 = SHA512.Create())
            {   // lo hago directamente, asi no almaceno la pass en un array de bytes en plano            
                //byte[] inputBytes = Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(puntero));
                try
                {
                    puntero = Marshal.SecureStringToGlobalAllocUnicode(contraseña); // le digo al puntero donde esta la secure password  en bytes planos ya
                       // creo hash de bytes       // esto me pasa la pass a bytes desde un string plano que saco del puntero para poder hashearla
                    hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(puntero)));
                }
                finally  // hago el try finally para asegurarme de liberar la memoria y que no quede una copia de los bytes de la pass
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(puntero);
                }
                
                // aqui reconstruyo el hash que esta en bytes a un string hexadecimal y lo devuelvo
                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2"));
                }
                return hashStringBuilder.ToString();
            }
        }

    }
}
