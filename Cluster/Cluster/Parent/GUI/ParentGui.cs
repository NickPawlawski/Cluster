using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cluster.Parent.GUI
{
    class ParentGui : Gui
    {
        public enum ControlEnums
        {
            RefreshChildren = 0
        }

        public Control[] UtilityControls { get; } = new Control[20];
        public override Control[] GetControls()
        {
            return UtilityControls;
        }

        private Form _mainForm;
        private Control _control;

        private GroupBox _controlBox;



        public ParentGui(Form mainForm)
        {
            _mainForm = mainForm;
            CreateForm();
            
        }

        private void CreateForm()
        {
            _control = new Control();

            //_mainForm.FormBorderStyle = FormBorderStyle.None;
            //68, 36, 22
            _mainForm.BackColor = Color.FromArgb(68, 36, 22);

            _mainForm.Size = new Size(518, 321);

            _control.Size = new Size((int)(_mainForm.Width * .005), (int)(_mainForm.Height * .005));

            CreateGroupBoxes();
            CreateControlBox(_controlBox);
        }

        public void AddChild(Child child)
        {
            Console.WriteLine(child.Ip);
        }

        private void CreateControlBox(Control parent)
        {
            var refreshChildrenButton = CreateButton(parent, .25, .45, _control, _control, .01, .01, Color.WhiteSmoke,
                Color.Black, "Refresh Children", new Font("Century Gothic", ReturnFontConvert(8)));
            refreshChildrenButton.BringToFront();
            UtilityControls[(int)ControlEnums.RefreshChildren] = refreshChildrenButton;

        }

        private void CreateGroupBoxes()
        {
            var controlBox = CreateGroupBox(_mainForm, .95, .3, _control, _control, .01, .01, Color.FromArgb(191, 161, 99));
            controlBox.BringToFront();
            controlBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));
            _controlBox = controlBox;

            //controlBox.Paint += PaintBorderlessGroupBox;

            var childBox = CreateGroupBox(_mainForm, .95, .5, _control, controlBox, .01, .01, Color.Gray);
            childBox.BringToFront();
            childBox.Font = new Font("Century Gothic", ReturnFontConvert(1000));

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
            return (int)Math.Round(fontSize * Screen.PrimaryScreen.Bounds.Width / 1920);
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
