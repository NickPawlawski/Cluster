using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Cluster.Child.GUI;
using Cluster.Child.Inst;
using Cluster.NetworkResources;

namespace Cluster.Child
{
    public partial class Child : Form
    {
        //Connection object to reconnect to the parent
        private Connection _conn;
        //Gui object createing the gui
        private readonly Gui _gui;
        
        public Child()
        {
            //Igonre this line....
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            //Create the gui
            _gui = new ChildGui(this);
            //Send the message parser the gui and instruction manager
            MessageParser.SetGui(_gui);
            MessageParser.SetInstManager(new InstManager());
            //Set the actions
            SetActions();
        }

        private void SetActions()
        {
            var controls = _gui.GetControls();
            controls[(int) ChildGui.ControlEnums.RunButton].Click += RunButton_Click;
        }

        private static void RunButton_Click(object sender, EventArgs e)
        {
            MessageSender.SendMessage("0," + GetLocalIpAddress(), SoftwareConfiguration.ParentIp);
        }
        
        private void Child_Load(object sender, EventArgs e)
        {
            _conn = new Connection();
            MessageParser.SetConn(_conn);
        }

        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
