using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Parent
{
    class Child
    {
        private int _id;
        public int Id => _id;

        private IPAddress _ip;

        public IPAddress Ip => _ip;

        public Child(int id, IPAddress ip)
        {
            _id = id;

            _ip = ip;
        }
    }
}
