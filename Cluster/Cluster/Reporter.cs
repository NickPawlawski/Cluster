using System;
using System.Collections.Generic;
using System.IO;

namespace Cluster
{
    //Reporting Class
    public static class Reporter
    {
        //Stack for managing the sections
        private static readonly Stack<string> Section = new Stack<string>();

        //Filename for the report
        private const string Filename = "Report.txt";
        //Folder path
        private static readonly string Folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Cluster";

        //Method to init the reporter
        public static void Startup()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }

            try
            {
                using (var streamWriter = File.AppendText(Folder + "\\" + Filename))
                {
                    streamWriter.WriteLine("*****************************************");
                    streamWriter.WriteLine("Software Launched at: " + DateTime.Now);
                    streamWriter.WriteLine(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }
        //List to manage the log 
        public static List<string> FrontEndReport { set; get; } = new List<string>();
        //List of lists that manage the log
        public static List<List<string>> FrontEndReports { get; } = new List<List<string>>();
        //Method to start a new section
        public static void StartSection(string sectionName)
        {
            Section.Push(sectionName);

            FrontEndReport = new List<string> {DateTime.Now + " Started Section: " + sectionName};

            try
            {
                using (var streamWriter = File.AppendText(Folder + "\\" + Filename))
                {
                    //Console.WriteLine(@"Error in reporter" + sectionName);
                    streamWriter.WriteLine("------------- Start: " + sectionName + " -------------");
                    streamWriter.WriteLine(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }
        //Method to end the most current section
        public static void EndSection()
        {
            FrontEndReport.Add("-----------------------");
            FrontEndReports.Add(FrontEndReport);
            var sectionName = Section.Pop();

            try
            {
                using (var streamWriter = File.AppendText(Folder + "\\" + Filename))
                {
                    //Console.WriteLine(@"Error in reporter" + sectionName);
                    streamWriter.WriteLine("------------- End: " + sectionName + " -------------");
                    streamWriter.WriteLine(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }
        //Method to write a message to the log or the gui log
        public static void WriteContent(string content, int location)
        {
            try
            {
                switch (location)
                {
                    case 0:
                        using (var streamWriter = File.AppendText(Folder + "\\" + Filename))
                        {
                            streamWriter.WriteLine(DateTime.Now + "    " + content);
                        }
                        break;

                    case 1:
                        FrontEndReport.Add("     " + content);
                        
                        break;

                    case 2:
                        using (var streamWriter = File.AppendText(Folder + "\\" + Filename))
                        {
                            streamWriter.WriteLine(DateTime.Now + "    " + content);
                            if (SoftwareConfiguration.Form != 2)
                                FrontEndReport.Add("     " + content);
                        }
                        break;

                    case 3:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }
        //Clears the front end report
        public static void ClearList()
        {
            FrontEndReport?.Clear();
        }
    } 
}
