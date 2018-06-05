using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cluster.NetworkResources;

namespace Cluster.Child
{
    public partial class Child : Form
    {
        MessageSender _messageSender = new MessageSender();
        MessageListener _messageListener = new MessageListener();

        //child
        public Child()
        {
            InitializeComponent();
        }

        private void Child_Load(object sender, EventArgs e)
        {
            _messageSender.SendMessage("0,"+GetLocalIPAddress());
        }

        private void INIT_Click(object sender, EventArgs e)
        {

            

        }

        private void Update_Click(object sender, EventArgs e)
        {
            var ud = new Update.Update();

        }

        private void Cluster_Close(object sender)
        {
            
        }

        private static string GetLocalIPAddress()
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
