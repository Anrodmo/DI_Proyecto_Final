using CrystalDecisions.CrystalReports.Engine;
using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.Services.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DI_Proyecyo_Final
{
    /// <summary>
    /// Lógica de interacción para Emergente.xaml
    /// </summary>
    public partial class Emergente : Page
    {
        private ReportDocument reportDocument = null;
        public Emergente()
        {
            InitializeComponent();



            reportDocument = new UsuariosGeneral();
            //reportDocument.Load(@"Services//Reports//UsuariosDetalle.rpt");

            reportDocument.SetParameterValue("autor", Sesion.UsuarioActivo.NombreUsuario);
            //reportDocument.SetParameterValue("nombre_usuario", "angel");
            reporteEmergente.ViewerCore.ReportSource = reportDocument;
        }
    }
}
