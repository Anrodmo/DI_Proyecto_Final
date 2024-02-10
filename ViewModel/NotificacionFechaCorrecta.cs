using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    /// <summary>
    /// Clase que notifica el cambio del contenido en tiempo de ejecucion en el datapicker bindeado
    /// </summary>
    internal class NotificacionFechaCorrecta : INotifyPropertyChanged
    {
        private DateTime? fechaValidaPropietariosGestion;
        private DateTime? fechaValidaPropietariosCreacion;

        public NotificacionFechaCorrecta()
        {
            // Inicializar la fecha seleccionada como null
            fechaValidaPropietariosGestion = null;
            fechaValidaPropietariosCreacion = null;
        }

        public DateTime? FechaValidaPropietariosGestion
        {
            get { return fechaValidaPropietariosGestion; }
            set
            {
                if (fechaValidaPropietariosGestion != value)
                {
                    fechaValidaPropietariosGestion = value;
                    OnPropertyChanged(nameof(FechaValidaPropietariosGestion));
                }
            }
        }

        public DateTime? FechaValidaPropietariosCreacion
        {
            get { return fechaValidaPropietariosCreacion; }
            set
            {
                if (fechaValidaPropietariosCreacion != value)
                {
                    fechaValidaPropietariosCreacion = value;
                    OnPropertyChanged(nameof(FechaValidaPropietariosCreacion));
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
