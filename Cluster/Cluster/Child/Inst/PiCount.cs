using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cluster.Child.Inst
{
    class PiCount : Inst
    {
        //Run method for the Pi counter
        public override void Run(long start, long stop, long dchunk)
        {
            //Write Output
            Output.ClearList();
            Reporter.WriteContent("start: "+start+ " stop: "+stop+" dchunk: "+dchunk,1);
            //List of threads used to split the job into many parts
            List<Thread> tList = new List<Thread>();
            //How many times the random numbers will be generated and calculated
            long chunksize = dchunk;

            //Initial amount of threads to be made
            long threadCount = 1;

            //Inital check for the mod amount
            long mod = stop % chunksize;

            //While loop to set the chunksize to be an equal amount for the total
            //amount of threads. It will finish with the chunksize either being 1
            //or a number divisible by the initial value
            while (mod != 0)
            { 
                chunksize = mod;
                mod = stop % mod;
            }

            //Set the thread count to be equal to the total number divided by the thread size
            threadCount = stop / chunksize;
            
            //Create the threads, add them to the list and start them
            for (var z = 0; z < threadCount; z++)
            {
                var z1 = chunksize;
                var t = new Thread(() => PiTotaler(z1));

                tList.Add(t);
                t.Start();
            }

            //Join the threads
            foreach (var t in tList)
            {
                t.Join();
            }
            
            //Sum the total number and save it
            var total = Output.AdditionList.Sum();
            Reporter.WriteContent("Start: "+start+ " Total: "+total,1);
            Output.WriteOutput(start,total);
        }

        //Pi calculator algorithm, stop is the amount of times it will run
        private static void PiTotaler(long stop)
        {
            //Create random object
            Random r = new Random();
            
            long total = 0;

            for (var i = 0; i < stop; i++)
            {
                var x = r.NextDouble();
                var y = r.NextDouble();

                x = ((x) * 2.0) - 1;
                y = ((y) * 2.0) - 1;

                var final = x * x + y * y;

                if (final <= 1)
                {
                    total++;
                }
            }

            Output.AddToList(total);
        }

        //Returns the dictionary containing the outputs of the algorithm
        public override Dictionary<long, long> GetOut()
        {
            return Output.OutputDictionary;
        }
    }
}
