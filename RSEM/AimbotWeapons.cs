using Guna.UI2.WinForms;
using RSEM.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSEM
{
    public partial class AimbotWeapons : Form
    {
        int hack = default;

        public AimbotWeapons(int _hack)
        {
            InitializeComponent();

            hack = _hack;
        }

        private void AimbotWeapons_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            int x = (Screen.PrimaryScreen.Bounds.Width / 2) - (Size.Width / 2);
            int y = (Screen.PrimaryScreen.Bounds.Height / 2) - (Size.Height / 2);

            Location = new Point(x, y);

            string[] weapons = Enum.GetNames(typeof(Enums.Weapons));

            for (int i = 0; i < weapons.Length; i++)
            {
                Guna2CheckBox CheckBox1 = new Guna2CheckBox();

                CheckBox1.Anchor = AnchorStyles.None;
                CheckBox1.AutoSize = true;
                CheckBox1.CheckedState.BorderColor = Color.FromArgb(33, 33, 33);
                CheckBox1.CheckedState.BorderRadius = 0;
                CheckBox1.CheckedState.BorderThickness = 0;
                CheckBox1.CheckedState.FillColor = Color.FromArgb(33, 33, 33);
                CheckBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 238);
                CheckBox1.Location = new Point(137, 15);
                CheckBox1.Name = "guna2CheckBox1";
                CheckBox1.Size = new Size(125, 21);
                CheckBox1.TabIndex = 0;
                CheckBox1.Cursor = Cursors.Hand;
                CheckBox1.Text = weapons[i].ToUpper();
                CheckBox1.UncheckedState.BorderColor = Color.Silver;
                CheckBox1.UncheckedState.BorderRadius = 2;
                CheckBox1.UncheckedState.BorderThickness = 1;
                CheckBox1.UncheckedState.FillColor = Color.White;

                AimTableLayout.Controls.Add(CheckBox1);
            }

            LoadSettings();
        }

        private void LoadSettings()
        {
            if (hack == 0)
            {

                string[] weapons = Settings.Aimbot.Weapons.Split(',');

                foreach (Guna2CheckBox weaponBox in AimTableLayout.Controls)
                    foreach (string weapon in weapons)
                        if (weaponBox.Text.ToLower() == weapon)
                            weaponBox.Checked = true;
            }

            else if (hack == 1) 
            {
                string[] weapons = Settings.Trigger.Weapons.Split(',');

                foreach (Guna2CheckBox weaponBox in AimTableLayout.Controls)
                    foreach (string weapon in weapons)
                        if (weaponBox.Text.ToLower() == weapon)
                            weaponBox.Checked = true;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (hack == 0)
            {
                string weapons = "";

                for (int i = 0; i < AimTableLayout.Controls.Count; i++)
                    if ((AimTableLayout.Controls[i] as Guna2CheckBox).Checked)
                        weapons += AimTableLayout.Controls[i].Text.ToLower() + ",";

                Settings.Aimbot.Weapons = weapons.Substring(0, weapons.Length - 1);
            }

            else if (hack == 1)
            {
                string weapons = "";

                for (int i = 0; i < AimTableLayout.Controls.Count; i++)
                    if ((AimTableLayout.Controls[i] as Guna2CheckBox).Checked)
                        weapons += AimTableLayout.Controls[i].Text.ToLower() + ",";

                Settings.Trigger.Weapons = weapons.Substring(0, weapons.Length - 1);
            }
            
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AimTableLayout.Controls.Count; i++)
                (AimTableLayout.Controls[i] as Guna2CheckBox).Checked = true;   
        }
    }
}
