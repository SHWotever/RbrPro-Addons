using RBRPro.Api;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TGD.Rbr.Telemetry.Data;

namespace Simhub.TelemetryForwarder
{
    /// <summary>
    /// The TestAddon
    /// A RBRPro Addon is a class implementing the IRbrProAddOn interface.
    /// Optionally, the Addon can receive telemetry by implementing the ITelemetryClient interface
    /// </summary>
    public class SimhubTelemetryForwarder : IRbrProAddOn
    {
        // Maybe in future these properties will be replaced by class attributes
        #region ABOUT
        public string Name { get => "simhub UDP forwarder"; }
        public string Description { get => "forwards NGP telemetry data to another UDP port"; }
        public string Author { get => "Wotever"; }

        // An optional icon is provided to the manager, just to decorate the tab item a bit...
        public Image Icon => new Image { Source = new BitmapImage(new Uri($"pack://application:,,,/Simhub.TelemetryForwarder;component/icon.png", UriKind.Absolute)) };

        // This property tells the manager if the addon can be detached in a separate window or not
        public bool IsDetachable { get => false; }
        public UdpClient UdpClient { get; }

        #endregion

        // The interface used to interact with the manager
        public IRbrPro _interactor;



        public SimhubTelemetryForwarder()
        {
            try
            {
                this.UdpClient = new UdpClient("127.0.0.1", 6776);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Called by the manager just after the addon loading, the contents are not ready yet...
        /// but in the meantime you can do something to prepare...
        /// for example we can load our language strings
        /// </summary>
        public void Init(IRbrPro rbrProInteractor)
        {
            _interactor = rbrProInteractor;
            _interactor.DataReceived += _interactor_DataReceived;
        }

        private void _interactor_DataReceived(object sender, TelemetryData data)
        {
            try
            {
                byte[] byteData = ConvertPacketToByteArray(data);
                UdpClient.Send(byteData, byteData.Length);
            }
            catch
            {

            }
        }

        public static byte[] ConvertPacketToByteArray<T>(T packet)
        {
            int size = Marshal.SizeOf(packet);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(packet, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        /// <summary>
        /// Called by the manager when it finishes the initialization and the contents are available
        /// </summary>
        /// <param name="rbrProInteractor"></param>
        public void Ready(IRbrPro rbrProInteractor)
        {

        }

        /// <summary>
        /// Called by the manager, this method just returns an instance of the GUI Control
        /// </summary>
        /// <returns></returns>
        public System.Windows.Controls.Control GetGui()
        {
            return new SimhubTelemetryForwarderGui(this, _interactor);
        }

        public void Exit()
        {
            // No action performed
        }

        public void OnParentWindowChange(Window parent)
        {
            // No action performed
        }
    }
}
