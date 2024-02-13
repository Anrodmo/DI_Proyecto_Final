using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    internal class NotificacionCambioEnTextBoxes : INotifyPropertyChanged
    {

        /*===============================================================*/
        /*                Gestion de usuarios                            */
        /*===============================================================*/
        private string nombreUsuario;
        private string contraseña1;
        private string contraseña2;
        private string nombreUsuarioGestion;      
      
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set
            {
                if (nombreUsuario != value)
                {
                    nombreUsuario = value;
                    OnPropertyChanged(nameof(NombreUsuario));
                }
            }
        }
        public string NombreUsuarioGestion
        {
            get { return nombreUsuarioGestion; }
            set
            {
                if (nombreUsuarioGestion != value)
                {
                    nombreUsuarioGestion = value;
                    OnPropertyChanged(nameof(NombreUsuarioGestion));
                }
            }
        }
        public string Contraseña1
        {
            get { return contraseña1; }
            set
            {
                if (contraseña1 != value)
                {
                    contraseña1 = value;
                    OnPropertyChanged(nameof(Contraseña1));
                }
            }
        }
        public string Contraseña2
        {
            get { return contraseña2; }
            set
            {
                if (contraseña2 != value)
                {
                    contraseña2 = value;
                    OnPropertyChanged(nameof(Contraseña2));
                }
            }
        }
        
        /*===============================================================*/
        /*                Gestion de propietarios                        */
        /*===============================================================*/
        private string nifPropietarioGestion;
        private string nombrePropietarioGestion;
        private string apellidosPropietarioGestion;
        private string emailPropietarioGestion;
        private string telefonoPropietarioGestion;
        private string callePropietarioGestion;
        private string bloquePropietarioGestion;
        private string pisoPropietarioGestion;
        private string codigoPostalPropietarioGestion;
        private string localidadPropietarioGestion;
        private string provinciaPropietarioGestion;
        
        public string NombrePropietarioGestion
        {
            get { return nombrePropietarioGestion; }
            set
            {
                if (nombrePropietarioGestion != value)
                {
                    nombrePropietarioGestion = value;
                    OnPropertyChanged(nameof(NombrePropietarioGestion));
                }
            }
        }
        public string NifPropietarioGestion
        {
            get { return nifPropietarioGestion; }
            set
            {
                if (nifPropietarioGestion != value)
                {
                    nifPropietarioGestion = value;
                    OnPropertyChanged(nameof(NifPropietarioGestion));
                }
            }
        }
        public string ApellidosPropietarioGestion
        {
            get { return apellidosPropietarioGestion; }
            set
            {
                if (apellidosPropietarioGestion != value)
                {
                    apellidosPropietarioGestion = value;
                    OnPropertyChanged(nameof(ApellidosPropietarioGestion));
                }
            }
        }
        public string EmailPropietarioGestion
        {
            get { return emailPropietarioGestion; }
            set
            {
                if (emailPropietarioGestion != value)
                {
                    emailPropietarioGestion = value;
                    OnPropertyChanged(nameof(EmailPropietarioGestion));
                }
            }
        }
        public string TelefonoPropietarioGestion
        {
            get { return telefonoPropietarioGestion; }
            set
            {
                if (telefonoPropietarioGestion != value)
                {
                    telefonoPropietarioGestion = value;
                    OnPropertyChanged(nameof(TelefonoPropietarioGestion));
                }
            }
        }
        public string CallePropietarioGestion
        {
            get { return callePropietarioGestion; }
            set
            {
                if (callePropietarioGestion != value)
                {
                    callePropietarioGestion = value;
                    OnPropertyChanged(nameof(CallePropietarioGestion));
                }
            }
        }
        public string BloquePropietarioGestion
        {
            get { return bloquePropietarioGestion; }
            set
            {
                if (bloquePropietarioGestion != value)
                {
                    bloquePropietarioGestion = value;
                    OnPropertyChanged(nameof(BloquePropietarioGestion));
                }
            }
        }
        public string PisoPropietarioGestion
        {
            get { return pisoPropietarioGestion; }
            set
            {
                if (pisoPropietarioGestion != value)
                {
                    pisoPropietarioGestion = value;
                    OnPropertyChanged(nameof(PisoPropietarioGestion));
                }
            }
        }
        public string CodigoPostalPropietarioGestion
        {
            get { return codigoPostalPropietarioGestion; }
            set
            {
                if (codigoPostalPropietarioGestion != value)
                {
                    codigoPostalPropietarioGestion = value;
                    OnPropertyChanged(nameof(CodigoPostalPropietarioGestion));
                }
            }
        }
        public string LocalidadPropietarioGestion
        {
            get { return localidadPropietarioGestion; }
            set
            {
                if (localidadPropietarioGestion != value)
                {
                    localidadPropietarioGestion = value;
                    OnPropertyChanged(nameof(LocalidadPropietarioGestion));
                }
            }
        }
        public string ProvinciaPropietarioGestion
        {
            get { return provinciaPropietarioGestion; }
            set
            {
                if (provinciaPropietarioGestion != value)
                {
                    provinciaPropietarioGestion = value;
                    OnPropertyChanged(nameof(ProvinciaPropietarioGestion));
                }
            }
        }

        /*===============================================================*/
        /*               Creación de propietarios                        */
        /*===============================================================*/

        private string nifPropietarioCreacion;
        private string nombrePropietarioCreacion;
        private string apellidosPropietarioCreacion;
        private string emailPropietarioCreacion;
        private string telefonoPropietarioCreacion;
        private string callePropietarioCreacion;
        private string bloquePropietarioCreacion;
        private string pisoPropietarioCreacion;
        private string codigoPostalPropietarioCreacion;
        private string localidadPropietarioCreacion;
        private string provinciaPropietarioCreacion;

        public string NifPropietarioCreacion
        {
            get { return nifPropietarioCreacion; }
            set
            {
                if (nifPropietarioCreacion != value)
                {
                    nifPropietarioCreacion = value;
                    OnPropertyChanged(nameof(NifPropietarioCreacion));
                }
            }
        }
        public string NombrePropietarioCreacion
        {
            get { return nombrePropietarioCreacion; }
            set
            {
                if (nombrePropietarioCreacion != value)
                {
                    nombrePropietarioCreacion = value;
                    OnPropertyChanged(nameof(NombrePropietarioCreacion));
                }
            }
        }
        public string ApellidosPropietarioCreacion
        {
            get { return apellidosPropietarioCreacion; }
            set
            {
                if (apellidosPropietarioCreacion != value)
                {
                    apellidosPropietarioCreacion = value;
                    OnPropertyChanged(nameof(ApellidosPropietarioCreacion));
                }
            }
        }
        public string EmailPropietarioCreacion
        {
            get { return emailPropietarioCreacion; }
            set
            {
                if (emailPropietarioCreacion != value)
                {
                    emailPropietarioCreacion = value;
                    OnPropertyChanged(nameof(EmailPropietarioCreacion));
                }
            }
        }
        public string TelefonoPropietarioCreacion
        {
            get { return telefonoPropietarioCreacion; }
            set
            {
                if (telefonoPropietarioCreacion != value)
                {
                    telefonoPropietarioCreacion = value;
                    OnPropertyChanged(nameof(TelefonoPropietarioCreacion));
                }
            }
        }
        public string CallePropietarioCreacion
        {
            get { return callePropietarioCreacion; }
            set
            {
                if (callePropietarioCreacion != value)
                {
                    callePropietarioCreacion = value;
                    OnPropertyChanged(nameof(CallePropietarioCreacion));
                }
            }
        }
        public string BloquePropietarioCreacion
        {
            get { return bloquePropietarioCreacion; }
            set
            {
                if (bloquePropietarioCreacion != value)
                {
                    bloquePropietarioCreacion = value;
                    OnPropertyChanged(nameof(BloquePropietarioCreacion));
                }
            }
        }
        public string PisoPropietarioCreacion
        {
            get { return pisoPropietarioCreacion; }
            set
            {
                if (pisoPropietarioCreacion != value)
                {
                    pisoPropietarioCreacion = value;
                    OnPropertyChanged(nameof(PisoPropietarioCreacion));
                }
            }
        }
        public string CodigoPostalPropietarioCreacion
        {
            get { return codigoPostalPropietarioCreacion; }
            set
            {
                if (codigoPostalPropietarioCreacion != value)
                {
                    codigoPostalPropietarioCreacion = value;
                    OnPropertyChanged(nameof(CodigoPostalPropietarioCreacion));
                }
            }
        }
        public string LocalidadPropietarioCreacion
        {
            get { return localidadPropietarioCreacion; }
            set
            {
                if (localidadPropietarioCreacion != value)
                {
                    localidadPropietarioCreacion = value;
                    OnPropertyChanged(nameof(LocalidadPropietarioCreacion));
                }
            }
        }
        public string ProvinciaPropietarioCreacion
        {
            get { return provinciaPropietarioCreacion; }
            set
            {
                if (provinciaPropietarioCreacion != value)
                {
                    provinciaPropietarioCreacion = value;
                    OnPropertyChanged(nameof(ProvinciaPropietarioCreacion));
                }
            }
        }

        /*===============================================================*/
        /*                Gestion de propiedades                         */
        /*===============================================================*/

        private string nifPropiedadGestion;
        private string descripcionPropiedadGestion;
        private string tamañoPropiedadGestion;
        private string callePropiedadGestion;
        private string bloquePropiedadGestion;
        private string pisoPropiedadGestion;
        private string codigoPostalPropiedadGestion;
        private string localidadPropiedadGestion;
        private string provinciaPropiedadGestion;

        public string NifPropiedadGestion
        {
            get { return nifPropiedadGestion; }
            set
            {
                if (nifPropiedadGestion != value)
                {
                    nifPropiedadGestion = value;
                    OnPropertyChanged(nameof(NifPropiedadGestion));
                }
            }
        }
        public string DescripcionPropiedadGestion
        {
            get { return descripcionPropiedadGestion; }
            set
            {
                if (descripcionPropiedadGestion != value)
                {
                    descripcionPropiedadGestion = value;
                    OnPropertyChanged(nameof(DescripcionPropiedadGestion));
                }
            }
        }
        public string TamañoPropiedadGestion
        {
            get { return tamañoPropiedadGestion; }
            set
            {
                if (tamañoPropiedadGestion != value)
                {
                    tamañoPropiedadGestion = value;
                    OnPropertyChanged(nameof(TamañoPropiedadGestion));
                }
            }
        }
        public string CallePropiedadGestion
        {
            get { return callePropiedadGestion; }
            set
            {
                if (callePropiedadGestion != value)
                {
                    callePropiedadGestion = value;
                    OnPropertyChanged(nameof(CallePropiedadGestion));
                }
            }
        }
        public string BloquePropiedadGestion
        {
            get { return bloquePropiedadGestion; }
            set
            {
                if (bloquePropiedadGestion != value)
                {
                    bloquePropiedadGestion = value;
                    OnPropertyChanged(nameof(BloquePropiedadGestion));
                }
            }
        }
        public string PisoPropiedadGestion
        {
            get { return pisoPropiedadGestion; }
            set
            {
                if (pisoPropiedadGestion != value)
                {
                    pisoPropiedadGestion = value;
                    OnPropertyChanged(nameof(PisoPropiedadGestion));
                }
            }
        }
        public string CodigoPostalPropiedadGestion
        {
            get { return codigoPostalPropiedadGestion; }
            set
            {
                if (codigoPostalPropiedadGestion != value)
                {
                    codigoPostalPropiedadGestion = value;
                    OnPropertyChanged(nameof(CodigoPostalPropiedadGestion));
                }
            }
        }
        public string LocalidadPropiedadGestion
        {
            get { return localidadPropiedadGestion; }
            set
            {
                if (localidadPropiedadGestion != value)
                {
                    localidadPropiedadGestion = value;
                    OnPropertyChanged(nameof(LocalidadPropiedadGestion));
                }
            }
        }
        public string ProvinciaPropiedadGestion
        {
            get { return provinciaPropiedadGestion; }
            set
            {
                if (provinciaPropiedadGestion != value)
                {
                    provinciaPropiedadGestion = value;
                    OnPropertyChanged(nameof(ProvinciaPropiedadGestion));
                }
            }
        }

        /*===============================================================*/
        /*                Creación de propiedades                        */
        /*===============================================================*/

        private string nifPropiedadCreacion;
        private string descripcionPropiedadCreacion;
        private string tamañoPropiedadCreacion;
        private string callePropiedadCreacion;
        private string bloquePropiedadCreacion;
        private string pisoPropiedadCreacion;
        private string codigoPostalPropiedadCreacion;
        private string localidadPropiedadCreacion;
        private string provinciaPropiedadCreacion;

        public string NifPropiedadCreacion
        {
            get { return nifPropiedadCreacion; }
            set
            {
                if (nifPropiedadCreacion != value)
                {
                    nifPropiedadCreacion = value;
                    OnPropertyChanged(nameof(NifPropiedadCreacion));
                }
            }
        }
        public string DescripcionPropiedadCreacion
        {
            get { return descripcionPropiedadCreacion; }
            set
            {
                if (descripcionPropiedadCreacion != value)
                {
                    descripcionPropiedadCreacion = value;
                    OnPropertyChanged(nameof(DescripcionPropiedadCreacion));
                }
            }
        }
        public string TamañoPropiedadCreacion
        {
            get { return tamañoPropiedadCreacion; }
            set
            {
                if (tamañoPropiedadCreacion != value)
                {
                    tamañoPropiedadCreacion = value;
                    OnPropertyChanged(nameof(TamañoPropiedadCreacion));
                }
            }
        }
        public string CallePropiedadCreacion
        {
            get { return callePropiedadCreacion; }
            set
            {
                if (callePropiedadCreacion != value)
                {
                    callePropiedadCreacion = value;
                    OnPropertyChanged(nameof(CallePropiedadCreacion));
                }
            }
        }
        public string BloquePropiedadCreacion
        {
            get { return bloquePropiedadCreacion; }
            set
            {
                if (bloquePropiedadCreacion != value)
                {
                    bloquePropiedadCreacion = value;
                    OnPropertyChanged(nameof(BloquePropiedadCreacion));
                }
            }
        }
        public string PisoPropiedadCreacion
        {
            get { return pisoPropiedadCreacion; }
            set
            {
                if (pisoPropiedadCreacion != value)
                {
                    pisoPropiedadCreacion = value;
                    OnPropertyChanged(nameof(PisoPropiedadCreacion));
                }
            }
        }
        public string CodigoPostalPropiedadCreacion
        {
            get { return codigoPostalPropiedadCreacion; }
            set
            {
                if (codigoPostalPropiedadCreacion != value)
                {
                    codigoPostalPropiedadCreacion = value;
                    OnPropertyChanged(nameof(CodigoPostalPropiedadCreacion));
                }
            }

        }
        public string LocalidadPropiedadCreacion
        {
            get { return localidadPropiedadCreacion; }
            set
            {
                if (localidadPropiedadCreacion != value)
                {
                    localidadPropiedadCreacion = value;
                    OnPropertyChanged(nameof(LocalidadPropiedadCreacion));
                }
            }
        }
        public string ProvinciaPropiedadCreacion
        {
            get { return provinciaPropiedadCreacion; }
            set
            {
                if (provinciaPropiedadCreacion != value)
                {
                    provinciaPropiedadCreacion = value;
                    OnPropertyChanged(nameof(ProvinciaPropiedadCreacion));
                }
            }
        }






        /*                      EVENTO                                  */

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
