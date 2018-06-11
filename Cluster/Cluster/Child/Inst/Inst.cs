using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Child.Inst
{
    abstract class Inst
    {
        //Abstract method for Run
        public abstract void Run(long start, long stop, long dchunk);
        //Abstract method for getting the output dictionary
        public abstract Dictionary<long, long> GetOut();

    }
}
