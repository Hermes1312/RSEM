using System.Collections.Generic;
using vKeys = System.Windows.Forms.Keys;

namespace RSEM.Other
{
    internal class Settings
    {
        public struct Keys
        {
            public static vKeys Bunnyhop =  vKeys.F6;
            public static vKeys Trigger   = vKeys.F7;
            public static vKeys Glow      = vKeys.F8;
            public static vKeys Radar     = vKeys.F9;
            public static vKeys Aimbot    = vKeys.F10;
            public static vKeys Chams     = vKeys.F11;
        }

        public struct Radar
        {
            public static bool Enabled = true;
        }

        public struct Bunnyhop
        {
            public static bool Enabled = true;
            public static int Key = Keyboard.VK_SPACE;
        }

        public struct Trigger
        {
            public static bool Enabled = true;
            public static vKeys Key = vKeys.Menu;
            public static string Weapons = "deagle,berettas,fiveseve,glock,ak47,aug,awp,famas,g35g1,galil,m249,m4a4,mac10,p90,mp5sd,ump,xm1014,ppbizon,mag7,negev,sawedoff,tec9,zeus,p2000,mp7,mp9,nova,p250,scar,sg553,ssg,m4a1s,usp,cz75,revolver";
            public static int Delay = 0;
        }

        public struct FoV
        {
            public static bool Enabled = true;
            public static int Value = 120;
        }
        
        public struct NoFlash
        {
            public static bool Enabled = true;
            public static float Value = 150f;
        }

        public struct Chams
        {
            public static bool Enabled = true;

            public static bool Enemies = true;
            public static float Enemies_Color_R = Glow.Enemies_Color_R;
            public static float Enemies_Color_G = Glow.Enemies_Color_G;
            public static float Enemies_Color_B = Glow.Enemies_Color_B;
            public static float Enemies_Color_A = 255f;

            public static bool Allies = true;
            public static float Allies_Color_R = Glow.Allies_Color_R;
            public static float Allies_Color_G = Glow.Allies_Color_G;
            public static float Allies_Color_B = Glow.Allies_Color_B;
            public static float Allies_Color_A = 255f;
        }

        public struct Glow
        {
            public static bool Enabled = true;
            public static int PlayerColorMode = 1;
            public static bool FullBloom = true;

            public static bool Enemies = true;
            public static float Enemies_Color_R = 192;
            public static float Enemies_Color_G = 57;
            public static float Enemies_Color_B = 43;
            public static float Enemies_Color_A = 255;

            public static bool Allies = true;
            public static float Allies_Color_R = 39;
            public static float Allies_Color_G = 174;
            public static float Allies_Color_B = 96;
            public static float Allies_Color_A = 255;

            public static bool Spotted = true;
            public static float Spotted_Color_R = 39;
            public static float Spotted_Color_G = 174;
            public static float Spotted_Color_B = 96;
            public static float Spotted_Color_A = 255;
        }

        public struct AutoPistol
        {
            public static bool Enabled = true;
            public static vKeys Key = vKeys.CapsLock;
        }

        public struct Aimbot
        {
            //3 ass
            //5 troso
            //6 uppertorso
            //7 neck
            public static string Weapons = "deagle,berettas,fiveseve,glock,ak47,aug,awp,famas,g35g1,galil,m249,m4a4,mac10,p90,mp5sd,ump,xm1014,ppbizon,mag7,negev,sawedoff,tec9,zeus,p2000,mp7,mp9,nova,p250,scar,sg553,ssg,m4a1s,usp,cz75,revolver";
            
            public static bool Enabled = true;
            public static bool Nearest = true;
            public static bool OnylVisible = true;
            public static bool Aimlock = true;
            public static bool FovEnabled = true;
            public static float Fov = 60f;
            public static int Bone = 8;
            public static float Smooth = 0f;
            public static bool RecoilControl = true;
            public static float RCSValue = 2f;
            public static float YawRecoilReductionFactory = RCSValue;
            public static float PitchRecoilReductionFactory = RCSValue;
            public static bool Curve = true;
            public static float CurveY = 5f;
            public static float CurveX = 5f;
        }
    }
}
