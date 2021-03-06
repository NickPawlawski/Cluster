﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Cluster.NetworkResources;
using Cluster.Parent.Operation;

namespace Cluster.Parent.GUI
{
    internal class ParentGui : Gui
    {
        public enum ControlEnums
        {
            RefreshChildren = 0,
            ChunkSize = 1,
            Min = 2,
            Max = 3,
            Report = 4,
            SetAll = 5,
            ResetAll = 6
        }

        private readonly List<Control[]> _childList = new List<Control[]>();

        public override List<Control[]> GetChildControls()
        {
            return _childList;
        }
        
        public Control[] UtilityControls { get; } = new Control[20];
        public override Control[] GetControls()
        {
            return UtilityControls;
        }

        private readonly Form _mainForm;
        private Control _control;

        private GroupBox _controlBox;
        private GroupBox _childBox;
        private GroupBox _messageBox;

        public TextBox GetTextBox { get; private set; }

        public ParentGui(Form mainForm)
        {
            _mainForm = mainForm;
            CreateForm();
            
        }

        private void CreateForm()
        {
            _control = new Control();

            _mainForm.BackColor = Color.FromArgb(68, 36, 22);

            _mainForm.Size = new Size(1000, 700);

            _control.Size = new Size((int)(_mainForm.Width * .005), (int)(_mainForm.Height * .005));

            CreateGroupBoxes();
            CreateControlBox(_controlBox);
            CreateListBox(_messageBox);
        }

        private void CreateListBox(Control parent)
        {
            var messageBox = CreateTextbox(parent, .95, .95, _control, _control, .01, .01);
            messageBox.BringToFront();
            messageBox.ReadOnly = true;
            messageBox.Multiline = true;
            messageBox.Font = new Font("Century Gothic", ReturnFontConvert(10));
            messageBox.BackColor = Color.FromArgb(68, 36, 22);
            messageBox.WordWrap = true;
            messageBox.ScrollBars = ScrollBars.Vertical;
            GetTextBox = messageBox;
        }

        public void AddChild(Dictionary<IPAddress,Child> children)
        {
            _childBox.Controls.Clear();

            _childList.Clear();

            var vertControl = _control;

            for (int i = 0; i < children.Count; i++)
            {
                if (_childList.Count > 0)
                {
                    vertControl = _childList.ElementAt(_childList.Count - 1)[0];
                }
                
                var childControls = new Control[5];

                var name = children.ElementAt(i).Key.ToString();
                var button = CreateLabel(_childBox, .2, .2, _control, vertControl, .01, .01, Color.Black, name, new Font("Century Gothic", ReturnFontConvert(8)));
                button.Tag = name;
                childControls[0] = button;

                var button2 = CreateButton(_childBox, .2, .2, button, vertControl, .01, .01, Color.WhiteSmoke,
                    Color.Black, "Ready/Remove", new Font("Century Gothic", ReturnFontConvert(8)));
                button2.Click += ReadyChild;
                button2.Tag = name;
                childControls[1] = button2;

                var readyBox = CreatePictureBox(_childBox, .2, .2, button2, vertControl, .01, .01, Color.Red);
                childControls[2] = readyBox;

                var updateButton = CreateButton(_childBox, .2, .2, readyBox, vertControl, .01, .01, Color.WhiteSmoke,
                    Color.Black, "Update", new Font("Century Gothic", ReturnFontConvert(8)));
                childControls[3] = updateButton;
                updateButton.Tag = name;
                updateButton.Click += SendUpdate;
                _childList.Add(childControls);
            }
        }

        public override void SetLog()
        {
            var frontEndLists = Reporter.FrontEndReport;

            var itemBox = GetTextBox;

            itemBox.Text = "";

            for (int i = frontEndLists.Count - 1; i > 0; i--)
            {
                itemBox.Text += frontEndLists[i] + Environment.NewLine;
            }
        }

        private static void ReadyChild(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            IPAddress childIp = IPAddress.Parse(b.Tag.ToString());
            MessageSender.SendMessage("1,1",childIp);
        }


        private void SendUpdate(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            
            MessageSender.SendMessage("50,", IPAddress.Parse(b.Tag.ToString()));
        }

        private void CreateControlBox(Control parent)
        {
            var refreshChildrenButton = CreateButton(parent, .25, .45, _control, _control, .01, .01, Color.WhiteSmoke,
                Color.Black, "Refresh Children", new Font("Century Gothic", ReturnFontConvert(8)));
            refreshChildrenButton.BringToFront();
            UtilityControls[(int)ControlEnums.RefreshChildren] = refreshChildrenButton;

            var runButton = CreateButton(parent, .25, .45, refreshChildrenButton, _control, .01, .01, Color.WhiteSmoke,
                Color.Black, "Run", new Font("Century Gothic", ReturnFontConvert(8)));
            runButton.Click += RunOperation;

            var chunkSizeLabel = CreateLabel(parent, .2, .2, runButton, _control, .01, .01, Color.WhiteSmoke,
                "Chunk Size", new Font("Century Gothic", ReturnFontConvert(8)));

            var chunkSizeTextbox = CreateTextbox(parent, .2, .2, runButton, chunkSizeLabel, .01, .01);
            chunkSizeTextbox.Font = new Font("Century Gothic", ReturnFontConvert(8));
            UtilityControls[(int)ControlEnums.ChunkSize] = chunkSizeTextbox;

            var minLabel = CreateLabel(parent, .2, .2, chunkSizeLabel, _control, .01, .01, Color.WhiteSmoke,
                "Min", new Font("Century Gothic", ReturnFontConvert(8)));

            var minTextbox = CreateTextbox(parent, .2, .2, chunkSizeLabel, minLabel, .01, .01);
            UtilityControls[(int) ControlEnums.Min] = minTextbox;
            minTextbox.Font = new Font("Century Gothic", ReturnFontConvert(8));
            var maxLabel = CreateLabel(parent, .2, .2, chunkSizeLabel, minTextbox, .01, .01, Color.WhiteSmoke,
                "Max", new Font("Century Gothic", ReturnFontConvert(8)));

            var maxTextbox = CreateTextbox(parent, .2, .2, chunkSizeLabel, maxLabel, .01, .01);
            UtilityControls[(int) ControlEnums.Max] = maxTextbox;
            maxTextbox.Font = new Font("Century Gothic", ReturnFontConvert(8));

            var reportButton = CreateButton(parent, .25, .45, _control, refreshChildrenButton, .01, .01, Color.WhiteSmoke,
                Color.Black, "Report", new Font("Century Gothic", ReturnFontConvert(8)));
            UtilityControls[(int)ControlEnums.Report] = reportButton;
            reportButton.Click += PrintReport;

            var setRunButton = CreateButton(parent, .25, .45, reportButton, runButton, .01, .01, Color.WhiteSmoke,
                Color.Black, "Ready All", new Font("Century Gothic", ReturnFontConvert(8)));
            UtilityControls[(int) ControlEnums.SetAll] = setRunButton;
            setRunButton.Click += ReadyAll;

            var resetButton = CreateButton(parent, .2, .45, setRunButton, runButton, .01, .01, Color.WhiteSmoke,
                Color.Black, "Reset All", new Font("Century Gothic", ReturnFontConvert(8)));
            UtilityControls[(int)ControlEnums.ResetAll] = resetButton;
            resetButton.Click += ResetAll;
        }

        private void ResetAll(object sender, EventArgs e)
        {
            foreach (var child in _childList)
            {
                Button b =(Button)child[1];
                
                IPAddress childIp = IPAddress.Parse(b.Tag.ToString());
                
                MessageSender.SendMessage("1,0", childIp);

                child[2].BackColor = Color.Red;
                
            }
            Button c = (Button) UtilityControls[(int) ControlEnums.RefreshChildren];
            c.PerformClick();
            OperationManager.ResetOperation();
        }

        private void ReadyAll(object sender, EventArgs e)
        {
            Button b;
            foreach (Control[] t in _childList)
            {
                b = (Button)t[1];
                b.PerformClick();
            }
        }

        private static void PrintReport(object sender, EventArgs e)
        {
            var output = Operation.Operation.RealOutputDictionary.Values.ToArray();

            var finalTotal = output.Aggregate<long, double>(0, (current, total) => current + total);

            var est = finalTotal / (Operation.Operation.Size*output.Length) * 4;
            MessageBox.Show(@"The Estimation calculated is: "+est,@"Pi Estimation");
        }

        private void RunOperation(object sender, EventArgs e)
        {
            long chunkSize;
            long min;
            long max;

            try
            {
                chunkSize = long.Parse(UtilityControls[(int) ControlEnums.ChunkSize].Text);
                min = long.Parse(UtilityControls[(int)ControlEnums.Min].Text);
                max = long.Parse(UtilityControls[(int)ControlEnums.Max].Text);
            }
            catch (Exception)
            {
                MessageBox.Show(@"One of the text boxes does not have a valid value");
                return;
            }

            OperationManager.StartOperation(min,max,chunkSize);
        }

        private void CreateGroupBoxes()
        {
            var controlBox = CreateGroupBox(_mainForm, .95, .25, _control, _control, .01, .01, Color.FromArgb(191, 161, 99));
            controlBox.BringToFront();
            controlBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));
            _controlBox = controlBox;

            var childBox = CreateGroupBox(_mainForm, .95, .3, _control, controlBox, .01, .01, Color.Gray);
            childBox.BringToFront();
            childBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));
            _childBox = childBox;

            var messageBox = CreateGroupBox(_mainForm, .95, .35, _control, childBox, .01, .01,
                Color.FromArgb(191, 161, 99));
            messageBox.BringToFront();
            messageBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));
            _messageBox = messageBox;
        }
        
        #region Create objects
        private static Button CreateButton(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding,
            Color backColor, Color foreColor, string text, Font font)
        {
            var button = new Button();
            parent.Controls.Add(button);
            button.Height = ReturnHitConvert(height, parent);
            button.Width = ReturnWidConvert(width, parent);
            button.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Text = text;
            button.Font = font;
            return button;
        }

        private static Label CreateLabel(Control parent, double width, double height, Control leftControl, Control aboveControl,
            double xPadding, double yPadding, Color foreColor, string text, Font font)
        {
            var label = new Label();
            parent.Controls.Add(label);
            label.Height = ReturnHitConvert(height, parent);
            label.Width = ReturnWidConvert(width, parent);
            label.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            label.ForeColor = foreColor;
            label.Text = text;
            label.Font = font;
            return label;
        }

        private static TextBox CreateTextbox(Control parent, double height, double width, Control leftControl,
            Control aboveControl, double xPadding, double yPadding)
        {
            var textBox = new TextBox();
            parent.Controls.Add(textBox);
            textBox.AutoSize = false;
            textBox.Height = ReturnHitConvert(height, parent);
            textBox.Width = ReturnWidConvert(width, parent);
            textBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            textBox.BringToFront();
            return textBox;
        }

        private static PictureBox CreatePictureBox(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            var pictureBox = new PictureBox();
            parent.Controls.Add(pictureBox);
            pictureBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            pictureBox.Height = ReturnHitConvert(height, parent);
            pictureBox.Width = ReturnWidConvert(width, parent);
            pictureBox.BackColor = backColor;
            return pictureBox;
        }

        private static TabControl CreateTabControl(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            var tabControl = new TabControl();
            parent.Controls.Add(tabControl);
            tabControl.Height = ReturnHitConvert(height, parent);
            tabControl.Width = ReturnWidConvert(width, parent);
            tabControl.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            tabControl.BackColor = backColor;
            return tabControl;
        }

        private static ListBox CreateListBox(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            var groupBox = new ListBox();
            parent.Controls.Add(groupBox);
            groupBox.Height = ReturnHitConvert(height, parent);
            groupBox.Width = ReturnWidConvert(width, parent);
            groupBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            groupBox.BackColor = backColor;
            return groupBox;
        }

        private static GroupBox CreateGroupBox(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            var groupBox = new GroupBox();
            parent.Controls.Add(groupBox);
            groupBox.Height = ReturnHitConvert(height, parent);
            groupBox.Width = ReturnWidConvert(width, parent);
            groupBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            groupBox.BackColor = backColor;
            return groupBox;
        }

        #region Window Scaling
        private static int ReturnWidConvert(double integer, Control control)
        {
            return (int)Math.Round(integer * control.Width);
        }

        private static int ReturnHitConvert(double integer, Control control)
        {
            return (int)Math.Round(integer * control.Height);
        }

        private static int ReturnFontConvert(double fontSize)
        {
            return (int)Math.Round(fontSize * Screen.PrimaryScreen.Bounds.Height / 1080);
        }

        #endregion
        #endregion

        private static void PaintBorderlessGroupBox(object sender, PaintEventArgs p)
        {
            var box = (GroupBox)sender;
            p.Graphics.Clear(Color.FromArgb(68, 36, 22));
            p.Graphics.DrawString(box.Text, box.Font, Brushes.Black, 0, 0);
        }
    }
}
