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
    public partial class Defect_properties : Form
    {
        public Defect_properties()
        {
            InitializeComponent();
        }

        private void Potrdi_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
