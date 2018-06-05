using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cluster.Parent;

namespace Cluster.NetworkResources
{
    static class MessageParser
    {
        private static Gui _gui;
        private static ChildManager _cm;
        public static void SetGui(Gui gui)
        {
            _gui = gui;
        }

        public static void SetChildManager(ChildManager cm)
        {
            _cm = cm;
        }

        public static void ParseMessage(string message)
        {
            var split = message.Split(',');
            try
            {
                switch (UInt32.Parse(split[0]))
                {
                    case 0:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            _cm.AddChild(IPAddress.Parse(split[1]));

                            Control[] controls = _gui.GetControls();
                            Button b = (Button)controls[0];
                            b.PerformClick();
                        }
                        else
                        {
                            
                        }
                        break;

                    case 1:
                        if (SoftwareConfiguration.Form == 0)
                        {

                        }
                        else
                        {

                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Exception Thrown: "+ e);
                throw;
            }
                
        }
    }
}
