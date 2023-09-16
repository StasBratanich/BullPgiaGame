using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBoard
{
    public partial class FormGameStart : Form
    {
        public FormGameStart()
        {
            InitializeComponent();
        }
        private void buttonSetGuesses_Click(object sender, EventArgs e)
        {
            if (m_NumberOfTries == 10)
            {
                m_NumberOfTries = 4;
            }
            else
            {
                m_NumberOfTries++;
            }

            NumberOfChancesBtn.Text = $"Number of chances: {m_NumberOfTries}";
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormGameBoard formGameBoard = new FormGameBoard(m_NumberOfTries);
            formGameBoard.Show();
        }
    }
}
