using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LC_order_reconstruction_3D
{
    public partial class Input_selection : Form
    {
        public Input_selection()
        {
            InitializeComponent();
        }

        public bool boundary, interpolation, reset;
        public int readmode, changemode, factor;

        private void Confirm_Click(object sender, EventArgs e)
        {
            boundary = Use_different_boundary.Checked;
            reset = Reset_tensor.Checked;

            interpolation = Interpolation.Checked;
            if (interpolation)
            {
                factor = (int)Factor_n.Value;
            }

            if (Tensor.Checked)
            {
                readmode = 0;
            }
            else if (Director.Checked)
            {
                readmode = 1;
            }

            if (E_field.Checked)
            {
                changemode = 0;
            }
            else if (a.Checked)
            {
                changemode = 1;
            }
            else if (Change_boundary.Checked)
            {
                changemode = 2;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void Interpolation_CheckedChanged(object sender, EventArgs e)
        {
            if (Interpolation.Checked)
            {
                Factor_n.Visible = true;
                Factor_label.Visible = true;
            }
            else
            {
                Factor_n.Visible = false;
                Factor_label.Visible = false;
            }
        }
    }
}
