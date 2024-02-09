using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    internal class NotificadorToggleButtons : INotifyPropertyChanged
    {
        private bool isOption1Checked;
        public bool IsOption1Checked
        {
            get { return isOption1Checked; }
            set
            {
                if (isOption1Checked != value)
                {
                    isOption1Checked = value;
                    OnPropertyChanged(nameof(IsOption1Checked));
                    // Asegúrate de desmarcar las otras opciones
                    if (value)
                    {
                        IsOption2Checked = false;
                        IsOption3Checked = false;
                        IsOption4Checked = false;
                        IsOption5Checked = false;
                    }
                }
            }
        }

        private bool isOption2Checked;
        public bool IsOption2Checked
        {
            get { return isOption2Checked; }
            set
            {
                if (isOption2Checked != value)
                {
                    isOption2Checked = value;
                    OnPropertyChanged(nameof(IsOption2Checked));
                    // Asegúrate de desmarcar las otras opciones
                    if (value)
                    {
                        IsOption1Checked = false;
                        IsOption3Checked = false;
                        IsOption4Checked = false;
                        IsOption5Checked = false;
                    }
                }
            }
        }

        private bool isOption3Checked;
        public bool IsOption3Checked
        {
            get { return isOption3Checked; }
            set
            {
                if (isOption3Checked != value)
                {
                    isOption3Checked = value;
                    OnPropertyChanged(nameof(IsOption3Checked));
                    // Asegúrate de desmarcar las otras opciones
                    if (value)
                    {
                        IsOption1Checked = false;
                        IsOption2Checked = false;
                        IsOption4Checked = false;
                        IsOption5Checked = false;
                    }
                }
            }
        }

        private bool isOption4Checked;
        public bool IsOption4Checked
        {
            get { return isOption4Checked; }
            set
            {
                if (isOption4Checked != value)
                {
                    isOption4Checked = value;
                    OnPropertyChanged(nameof(IsOption4Checked));
                    // Asegúrate de desmarcar las otras opciones
                    if (value)
                    {
                        IsOption1Checked = false;
                        IsOption2Checked = false;
                        IsOption3Checked = false;
                        IsOption5Checked = false;
                    }
                }
            }
        }

        private bool isOption5Checked;
        public bool IsOption5Checked
        {
            get { return isOption5Checked; }
            set
            {
                if (isOption5Checked != value)
                {
                    isOption5Checked = value;
                    OnPropertyChanged(nameof(IsOption5Checked));
                    // Asegúrate de desmarcar las otras opciones
                    if (value)
                    {
                        IsOption1Checked = false;
                        IsOption2Checked = false;
                        IsOption3Checked = false;
                        IsOption4Checked = false;
                    }
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
