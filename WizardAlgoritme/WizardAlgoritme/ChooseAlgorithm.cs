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
    public partial class ChooseAlgorithm : Form
    {
        Form1 game;
        public ChooseAlgorithm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //astar
            game = new Form1(1);
            game.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DFS
            game = new Form1(2);
            game.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //BFS
            game = new Form1(3);
            game.Show();
        }

        private void ChooseAlgorithm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
