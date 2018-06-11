using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Child.Inst
{
    static class Output
    {
        //Dictionaty for storing the outputs
        public static Dictionary<long, long> OutputDictionary { get; } = new Dictionary<long, long>();
        //List for storing the threaded method calculations
        public static List<long> AdditionList { get; } = new List<long>();
        //Method for writing the output
        public static void WriteOutput(long index, long output)
        {
            try
            {
                OutputDictionary.Add(index,output);
            }
            catch (Exception e)
            {
                Reporter.WriteContent("ERROR IN WRITEOUTPUT",1);
            }
            
        }
        //Method for clearing the list
        public static void ClearList()
        {
            AdditionList.Clear();
        }

        //Method for adding the list
        public static void AddToList(long i)
        {
            AdditionList.Add(i);
        }

        //Method for clearing the dictionary
        public static void ClearOutput()
        {
            OutputDictionary.Clear();
        }
    }
}
