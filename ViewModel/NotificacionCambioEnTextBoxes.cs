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
        private string nombreUsuario;
        private string contraseña1;
        private string contraseña2;
        private string nombreUsuarioGestion;
        private string nombrePropietarioGestion;
        private string nifPropietarioGestion;
        private string apellidosPropietarioGestion;
        private string emailPropietarioGestion;
        private string telefonoPropietarioGestion;
        private string callePropietarioGestion;
        private string bloquePropietarioGestion;
        private string pisoPropietarioGestion;
        private string codigoPostalPropietarioGestion;
        private string localidadPropietarioGestion;
        private string provinciaPropietarioGestion;

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




        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
