using System;
using System.Windows.Forms;

namespace IkariamBots.Forms
{
    public partial class Military : Form
    {
        public Military()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            calculateMilitary();
        }

        private void calculateMilitary()
        {
            int Hoplite = int.Parse(numericUpDown1.Value.ToString());
            int Steam_Giant = int.Parse(numericUpDown6.Value.ToString());
            int Swordsman = int.Parse(numericUpDown3.Value.ToString());
            int Spearman = int.Parse(numericUpDown2.Value.ToString());
            int Sulphur_Carabineer = int.Parse(numericUpDown8.Value.ToString());
            int Archer = int.Parse(numericUpDown13.Value.ToString());
            int Slinger = int.Parse(numericUpDown4.Value.ToString());
            int Mortar = int.Parse(numericUpDown9.Value.ToString());
            int Catapult = int.Parse(numericUpDown7.Value.ToString());
            int Ram = int.Parse(numericUpDown5.Value.ToString());
            int Balloon_Bombardier = int.Parse(numericUpDown11.Value.ToString());
            int Gyrocopter = int.Parse(numericUpDown10.Value.ToString());
            int Cook = int.Parse(numericUpDown12.Value.ToString());
            int Doctor = int.Parse(numericUpDown14.Value.ToString());

            double Carpenter = int.Parse(numericUpDown16.Value.ToString()) * 0.01;
            double FireworkTestArea = int.Parse(numericUpDown15.Value.ToString()) * 0.01;
            double Optician = int.Parse(numericUpDown18.Value.ToString()) * 0.01;
            double WinePress = int.Parse(numericUpDown17.Value.ToString()) * 0.01;

            double total_generals = Hoplite * 1.4 + Steam_Giant * 6.2 + Swordsman*1.2 + Spearman*0.6 + Sulphur_Carabineer*4 + Archer*1.1 + Slinger*0.4 + Mortar*31 + Catapult*11.2 + Ram*4.4 + Balloon_Bombardier*5.8 + Gyrocopter*2.5 + Cook*4 + Doctor*10;
            int population = Hoplite*1 + Steam_Giant*2 + Swordsman*1 + Spearman*1 + Sulphur_Carabineer*1 + Archer*1 + Slinger*1 + Mortar*5 + Catapult*5 + Ram*5 + Balloon_Bombardier*5 + Gyrocopter*3 + Cook*1 + Doctor*1;
            double totalWood = Hoplite*(40-40*Carpenter)+Steam_Giant*(130-130*Carpenter)+ Spearman * (30 - 30 * Carpenter) + Swordsman * (30 - 30 * Carpenter) + Slinger*(20-20*Carpenter)+Archer * (30 - 30 * Carpenter)+Sulphur_Carabineer*(50-50*Carpenter)+Ram*(220-220*Carpenter)+Catapult*(260-260*Carpenter)+Mortar*(300-300*Carpenter)+Gyrocopter*(25-25*Carpenter)+Balloon_Bombardier*(40-40*Carpenter)+Cook*(50-50*Carpenter)+Doctor*(50-50*Carpenter);
            double totalSulfur = Hoplite * (30 - 30 * FireworkTestArea) + Steam_Giant * (180 - 180 * FireworkTestArea) + Swordsman * (30 - 30 * FireworkTestArea) + Archer * (25 - 25 * FireworkTestArea) + Sulphur_Carabineer * (150 - 150 * FireworkTestArea) + Catapult * (300 - 300 * FireworkTestArea) + Mortar * (1250 - 1250 * FireworkTestArea) + Gyrocopter * (100 - 100 * FireworkTestArea) + Balloon_Bombardier * (250 - 250 * FireworkTestArea);
            double totalGlass = Doctor*(450-450*Optician);
            double totalWine = Cook*(150-150*WinePress);
            double totalGold = Hoplite * 3 + Steam_Giant * 12 + Swordsman * 4 + Spearman * 1 + Sulphur_Carabineer * 3 + Archer * 4 + Slinger * 2 + Mortar * 30 + Catapult * 25 + Ram * 15 + Balloon_Bombardier * 45 + Gyrocopter * 15 + Cook * 10 + Doctor * 20;
            double totalGoldReduction = 0;
            if (listBox2.SelectedIndex == 1)
                totalGoldReduction += 0.02;
            totalGoldReduction += int.Parse(numericUpDown19.Value.ToString())/100;
            totalGold = totalGold - totalGold * totalGoldReduction;
            double totalUpkeep = totalGold*2;

            generals.Text = total_generals.ToString("N");
            citizenes.Text = population.ToString("N");
            
            Wood.Text = ((int)Math.Round(totalWood)).ToString("N");
            Sulfur.Text = ((int)Math.Round(totalSulfur)).ToString("N");
            Glass.Text = ((int)Math.Round(totalGlass)).ToString("N"); ;
            Wine.Text = ((int)Math.Round(totalWine)).ToString("N");
            Gold.Text = ((int)Math.Round(totalGold)).ToString("N");
            Upkeep.Text = ((int)Math.Round(totalUpkeep)).ToString("N"); ;
        }
    }
}
