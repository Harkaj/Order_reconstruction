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
    public partial class Mode_selection : Form
    {
        public Mode_selection()
        {
            InitializeComponent();
        }

        public int mode = 0;

        private void Standard_Click(object sender, EventArgs e)
        {
            mode = 1;

            this.DialogResult = DialogResult.OK;
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            mode = 2;

            this.DialogResult = DialogResult.OK;
        }

        private void Determine_director_field_Click(object sender, EventArgs e)
        {
            mode = 3;

            this.DialogResult = DialogResult.OK;
        }

        private void Interferometry_Click(object sender, EventArgs e)
        {
            mode = 4;

            this.DialogResult = DialogResult.OK;
        }

        private void Field_known_Click(object sender, EventArgs e)
        {
            mode = 5;

            this.DialogResult = DialogResult.OK;
        }

        private void POV_output_Click(object sender, EventArgs e)
        {
            mode = 7;

            this.DialogResult = DialogResult.OK;
        }
    }
}
