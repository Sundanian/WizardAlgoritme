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
        public int algorithm;

        public Form1(int algorithm)
        {
            this.algorithm = algorithm;

            InitializeComponent();

            ClientSize = new Size(500, 500);

            visualManager = new GridManager(CreateGraphics(), this.DisplayRectangle, algorithm);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Loop_Tick(object sender, EventArgs e)
        {
            visualManager.GameLoop();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                visualManager.Wizard.Position = visualManager.Wizard.GetNextMove(algorithm);
            }
        }
    }
}
