using System;
using System.IO;
using System.Windows.Forms;

namespace Cluster
{
    //Class for Creating and reading the configuration file
    internal class ConfigReader
    {
        //Filename for the config file
        private const string Filename = "Cluster.cfg";
        //Flag for attemting to read the file
        private bool _attempt;
        //Variable for the Config Reader
        private static ConfigReader _configReader;

        //Accessor for the Config Reader
        public static ConfigReader GetConfigReader()
        {
            return _configReader ?? (_configReader = new ConfigReader(false));
        }

        //Method for reading the config file
        private ConfigReader(bool forceWrite)
        {
            //Get the folder path
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //Append cluster to it
            folder += "\\Cluster";
            //If the folder doesnt exist or is told to force the write
            if (!Directory.Exists(folder) || forceWrite)
            {
                //Create the Folder
                Directory.CreateDirectory(folder);
                //Create the config file
                CreateConfig(folder);
            }
            //Read the config file
            ReadConfig(folder);
        }

        //Method for reading the config file
        private void ReadConfig(string path)
        {
            //Catch errors
            try
            {
                //Path + filename for the location
                using (var sr = new StreamReader(path + "\\" + Filename))
                {
                    //Read each line
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Split on = character
                        var split = line.Split('=');

                        //Send to the Software Configuration to set the value
                        switch (SoftwareConfiguration.SetValue(split[0].Trim().ToLower(), split[1].Trim()))
                        {
                            case 0:
                                //It Worked!
                                break;
                            case 1:
                                MessageBox.Show(@"Key Does Not Exist");
                                break;
                            case 2:
                                MessageBox.Show(@"Value Not Accecptable: " + split[1].Trim());
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

        //Method for creating the config 
        private static void CreateConfig(string path)
        {
            try
            {
                //If you want to add default lines to the file
                //Put them in here
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
