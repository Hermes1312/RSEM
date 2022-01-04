using Guna.UI2.WinForms;
using Newtonsoft.Json.Linq;
using RSEM.Features;
using RSEM.Managers;
using RSEM.Other;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Extensions = RSEM.Other.Extensions;

namespace RSEM
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);


        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        public int mouseLeave = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void ToggleCheat(string name)
        {
            switch (name)
            {
                case "Aimbot":
                    aimToggleSwitch.Checked = !aimToggleSwitch.Checked;
                    break;

                case "Trigger":
                    triggerbotToggleSwitch.Checked = !triggerbotToggleSwitch.Checked;
                    break;

                case "Chams":
                    chamsToggleSwitch.Checked = !chamsToggleSwitch.Checked;
                    break;

                case "Glow":
                    
                    break;

                case "Bunnyhop":
                    bunnyToggleSwitch.Checked = !bunnyToggleSwitch.Checked;
                    break;

                case "Radar":
                    radarToggleSwitch.Checked = !radarToggleSwitch.Checked;
                    break;
            }
        }

        private void GetLastConfig()
        {
            if (File.Exists("last"))
            {
                string path = File.ReadAllText("last");

                Config.Load(path);
                LoadConfigToForm();
            }
        }

        private void GetOffsets()
        {
            /*WebClient client = new WebClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            string jsonUrl = @"https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.json",
                   jsonContent = client.DownloadString(jsonUrl),
                   updateVersion = client.DownloadString(@"https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.cs").Split('\n')[2].Remove(0, 3).Substring(0, 10);

            JObject json = JObject.Parse(jsonContent);

            Offsets.dwEntityList = (int)json["signatures"]["dwEntityList"];
            Offsets.m_hActiveWeapon = (int)json["netvars"]["m_hActiveWeapon"];
            Offsets.m_iItemDefinitionIndex = (int)json["netvars"]["m_iItemDefinitionIndex"];
            Offsets.m_bSpottedByMask = (int)json["netvars"]["m_bSpottedByMask"];
            Offsets.dwForceJump = (int)json["signatures"]["dwForceJump"];
            Offsets.m_clrRender = (int)json["netvars"]["m_clrRender"];
            Offsets.model_ambient_min = (int)json["signatures"]["model_ambient_min"];
            Offsets.m_dwBoneMatrix = (int)json["netvars"]["m_dwBoneMatrix"];
            Offsets.m_bIsScoped = (int)json["netvars"]["m_bIsScoped"];
            Offsets.m_iDefaultFOV = (int)json["netvars"]["m_iDefaultFOV"];
            Offsets.dwGlowObjectManager = (int)json["signatures"]["dwGlowObjectManager"];
            Offsets.m_bIsDefusing = (int)json["netvars"]["m_bIsDefusing"];
            Offsets.m_flFlashMaxAlpha = (int)json["netvars"]["m_flFlashMaxAlpha"];
            Offsets.m_lifeState = (int)json["netvars"]["m_lifeState"];
            Offsets.m_iHealth = (int)json["netvars"]["m_iHealth"];
            Offsets.m_fFlags = (int)json["netvars"]["m_fFlags"];
            Offsets.m_iTeamNum = (int)json["netvars"]["m_iTeamNum"];
            Offsets.m_iShotsFired = (int)json["netvars"]["m_iShotsFired"];
            Offsets.m_iCrosshairId = (int)json["netvars"]["m_iCrosshairId"];
            Offsets.m_bDormant = (int)json["signatures"]["m_bDormant"];
            Offsets.m_MoveType = (int)json["netvars"]["m_MoveType"];
            Offsets.m_vecOrigin = (int)json["netvars"]["m_vecOrigin"];
            Offsets.m_aimPunchAngle = (int)json["netvars"]["m_aimPunchAngle"];
            Offsets.m_vecViewOffset = (int)json["netvars"]["m_vecViewOffset"];
            Offsets.m_bSpotted = (int)json["netvars"]["m_bSpotted"];
            Offsets.dwClientState_State = (int)json["signatures"]["dwClientState_State"];
            Offsets.dwClientState_MaxPlayer = (int)json["signatures"]["dwClientState_MaxPlayer"];
            Offsets.dwClientState_ViewAngles = (int)json["signatures"]["dwClientState_ViewAngles"];*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetLastConfig();

            StartPosition = FormStartPosition.CenterScreen;
            int x = (Screen.PrimaryScreen.Bounds.Width / 2) - (Size.Width / 2);
            int y = (Screen.PrimaryScreen.Bounds.Height / 2) - (Size.Height / 2);

            Location = new Point(x, y);

            //GetOffsets();

            ThreadManager.form1 = this;
            Globals.Proc.Process = Extensions.Proc.WaitForProcess(Globals.Proc.Name);
            Extensions.Proc.WaitForModules(Globals.Proc.Modules, Globals.Proc.Name);
            MemoryManager.Initialize(Globals.Proc.Process.Id);

            ThreadManager.Add("Watcher", Watcher.Run);
            ThreadManager.Add("Reader", Reader.Run);
            ThreadManager.Add("Bunnyhop", Bunnyhop.Run);
            ThreadManager.Add("Trigger", Trigger.Run);
            ThreadManager.Add("Glow", Glow.Run);
            ThreadManager.Add("Radar", Radar.Run);
            ThreadManager.Add("Aimbot", Aimbot.Run);
            ThreadManager.Add("Chams", Chams.Run);
            ThreadManager.Add("NoFlash", NoFlash.Run);
            ThreadManager.Add("FoV", FoV.Run);
            ThreadManager.Add("AutoPistol", AutoPistol.Run);

            ThreadManager.ToggleThread("Watcher");
            ThreadManager.ToggleThread("Reader");

            if (Settings.Bunnyhop.Enabled) ThreadManager.ToggleThread("Bunnyhop");
            if (Settings.Trigger.Enabled) ThreadManager.ToggleThread("Trigger");
            if (Settings.Glow.Enabled) ThreadManager.ToggleThread("Glow");
            if (Settings.Radar.Enabled) ThreadManager.ToggleThread("Radar");
            if (Settings.Aimbot.Enabled) ThreadManager.ToggleThread("Aimbot");
            if (Settings.Chams.Enabled) ThreadManager.ToggleThread("Chams");
            if (Settings.FoV.Enabled) ThreadManager.ToggleThread("FoV");
            if (Settings.NoFlash.Enabled) ThreadManager.ToggleThread("NoFlash");
            if (Settings.AutoPistol.Enabled) ThreadManager.ToggleThread("AutoPistol");

            AimTableLayout.SetColumnSpan(labelAimbot, 2);
            AimTableLayout.SetColumnSpan(labelTrigger, 2);
            AimTableLayout.SetColumnSpan(labelChams, 2);
            AimTableLayout.SetColumnSpan(labelMiscs, 2);
            AimTableLayout.SetColumnSpan(labelKeyBind, 2);

            LoadConfigToForm();
        }

        private void PanelColorPick_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if(cd.ShowDialog() == DialogResult.OK)
            {
                (sender as Guna2Panel).FillColor = cd.Color;
            }
        }

        private void GetKeyControl_KeyUp(object sender, KeyEventArgs e)
        {
            (sender as Guna2TextBox).Text = e.KeyCode.ToString();
        }

        private void aimToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RSEMComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((sender as Guna2ComboBox).SelectedIndex == 1)
            {
                (sender as Guna2ComboBox).SelectedIndex = 0;

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Config file (*.ini)|*.ini|All files (*.*)|*.*";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Config.Save(sfd.FileName);
                    File.WriteAllText("last", sfd.FileName);
                }
            }

            if((sender as Guna2ComboBox).SelectedIndex == 2)
            {
                (sender as Guna2ComboBox).SelectedIndex = 0;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Config file (*.ini)|*.ini|All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText("last", ofd.FileName);
                    Config.Load(ofd.FileName);
                    LoadConfigToForm();
                }
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            Settings.Aimbot.Enabled                     = aimToggleSwitch.Checked;
            Settings.Aimbot.OnylVisible                 = aimVisibleToggleSwitch.Checked;
            Settings.Aimbot.Nearest                     = aimNearestToggleSwitch.Checked;
            Settings.Aimbot.FovEnabled                  = aimFovToggleSwitch.Checked;
            Settings.Aimbot.Bone                        = GetBone(aimBoneComboBox.SelectedItem.ToString());
            Settings.Aimbot.Fov                         = (float)aimFovNUD.Value;
            Settings.Aimbot.Smooth                      = aimSmoothTrackBar.Value;
            Settings.Aimbot.RecoilControl               = rcsToggleSwitch.Checked;
            Settings.Aimbot.YawRecoilReductionFactory   = rcsTrackBar.Value/10;
            Settings.Aimbot.PitchRecoilReductionFactory = rcsTrackBar.Value/10;

            Settings.Trigger.Enabled                    = triggerbotToggleSwitch.Checked;
            Settings.Trigger.Delay                      = (int)triggerDelayNUD.Value;

            Settings.Glow.Enabled           = chamsToggleSwitch.Checked;
            Settings.Glow.Enemies_Color_R   = glowEnemiesColorPanel.FillColor.R;
            Settings.Glow.Enemies_Color_G   = glowEnemiesColorPanel.FillColor.G;
            Settings.Glow.Enemies_Color_B   = glowEnemiesColorPanel.FillColor.B;

            Settings.Glow.Allies            = !glowOEToggleSwitch.Checked;
            Settings.Glow.Allies_Color_R    = glowTeamColorPanel.FillColor.R;
            Settings.Glow.Allies_Color_G    = glowTeamColorPanel.FillColor.G;
            Settings.Glow.Allies_Color_B    = glowTeamColorPanel.FillColor.B;

            Settings.Glow.Spotted_Color_R = glowSpottedColorPanel.FillColor.R;
            Settings.Glow.Spotted_Color_G = glowSpottedColorPanel.FillColor.G;
            Settings.Glow.Spotted_Color_B = glowSpottedColorPanel.FillColor.B;

            Settings.Bunnyhop.Enabled   = bunnyToggleSwitch.Checked;
            Settings.Radar.Enabled      = radarToggleSwitch.Checked;


            Settings.Trigger.Key = (Keys)Enum.Parse(typeof(Keys), kbTriggerbotKey.Text);

            Settings.Keys.Aimbot = (Keys)Enum.Parse(typeof(Keys), kbToggleAim.Text);
            Settings.Keys.Bunnyhop = (Keys)Enum.Parse(typeof(Keys), kbBunnyhop.Text);
            Settings.Keys.Radar = (Keys)Enum.Parse(typeof(Keys), kbRadar.Text);
            Settings.Keys.Glow = (Keys)Enum.Parse(typeof(Keys), kbToggleGlow.Text);
            Settings.Keys.Trigger = (Keys)Enum.Parse(typeof(Keys), kbTriggerToggle.Text);

            Settings.FoV.Enabled = fovToggleSwitch.Checked;
            Settings.FoV.Value = (int)miscsFovNUD.Value;

            Settings.NoFlash.Enabled = noFlashToggleSwitch.Checked;
            Settings.NoFlash.Value = (float)miscsNoFlashTrackBar.Value;

            Settings.AutoPistol.Enabled = autoPistolToggleSwitch.Checked;
            Settings.AutoPistol.Key = (Keys)Enum.Parse(typeof(Keys), kbAutoPistol.Text);

            if (glowTypeCombo.SelectedIndex == 0)
            {
                Settings.Glow.FullBloom = false;
            }

            if (glowTypeCombo.SelectedIndex == 1)
            {
                Settings.Glow.FullBloom = true;
            }

        }

        private void LoadConfigToForm()
        {
            aimToggleSwitch.Checked = Settings.Aimbot.Enabled;
            aimVisibleToggleSwitch.Checked = Settings.Aimbot.OnylVisible;
            aimNearestToggleSwitch.Checked = Settings.Aimbot.Nearest;
            aimFovToggleSwitch.Checked = Settings.Aimbot.FovEnabled;
            aimBoneComboBox.SelectedIndex = GetBone(Settings.Aimbot.Bone);
            aimFovNUD.Value = (int)Settings.Aimbot.Fov;
            aimSmoothTrackBar.Value = (int)Settings.Aimbot.Smooth;
            rcsToggleSwitch.Checked = Settings.Aimbot.RecoilControl;
            rcsTrackBar.Value = (int)Settings.Aimbot.YawRecoilReductionFactory*10;

            triggerbotToggleSwitch.Checked = Settings.Trigger.Enabled;
            triggerDelayNUD.Value = Settings.Trigger.Delay;

            chamsToggleSwitch.Checked = Settings.Glow.Enabled;
            glowEnemiesColorPanel.FillColor = Color.FromArgb((int)Settings.Glow.Enemies_Color_R, (int)Settings.Glow.Enemies_Color_G, (int)Settings.Glow.Enemies_Color_B);
            glowTeamColorPanel.FillColor = Color.FromArgb((int)Settings.Glow.Allies_Color_R, (int)Settings.Glow.Allies_Color_G, (int)Settings.Glow.Allies_Color_B);
            glowSpottedColorPanel.FillColor = Color.FromArgb((int)Settings.Glow.Spotted_Color_R, (int)Settings.Glow.Spotted_Color_G, (int)Settings.Glow.Spotted_Color_B);

            bunnyToggleSwitch.Checked = Settings.Bunnyhop.Enabled;
            radarToggleSwitch.Checked = Settings.Radar.Enabled;

            kbToggleAim.Text = Settings.Keys.Aimbot.ToString();
            kbAimKey.Text = Settings.Keys.Aimbot.ToString();
            kbBunnyhop.Text = Settings.Keys.Bunnyhop.ToString();
            kbRadar.Text = Settings.Keys.Radar.ToString();
            kbToggleGlow.Text = Settings.Keys.Glow.ToString();
            kbTriggerbotKey.Text = Settings.Trigger.Key.ToString();
            kbTriggerToggle.Text = Settings.Keys.Trigger.ToString();     

            kbAutoPistol.Text = Settings.AutoPistol.Key.ToString();
            autoPistolToggleSwitch.Checked = Settings.AutoPistol.Enabled;
        }

        private static int GetBone(string value)
        {
            switch (value)
            {
                case "Head":        return 8;
                case "Neck":        return 7;
                case "Upper torso": return 6;
                case "Lower torso": return 5;
                case "Ass":         return 3;
            }

            return 8;
        }
        private static int GetBone(int value)
        {
            switch (value)
            {
                case 8:         return 0;
                case 7:         return 1;
                case 6:         return 2;
                case 5:         return 3;
                case 3:         return 4;
            }

            return 0;
        }

        private void glowTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            new AimbotWeapons(0).Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            new AimbotWeapons(1).Show();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            new AimbotWeapons(1).Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MemoryManager.WriteMemory<int>(Structs.LocalPlayer.Base + Offsets.m_iDefaultFOV, 90);
            MemoryManager.WriteMemory<float>(Structs.LocalPlayer.Base + Offsets.m_flFlashMaxAlpha, 255);

            for (int i = 0; i < Structs.ClientState.MaxPlayers; i++)
            {
                int cEntity = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwEntityList + (i - 1) * 16);
                Structs.Enemy_t cEntityStruct = MemoryManager.ReadMemory<Structs.Enemy_t>(cEntity);

                if (!cEntityStruct.Health.IsAlive()
                    || cEntityStruct.Dormant
                    || !cEntityStruct.Team.HasTeam()) continue;

                if (Settings.Chams.Enemies && !cEntityStruct.Team.IsMyTeam())
                {
                    Structs.clrRender_t clrRenderStruct = new Structs.clrRender_t()
                    {
                        r = Convert.ToByte(255),
                        g = Convert.ToByte(255),
                        b = Convert.ToByte(255),
                        a = Convert.ToByte(255)
                    };

                    MemoryManager.WriteMemory<Structs.clrRender_t>(cEntity + Offsets.m_clrRender, clrRenderStruct);
                }

                if (Settings.Chams.Allies && cEntityStruct.Team.IsMyTeam())
                {
                    Structs.clrRender_t clrRenderStruct = new Structs.clrRender_t()
                    {
                        r = Convert.ToByte(255),
                        g = Convert.ToByte(255),
                        b = Convert.ToByte(255),
                        a = Convert.ToByte(255)
                    };

                    MemoryManager.WriteMemory<Structs.clrRender_t>(cEntity + Offsets.m_clrRender, clrRenderStruct);
                }
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}
