using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FNA.GUI
{
    /// <summary>
    /// 
    /// </summary>
    public class DropDown_cl
    {
        /// <summary>
        /// Shows a dialog box with an input field.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="options"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult Show(string title, string prompt, List<string> options, ref string value)
        {
            Form mForm = new Form();
            Label mLabel = new Label();
            ComboBox mComboBox = new ComboBox();
            Button mOKButton = new Button();

            mForm.Text = title;
            mLabel.Text = prompt;
            mComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 0; i < options.Count; i++)
            {
                mComboBox.Items.Add(options[i]);
            }
            mComboBox.SelectedIndex = 0;

            mOKButton.Text = "OK";
            mOKButton.DialogResult = DialogResult.OK;

            mForm.ClientSize = new Size(240, 80);
            mLabel.SetBounds(4, 4, mForm.ClientSize.Width - 8, 22);
            mComboBox.SetBounds(32, 26, mForm.ClientSize.Width - 64, 24);
            mOKButton.SetBounds(mForm.ClientSize.Width / 2 - 32, 54, 64, 22);

            mLabel.AutoSize = true;
            mComboBox.Anchor = AnchorStyles.Bottom;
            mOKButton.Anchor = AnchorStyles.Bottom;

            mForm.Controls.AddRange(new Control[] { mLabel, mComboBox, mOKButton });
            mForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            mForm.StartPosition = FormStartPosition.CenterScreen;
            mForm.MinimizeBox = false;
            mForm.MaximizeBox = false;
            mForm.AcceptButton = mOKButton;

            DialogResult dialogResult = mForm.ShowDialog();
            value = mComboBox.Text;
            return dialogResult;
        }
    }
}