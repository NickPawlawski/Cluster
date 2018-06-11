using System;
using System.Windows.Forms;
using Cluster.NetworkResources;
using Cluster.Parent.GUI;

namespace Cluster.Parent
{
    public partial class Parent : Form
    {
        //Gui for the parent
        private ParentGui _gui;
        //ChildManager for the parent
        private ChildManager _cm;

        //Constructor
        public Parent()
        {
            InitializeComponent();
        }
        
        private void Parent_Load(object sender, EventArgs e)
        {
            //IGNORE THIS.......
            CheckForIllegalCrossThreadCalls = false;
            //Creates the parent Gui
            _gui = new ParentGui(this);
            SetActions();
            //Creates child manager and sets the message parser 
            _cm = new ChildManager();
            MessageParser.SetGui(_gui);
            MessageParser.SetChildManager(_cm);
        }

        //Sets the action of refreshing and closing of the form
        private void SetActions()
        {
            _gui.UtilityControls[(int)ParentGui.ControlEnums.RefreshChildren].Click += RefreshChildrenButton_Click;
            FormClosing += Form_Close;
        }

        //Sets the event of refreshing the child list
        private void RefreshChildrenButton_Click(object sender, EventArgs e)
        {
            var children = _cm.Children;
            _gui.AddChild(children);
        }

        //Sets the event of closing the form
        private void Form_Close(object sender, FormClosingEventArgs e)
        {
            foreach (var child in _cm.Children)
            {
                Console.WriteLine(@"Killing child");
                MessageSender.SendMessage("52,a",child.Key);
            }
        }
    }
}
