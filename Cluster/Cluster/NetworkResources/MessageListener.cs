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

        public MessageListener( )
        {
            CICOListener();
        }

        public void KillThread()
        {
            MessageSender ms = new MessageSender();

            ms.SendMessageToSelf();
            
            //listenThread.Abort();
            if (listenThread.Join(10000))
            {
                //MessageBox.Show("join suc");
            }
            else
            {
                //MessageBox.Show("join fail");
                //listenThread.Abort();
            }

        }

        private void CICOListener( )
        {
            int port = 12457;

            listener = new TcpListener(IPAddress.Any, port);
            listenThread = new Thread(new ThreadStart(Service));
            listenThread.Start();
        }

        private void Service()
        {
            listener.Start();
            
            while (!killThread)
            {
                
               
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream clientStream = client.GetStream();
                    StreamReader sr = new StreamReader(clientStream);
                    string split = sr.ReadToEnd();
                    MessageParser.ParseMessage(split);
                    //MessageBox.Show("Message recieved: " + split[0]);
                    //listenManager(split);
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }

                killThread = ThreadKill.ThreadKillFlag;

                Console.WriteLine("In thread: " + killThread);
            }
            //MessageBox.Show("Thread is going to die");
        }

        private void listenManager(string[] message)
        {

        }
    }
}
