using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cluster.Child.GUI
{
    class ChildGui : Gui
    {
        //Enumerations for the control objects
        public enum ControlEnums
        {
            UpdateButton = 0,
            RunButton = 1,
            IdLabel = 2
        }

        //Text box for printing text to the screen
        public TextBox GetTextBox { get; private set; }

        //Groupbox object that contains the textbox
        private GroupBox _messageBox;

        //Main form
        private readonly Form _mainForm;

        //Array of controls for accessing in other classes
        public Control[] UtilityControls { get; } = new Control[20];

        //Empty control
        private Control _control;

        
        //Groupbox containing all of the controls
        private GroupBox _controlGroupBox;


        //Constructor 
        public ChildGui(Form mainForm)
        {
            _mainForm = mainForm;
            CreateForm();
        }

        //overridden method used in another class
        public override List<Control[]> GetChildControls()
        {
            return null;
        }

        //Accessor for getting the controls 
        public override Control[] GetControls()
        {
            return UtilityControls;
        }

        //Method for creating the form
        private void CreateForm()
        {
            _control = new Control();
            
            _mainForm.BackColor = Color.FromArgb(68, 36, 22);
            _mainForm.Size = new Size(518, 321);

            _control.Size = new Size((int)(_mainForm.Width * .005), (int)(_mainForm.Height * .005));

            CreateGroupBoxes();
            CreateControlButtons(_controlGroupBox);
            CreateListBox(_messageBox);
        }

        //Creates the list box to display messages
        private void CreateListBox(Control parent)
        {
            var messageBox = CreateTextbox(parent, .95, .95, _control, _control, .01, .01);
            messageBox.BringToFront();
            messageBox.ReadOnly = true;
            messageBox.Multiline = true;
            messageBox.ForeColor = Color.WhiteSmoke;
            messageBox.Font = new Font("Century Gothic", ReturnFontConvert(10));
            messageBox.BackColor = Color.FromArgb(68, 36, 22);
            messageBox.WordWrap = true;
            messageBox.ScrollBars = ScrollBars.Vertical;
            GetTextBox = messageBox;
        }
        //Refreshes the log and displayes new messages
        public override void SetLog()
        {
            var frontEndLists = Reporter.FrontEndReport;

            var itemBox = GetTextBox;

            itemBox.Text = "";

            for (int i = frontEndLists.Count-1; i > 0; i--)
            {
                itemBox.Text += frontEndLists[i] + Environment.NewLine;
            }
            
            
        }

        //Creates all of the buttons on the gui
        private void CreateControlButtons(Control parent)
        {
            var updateButton = CreateButton(parent, .25, .25, _control, _control, .01, .01, Color.WhiteSmoke,
                Color.Black, "Update", new Font("Century Gothic", ReturnFontConvert(8)));
            updateButton.BringToFront();
            UtilityControls[(int) ControlEnums.UpdateButton] = updateButton;

            var runButton = CreateButton(parent, .25, .25, _control, updateButton, .01, .01, Color.WhiteSmoke,
                Color.Black, "Run", new Font("Century Gothic", ReturnFontConvert(8)));
            runButton.BringToFront();
            UtilityControls[(int) ControlEnums.RunButton] = runButton;

            var idLabel = CreateLabel(parent, .2, .2, _control, runButton, .01, .01, Color.WhiteSmoke, "No Id Yet",
                new Font("Century Gothic", ReturnFontConvert(8)));
            UtilityControls[(int) ControlEnums.IdLabel] = idLabel;

        }

        //Creates all of the groupboxes for the gui
        private void CreateGroupBoxes()
        {
            var mainGroupBox = CreateGroupBox(_mainForm, .95, .5, _control, _control, .01, .01,
                Color.FromArgb(68, 36, 22));
            mainGroupBox.BringToFront();
            mainGroupBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));
            _controlGroupBox = mainGroupBox;

            var messageBox = CreateGroupBox(_mainForm, .95, .35, _control, mainGroupBox, .01, .01,
                Color.FromArgb(191, 161, 99));
            messageBox.BringToFront();
            messageBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));
            _messageBox = messageBox;

        }


        //Ignore this please. Its how the gui is built.
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
