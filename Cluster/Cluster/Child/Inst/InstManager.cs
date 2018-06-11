using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Child.Inst
{
    class InstManager
    {
        //Instruction variable 
        private Inst _inst;
        //Flag for if it sends a return message. 0 = no, 1 = yes
        public int SendMessage = 0;
        
        //Method for setting what instruction object to use
        public void SetInst(int index)
        {
            switch (index)
            {
                case 0:// 0 is reset
                    _inst = null;
                    Output.ClearList();
                    Output.ClearOutput();
                    SendMessage = 0;
                    Reporter.WriteContent("Instruction Reset",1);
                    break;
                case 1:// 1 is the pi count
                    _inst = new PiCount();
                    SendMessage = 1;
                    Reporter.WriteContent("Pi Count Loaded",1);
                    break;
                case 2:// 2 is count
                    _inst = new Count();
                    SendMessage = 1;
                    Reporter.WriteContent("Other Count Loaded", 1);
                    break;
            }
        }

        //Method for starting the run method
        public void StartInst(long start, long stop, long dChunk)
        {
            _inst.Run(start,stop,dChunk);
        }
        //Method for returning the dictionary
        public Dictionary<long, long> GetDictionary()
        {
            return _inst.GetOut();
        }

        public void SetOutput()
        {
            
        }
    }
}
