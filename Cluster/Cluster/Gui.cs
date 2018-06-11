using System.Collections.Generic;
using System.Windows.Forms;

namespace Cluster
{
    //Abstract class for each Gui
    internal abstract class Gui
    {
        //Array for all the controls
        public abstract Control[] GetControls();
        //List of child controls for the parent
        public abstract List<Control[]> GetChildControls();
        //Method to set the log on the form
        public abstract void SetLog();
    }
}
