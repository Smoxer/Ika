using System;
using System.Windows.Forms;

namespace IkariamBots.Forms
{
    public partial class SendGyro : Form
    {
        public int PerRound, Rounds;
        public string Unit, Option;

        public SendGyro()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerRound = Decimal.ToInt32(numericUpDown1.Value);
            Rounds = Decimal.ToInt32(numericUpDown2.Value);
            if (radioButton1.Checked)
                Unit = "Hoplite";
            else if (radioButton2.Checked)
                Unit = "Spearman";
            else if (radioButton3.Checked)
                Unit = "Swordsman";

            if (radioButton4.Checked)
                Option = "Occupy";
            else if (radioButton5.Checked)
                Option = "DeployUnits";
            else if (radioButton6.Checked)
                Option = "Raid";
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
