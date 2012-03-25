using System;
using System.Drawing;
using System.Windows.Forms;

namespace FNA.GUI
{
    /// <summary>
    /// 
    /// </summary>
    public class InputDialog_cl
    {
        /// <summary>
        /// Shows a dialog box with an input field.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult Show(string title, string prompt, ref string value)
        {
            Form mForm = new Form();
            Label mLabel = new Label();
            TextBox mTextBox = new TextBox();
            Button mOKButton = new Button();

            mForm.Text = title;
            mLabel.Text = prompt;
            mTextBox.Text = value;

            mOKButton.Text = "OK";
            mOKButton.DialogResult = DialogResult.OK;

            mForm.ClientSize = new Size(240, 80);
            mLabel.SetBounds(4, 4, mForm.ClientSize.Width-8, 22);
            mTextBox.SetBounds(32, 26, mForm.ClientSize.Width - 64, 24);
            mOKButton.SetBounds(mForm.ClientSize.Width / 2 - 32, 54, 64, 22);

            mLabel.AutoSize = true;
            mTextBox.Anchor = AnchorStyles.Bottom;
            mOKButton.Anchor = AnchorStyles.Bottom;

            mForm.Controls.AddRange(new Control[] { mLabel, mTextBox, mOKButton });
            mForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            mForm.StartPosition = FormStartPosition.CenterScreen;
            mForm.MinimizeBox = false;
            mForm.MaximizeBox = false;
            mForm.AcceptButton = mOKButton;

            DialogResult dialogResult = mForm.ShowDialog();
            value = mTextBox.Text;
            return dialogResult;
        }
    }
}
