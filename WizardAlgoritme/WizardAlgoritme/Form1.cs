using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WizardAlgoritme
{
    public partial class Form1 : Form
    {
        private GridManager visualManager;
        public Form1()
        {
            InitializeComponent();

            ClientSize = new Size(500, 500);

            visualManager = new GridManager(CreateGraphics(), this.DisplayRectangle);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Loop_Tick(object sender, EventArgs e)
        {
            visualManager.GameLoop();
        }
    }
}
