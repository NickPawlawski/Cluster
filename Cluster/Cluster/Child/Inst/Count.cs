using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cluster.NetworkResources;

namespace Cluster.Child.Inst
{
    class Count : Inst
    {
        //Run method for counting. It just counts.
        public override void Run(long start, long stop, long dchunk)
        {
            for(long i = start; i< stop; i++)
            Output.WriteOutput(i,i);
        }
        
        public override Dictionary<long, long> GetOut()
        {
            return Output.OutputDictionary;
        }
    }
}
