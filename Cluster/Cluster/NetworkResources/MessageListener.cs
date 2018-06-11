using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cluster.NetworkResources
{
    internal class MessageListener
    {
        private byte[] in_buffer = new byte[32];

        private bool killThread = false;

        TcpListener listener;
        Thread listenThread;

        public MessageListener()
        {
            TcpListener();
        }

        public void KillThread()
        {
            

            MessageSender.SendMessageToSelf();
            
            if (listenThread.Join(10000))
            {
                //MessageBox.Show("join suc");
            }
            else
            {
                //MessageBox.Show("join fail");
            }

        }

        private void TcpListener()
        {
            int port = 12457;

            listener = new TcpListener(IPAddress.Any, port);
            listenThread = new Thread(Service);
            listenThread.Start();
        }

        private void Service()
        {
            try
            {
                listener.Start();
            }
            catch (Exception e)
            {
                
            }
            
            
            while (!killThread)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream clientStream = client.GetStream();
                    StreamReader sr = new StreamReader(clientStream);
                    string split = sr.ReadToEnd();
                    MessageParser.ParseMessage(split);
                    
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }

                killThread = ThreadKill.ThreadKillFlag;
            }
        }
    }
}
