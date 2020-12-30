using RBRPro.Api;
using System.Windows;
using System.Windows.Controls;

namespace Simhub.TelemetryForwarder
{
    /// <summary>
    /// Logica di interazione per TestAddonGui.xaml
    /// </summary>
    public partial class SimhubTelemetryForwarderGui : UserControl
    {
        IRbrPro _rbrPro = null;
        
        

        public SimhubTelemetryForwarderGui(IRbrProAddOn addon, IRbrPro interactor)
        {
            

            InitializeComponent();
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _rbrPro?.StartGame(false, false, false);
        }
    }
}
