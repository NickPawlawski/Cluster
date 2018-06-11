using System;
using System.Collections.Generic;

namespace Cluster.Parent.Operation
{
    //Class for a single operation. 
    //The class is static so repeating operations is easily done
    internal static class Operation
    {
        //The number the operation is currently at
        public static long Current { get; private set; }
        //The number the operation will increase by each time
        public static long Size { get; set; }
        //The number the operation will start at
        public static long Start { get; set; }
        //The number the operation will stop at
        public static long Stop { get; set; }
        //The list of children that will be used in the operation
        public static List<Child> Children { get; set; }
        //The dictionary the outputs will be initially saved to
        public static Dictionary<long, Tuple<Child,int>> OutputDictionary { get; } = new Dictionary<long, Tuple<Child, int>>();
        //The dictionart the outputs will be saved 
        public static Dictionary<long, long> RealOutputDictionary { get; } = new Dictionary<long, long>();

        //Method to reset the op
        public static void ResetOperation()
        {
            Start = 0;
            Stop = 0;
            Children = null;
            Size = 0;
            Current = 0;

            OutputDictionary.Clear();
            RealOutputDictionary.Clear();
        }

        //Method to start the op
        public static void SetOperation(long start, long stop,List<Child> children, long size)
        {
            Start = start;
            Stop = stop;
            Children = children;
            Size = size;
            Current = start;
            
            foreach (var t in children)
            {
                t.SetRunning(true);
            }
        }

        //When the op sends another job out, the current gets increased
        public static void IncreaseCurrent(long i,Child id)
        {
            OutputDictionary.Add(Current,new Tuple<Child, int>(id,0));
            Current += i;
        }

        //Method for setting the final output after the children return values
        public static void SetOutput(long index, long value)
        {
            Reporter.WriteContent("Updating index: "+index,1);
            
            RealOutputDictionary.Add(index,value);
        }
    }
}
