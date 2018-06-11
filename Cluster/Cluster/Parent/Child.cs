using System.Net;

namespace Cluster.Parent
{
    //Child class
    internal class Child
    {
        //ID of the child
        public int Id { get; }

        //IP of the child
        public IPAddress IpAddress { get; }

        //If the child is currently running in the operation
        public bool Running { get; private set; }

        //Constructor of the child
        public Child(int id,IPAddress ip)
        {
            Id = id;
            IpAddress = ip;
        }

        //Sets the running variable of the child
        public void SetRunning(bool running)
        {
            Running = running;
        }
    }
}
