using System;
using System.Windows.Forms;

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
            //Read the config
            ConfigReader.GetConfigReader();
            
            //Switch on the form setting
            switch (SoftwareConfiguration.Form)
            {
                //0 is the parent
                case 0:
                    Application.Run(new Parent.Parent());
                    break;
                //1 is the child
                case 1:
                    Application.Run(new Child.Child());
                    break;
                //do the child by default
                default:
                    Application.Run(new Child.Child());
                    break;
            }
        }
    }
}
