using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using FNA.Core;

namespace FNA.GUI
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyDialog_cl
    {
        /// <summary>
        /// Shows a dialog box with a property grid.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult Show(string title, ref Entity_cl value)
        {
            Form mForm = new Form();
            PropertyGrid mPropertyGrid = new PropertyGrid();
            Button mOKButton = new Button();

            mForm.Text = title + " Properties";
            mPropertyGrid.SelectedObject = value;
            mOKButton.Text = "OK";
            mOKButton.DialogResult = DialogResult.OK;

            mForm.ClientSize = new Size(320, 320);
            mPropertyGrid.SetBounds(4, 4, mForm.ClientSize.Width - 4, mForm.ClientSize.Height - 40);
            mOKButton.SetBounds(mForm.ClientSize.Width / 2 - 32, mForm.ClientSize.Height - 36, 64, 22);
            
            mPropertyGrid.Anchor = AnchorStyles.Top;
            mOKButton.Anchor = AnchorStyles.Bottom;

            mForm.Controls.AddRange(new Control[] { mPropertyGrid, mOKButton });
            mForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            mForm.StartPosition = FormStartPosition.CenterScreen;
            mForm.MinimizeBox = false;
            mForm.MaximizeBox = false;
            mForm.AcceptButton = mOKButton;

            DialogResult dialogResult = mForm.ShowDialog();
            return dialogResult;
        }
    }
}