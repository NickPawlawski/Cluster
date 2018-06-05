using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Cluster.NetworkResources;
using Cluster.Parent.GUI;

namespace Cluster.Parent
{
    public partial class Parent : Form
    {
        private MessageListener ml = new MessageListener();
        private MessageSender ms = new MessageSender();

        private GUI.ParentGui _gui;
        private ChildManager _cm;

        public Parent()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CheckForIllegalCrossThreadCalls = false;
            _gui = new ParentGui(this);
            SetActions();

            _cm = new ChildManager();
            MessageParser.SetGui(_gui);
            MessageParser.SetChildManager(_cm);
        }

        private void SetActions()
        {
            _gui.UtilityControls[(int)ParentGui.ControlEnums.RefreshChildren].Click += RefreshChildrenButton_Click;
        }

        private void RefreshChildrenButton_Click(object sender, EventArgs e)
        {
            List<Child> children = _cm.Children;

            foreach (var child in children)
            {
                _gui.AddChild(child);
            }

        }
    }
}
