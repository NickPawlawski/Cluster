using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cluster.Child.GUI;
using Cluster.Child.Inst;
using Cluster.Parent;
using Cluster.Parent.Operation;

namespace Cluster.NetworkResources
{
    static class MessageParser
    {
        private static Gui _gui;
        private static ChildManager _cm;
        private static InstManager _instManager;
        private static Connection _conn;
       
        public static void SetGui(Gui gui)
        {
            _gui = gui;
        }

        public static void SetConn(Connection conn)
        {
            _conn = conn;
        }
        public static void SetChildManager(ChildManager cm)
        {
            _cm = cm;
        }

        public static void SetInstManager(InstManager instManager)
        {
            _instManager = instManager;
        }

        public static void ParseMessage(string message)
        {
            var split = message.Split(',');

            if (SoftwareConfiguration.Form == 1)
            {
                //MessageBox.Show(split[1]);
            }

            try
            {
                switch (UInt32.Parse(split[0]))
                {
                    case 0:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            if (_cm.AddChild(IPAddress.Parse(split[1])) == 1)
                            {
                                Reporter.WriteContent("Client Connected that is already a listed child: "+ split[1],1);
                            }
                            else
                            {
                                 Reporter.WriteContent("Child Connected: "+_cm.Children[IPAddress.Parse(split[1])].IpAddress +" Id: " + _cm.Children[IPAddress.Parse(split[1])].Id, 1);
                            }
                            
                            MessageSender.SendMessage("0,"+_cm.Children[IPAddress.Parse(split[1])].Id, IPAddress.Parse(split[1]));
                            
                            //send ID to new child
                        }
                        else
                        {
                             _gui.GetControls()[(int)ChildGui.ControlEnums.IdLabel].Text = split[1];
                            Reporter.WriteContent("Connected, Id given is: "+ split[1], 1);
                            _conn.Connected();
                            //MessageBox.Show("My id is: " + split[1]);
                            //Set Child Id to split[1]
                        }
                        break;

                    case 1:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            string[] split2 = split[1].Split('-');

                            List<Control[]> childControls = _gui.GetChildControls();

                            Control[] goalControls = null;

                            for (int i = 0; i < childControls.Count; i++)
                            {
                                if (String.Compare(childControls.ElementAt(i)[0].Text, split2[1], StringComparison.Ordinal) == 0)
                                {
                                    goalControls = childControls.ElementAt(i);
                                }
                            }

                            if (goalControls != null)
                            {
                                goalControls[2].BackColor = Color.Green;
                                
                                OperationManager.AddChild(_cm.Children[IPAddress.Parse(split2[1])]);
                            }
                            else
                            {
                                MessageBox.Show(split[1],@"No Child was found for message");
                            }

                            Reporter.WriteContent(split[1],1);
                        }
                        else
                        {
                            _instManager.SetInst(Int32.Parse(split[1].Trim()));
                            if(_instManager.SendMessage == 1)
                            MessageSender.SendMessage("1,Instruction Is Set-"+GetLocalIPAddress(),SoftwareConfiguration.ParentIp);
                        }
                        break;
                    case 2:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            //Reporter.WriteContent(split[1],0);
                            string[] split2 = split[1].Trim().Split('-');
                            Parent.Child chld = OperationManager.ChildList[Int32.Parse(split2[0])];
                            //Reporter.WriteContent("Child completed task: Child:"+ split[1], 1);
                            OperationManager.ContinueOperation(chld);

                            
                        }
                        else
                        {
                            string[] startSplit = split[1].Split('-');
                            long start = long.Parse(startSplit[0]);
                            long stop = long.Parse(startSplit[1]);
                            long dchunk = long.Parse(startSplit[2]);
                            _instManager.StartInst(start,stop,dchunk);

                            MessageSender.SendMessage("2,"+_gui.GetControls()[(int)ChildGui.ControlEnums.IdLabel].Text + @"- Start: "+start+@" Stop: "+(start+stop-1),SoftwareConfiguration.ParentIp);
                            //Reporter.WriteContent("Completed task: "+start+" "+stop,1);
                        }
                        break;
                    case 3:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            string[] split2 = split[1].Split('-');

                            long index =  long.Parse(split2[0]);
                            long value = long.Parse(split2[1]);
                            Reporter.WriteContent("Recieved Output for index: "+ index+ " value: "+value,1);
                            OperationManager.SetOutputs(index,value);
                        }
                        else
                        {
                            long index = long.Parse(split[1]);
                            //_gui.GetControls()[(int) ChildGui.ControlEnums.IdLabel].Text;
                            Dictionary<long, long> dict =  _instManager.GetDictionary();
                            Reporter.WriteContent("Sending back " + dict[index], 1);
                            MessageSender.SendMessage("3,"+index+"-"+dict[index],SoftwareConfiguration.ParentIp);
                        }
                        break;
                    case 50:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            MessageBox.Show(split[1]);
                        }
                        else
                        {
                            //MessageBox.Show("About to Update");
                            MessageSender.SendMessage("50,Got Message",SoftwareConfiguration.ParentIp);
                            ClickButton(0); 

                        }
                        break;
                    case 51:
                        break;
                    case 52:
                        if (SoftwareConfiguration.Form == 0)
                        {
                            
                        }
                        else
                        {
                            Reporter.WriteContent("Connection To Parent Lost",1);
                           _conn.KillConnection();
                        }
                        break;
                    case 53:
                        if (SoftwareConfiguration.Form == 0)
                        {

                        }
                        else
                        {
                            try
                            {
                                Dictionary<long, long> dict = _instManager.GetDictionary();
                                Reporter.WriteContent("Start report", 1);
                                for (int i = 0; i < dict.Count; i++)
                                {
                                    Reporter.WriteContent("Dict value " + i + " " + dict[i], 1);
                                }
                            }
                            catch (Exception e)
                            {
                                Reporter.WriteContent("Error: "+e,1);
                            }
                            
  
                        }
                        break;
                        
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Exception Thrown: "+ e);
                throw;
            }
              _gui.SetLog();  
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

        private static void ClickButton(int e)
        {
            Control[] controls = _gui.GetControls();
            Button b = (Button)controls[e];
            b.PerformClick();
        }
    }
}
