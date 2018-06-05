using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cluster.Parent
{
    class ChildManager
    {

        public List<Child> Children => children;


        private List<Child> children = new List<Child>();
        private int total;
        public ChildManager()
        {
            
        }

        public void AddChild(IPAddress ipAddress)
        {
            total = children.Count;

            children.Add(new Child(total, ipAddress));

        }
    }
}
