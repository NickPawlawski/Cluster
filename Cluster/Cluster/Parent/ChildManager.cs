using System.Collections.Generic;
using System.Net;

namespace Cluster.Parent
{
    internal class ChildManager
    {
        //Child dictionary with the IPaddress as the key for easy access
        public Dictionary<IPAddress, Child> Children { get; } = new Dictionary<IPAddress, Child>();

        //Adds child to dictionary
        public int AddChild(IPAddress ipAddress)
        {
            if (Children.ContainsKey(ipAddress)) return 1;

            Children.Add(ipAddress, new Child(Children.Count, ipAddress));

            return 0;
        }
    }
}
