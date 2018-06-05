using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cluster.Child;

namespace Cluster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ConfigReader.GetConfigReader();
            //check
            Update.Update ud = new Update.Update();

            switch (SoftwareConfiguration.Form)
            {
                case 0:
                    Application.Run(new Parent.Parent());
                    break;
                case 1:
                    Application.Run(new Child.Child());
                    break;
                
                default:
                    Application.Run(new Child.Child());
                    break;

            }


           
            
        }
    }
}
