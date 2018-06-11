using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Cluster.NetworkResources;

namespace Cluster.Parent.Operation
{
    internal static class OperationManager
    {
        //Stopwatch for timing 
        private static readonly Stopwatch Sw = new Stopwatch();
        //List of children for the operation
        public static readonly List<Child> _childList = new List<Child>();
        //Accessor for the child list
        public static List<Child> ChildList => _childList;

        //Method to reset the op
        public static void ResetOperation()
        {
            _childList.Clear();
            Sw.Reset();
            Operation.ResetOperation();
        }
        
        //Method for adding a child to the op
        public static void AddChild(Child child)
        {
            if (!_childList.Contains(child))
            {
                _childList.Add(child);
            }
            else
            {
                Reporter.WriteContent(@"Attempted to add child that already existed",1);
            }
        }

        //Method for removing a child from the op
        public static void RemoveChild(Child child)
        {
            if (_childList.Contains(child))
            {
                _childList.Remove(child);
            }
            else
            {
                MessageBox.Show(@"Attempted to remove a child that didnt exist");
            }
        }

        //Method for starting the operation
        public static void StartOperation(long start, long stop, long size)
        {
            //Start the operation
            Reporter.StartSection("Operation Starting");
            Operation.SetOperation(start,stop,_childList,size);
            Sw.Start();

            //Send each child in the operation list a chunk of the operation
            foreach (var t in Operation.Children)
            {
                //If there is more to send
                if (Operation.Current < Operation.Stop)
                {
                    MessageSender.SendMessage("2,"+ Operation.Current+"-"+size + "-" + SoftwareConfiguration.DChunk, t.IpAddress);
                    Operation.IncreaseCurrent(size, t); 
                }
                else
                {
                    MessageBox.Show(@"Not all children will be used");
                }
            }
        }

        //Method for continuing the operation
        public static void ContinueOperation(Child child)
        {
            Reporter.WriteContent("Current: " + Operation.Current,1);
            //If there is more jobs to do
            if (Operation.Current < Operation.Stop)
            {
                //Send a job to the child that returned a job
                MessageSender.SendMessage("2,"+Operation.Current+"-"+ Operation.Size+"-"+SoftwareConfiguration.DChunk,child.IpAddress);

                Operation.IncreaseCurrent(Operation.Size,child);
            }
            else
            {
                //No more jobs left, set the child's running to false
                child.SetRunning(false);

                //Check to see if any other children are running
                var stop = true;

                foreach (var t in Operation.Children)
                {
                    if (t.Running)
                    {
                        stop = false;
                    }
                }
                //If a child is still running dont do anything
                if (!stop) return;

                //All the children are done, stop and report time.
                Sw.Stop();
                Reporter.WriteContent("Send report message, Total time: " + Sw.ElapsedMilliseconds, 1);
                //MessageSender.SendMessage("53,x"  , child.IpAddress);
                       
                //Finish the operation
                FinishOperation();
                      
                Reporter.WriteContent("Operation Complete",0);
                Reporter.EndSection();
            }
        }

        //Method for finishing the operation
        public static void FinishOperation()
        {
            //Get the list of jobs 
            var startArray = Operation.OutputDictionary.Keys.ToArray();

            //For each job send the child that did it a message to return the value of the specified index
            for (var i = 0; i < Operation.OutputDictionary.Count; i++)
            {
                Console.WriteLine(@"Requesting output from child: "+ Operation.OutputDictionary[startArray[i]].Item1.Id, 1);
                MessageSender.SendMessage("3,"+startArray[i], Operation.OutputDictionary[startArray[i]].Item1.IpAddress);
            }
        }

        //Method for setting the final output after the child returns it
        public static void SetOutputs(long index, long value)
        {
            Operation.SetOutput(index,value);
        }
    }
}
