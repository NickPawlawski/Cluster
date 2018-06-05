using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cluster
{
    class ConfigReader
    {
        private const string Filename = "Cluster.cfg";
        private bool _attempt;
        private static ConfigReader configReader;

        public static ConfigReader GetConfigReader()
        {
            return configReader ?? (configReader = new ConfigReader(false));
        }

        private ConfigReader(bool forceWrite)
        {

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            folder += "\\Cluster";

            if (!Directory.Exists(folder) || forceWrite)
            {
                Directory.CreateDirectory(folder);
                CreateConfig(folder);
            }

            ReadConfig(folder);
        }

        private void ReadConfig(string path)
        {
            try
            {
                using (var sr = new StreamReader(path + "\\" + Filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var split = line.Split('=');

                        switch (SoftwareConfiguration.SetValue(split[0].Trim().ToLower(), split[1].Trim()))
                        {
                            case 0:
                                //It Worked!
                                break;
                            case 1:
                                MessageBox.Show("Key Does Not Exist");
                                break;
                            case 2:
                                MessageBox.Show("Value Not Accecptable: " + split[1].Trim());
                                break;
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                //Try to re create the conf file and read it. if it fails again show message
                CreateConfig(path);

                if (!_attempt)
                {
                    _attempt = true;
                    ReadConfig(path);
                }
                else
                {
                    MessageBox.Show(@"There was an error creating or reading the configuration file: " + e,
                        @"Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Unknown Exception has been thrown: " + ex, @"Configuration Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CreateConfig(string path)
        {
            try
            {
                using (var sw = new StreamWriter(path + "\\" + Filename))
                {
                    sw.WriteLine("form=1");
                    
                }
            }
            catch (FileNotFoundException fnf)
            {
                MessageBox.Show(@"FileNotFound: " + Filename + @" " + fnf);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex);
            }
        }
    }
}
