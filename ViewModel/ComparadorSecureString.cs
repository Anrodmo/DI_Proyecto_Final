using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    internal static class ComparadorSecureString
    {

        public static bool AreSecureStringsEqual(SecureString secureString1, SecureString secureString2)
        {
            bool iguales = false;

            if (secureString1 != null && secureString2 != null)
            {
                IntPtr bstr1 = IntPtr.Zero;
                IntPtr bstr2 = IntPtr.Zero;

                try
                {
                    bstr1 = Marshal.SecureStringToBSTR(secureString1);
                    bstr2 = Marshal.SecureStringToBSTR(secureString2);

                    // Convierte los BSTRs en arreglos de bytes y compara los bytes
                    byte[] bytes1 = ConvertBSTRToByteArray(bstr1);
                    byte[] bytes2 = ConvertBSTRToByteArray(bstr2);

                    iguales = ByteArrayEquals(bytes1, bytes2);
                }
                finally
                {
                    if (bstr1 != IntPtr.Zero)
                        Marshal.ZeroFreeBSTR(bstr1);

                    if (bstr2 != IntPtr.Zero)
                        Marshal.ZeroFreeBSTR(bstr2);
                }

            }
               
            return iguales;
            
        }

        private static byte[] ConvertBSTRToByteArray(IntPtr bstr)
        {
            int length = Marshal.ReadInt32(bstr, -4);
            byte[] bytes = new byte[length];
            Marshal.Copy(bstr, bytes, 0, length);
            return bytes;
        }

        private static bool ByteArrayEquals(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }
    }
}
