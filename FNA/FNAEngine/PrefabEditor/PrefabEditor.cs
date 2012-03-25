using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrefabEditor
{
    public partial class PrefabEditor : Form
    {
        PrefabEditorPanel mPrefabPanel;

        public PrefabEditor()
        {
            InitializeComponent();

            mPrefabPanel = new PrefabEditorPanel();
            mPrefabPanel.Initialize();
            Controls.Add(mPrefabPanel);
        }
    }
}
