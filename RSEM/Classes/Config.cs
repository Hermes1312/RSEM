using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using IniParser;
using IniParser.Model;

using RSEM.Other;
using System.Windows.Forms;

namespace RSEM.Managers
{
    internal class Config
    {
        public static bool IsFileUsable(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) 
            {
                return fileStream.CanWrite ? true : false;
            }
        }

        public static void Load(string path)
        {
            if (!IsFileUsable(path)) 
            {
                Extensions.Error("[Config][Load][Error] Config File is not usable", 0, false);
                return;
            }

            FileIniDataParser configParser = new FileIniDataParser();
            IniData configData = configParser.ReadFile(path);

                Settings.Radar.Enabled                      = bool.Parse(configData["Radar"]["Enabled"]);

                Settings.Bunnyhop.Enabled                   = bool.Parse(configData["Bunnyhop"]["Enabled"]);

                Settings.Trigger.Enabled                    = bool.Parse(configData["Trigger"]["Enabled"]);
                Settings.Trigger.Key                        = (Keys)Convert.ToInt32(configData["Trigger"]["Key"]);
                Settings.Trigger.Delay                      = int.Parse(configData["Trigger"]["Delay"]);

                Settings.Chams.Enabled                      = bool.Parse(configData["Chams"]["Enabled"]);

                

                Settings.Glow.Enabled                       = bool.Parse(configData["Glow"]["Enabled"]);
                Settings.Glow.PlayerColorMode               = int.Parse(configData["Glow"]["PlayerColorMode"]);
                Settings.Glow.FullBloom                     = bool.Parse(configData["Glow"]["FullBloom"]);

                Settings.Glow.Allies                        = bool.Parse(configData["Glow"]["Allies"]);
                Settings.Glow.Allies_Color_R                = float.Parse(configData["Glow"]["Allies_Color_R"]);
                Settings.Glow.Allies_Color_G                = float.Parse(configData["Glow"]["Allies_Color_G"]);
                Settings.Glow.Allies_Color_B                = float.Parse(configData["Glow"]["Allies_Color_B"]);
                Settings.Glow.Allies_Color_A                = float.Parse(configData["Glow"]["Allies_Color_A"]);

                Settings.Glow.Enemies                       = bool.Parse(configData["Glow"]["Enemies"]);
                Settings.Glow.Enemies_Color_R               = float.Parse(configData["Glow"]["Enemies_Color_R"]);
                Settings.Glow.Enemies_Color_G               = float.Parse(configData["Glow"]["Enemies_Color_G"]);
                Settings.Glow.Enemies_Color_B               = float.Parse(configData["Glow"]["Enemies_Color_B"]);
                Settings.Glow.Enemies_Color_A               = float.Parse(configData["Glow"]["Enemies_Color_A"]);


                Settings.Glow.Spotted = bool.Parse(configData["Glow"]["Spotted"]);
                Settings.Glow.Spotted_Color_R = float.Parse(configData["Glow"]["Spotted_Color_R"]);
                Settings.Glow.Spotted_Color_G = float.Parse(configData["Glow"]["Spotted_Color_G"]);
                Settings.Glow.Spotted_Color_B = float.Parse(configData["Glow"]["Spotted_Color_B"]);
                Settings.Glow.Spotted_Color_A = float.Parse(configData["Glow"]["Spotted_Color_A"]);


                Settings.Aimbot.Enabled                     = bool.Parse(configData["Aimbot"]["Enabled"]);
                Settings.Aimbot.OnylVisible                 = bool.Parse(configData["Aimbot"]["OnylVisible"]);
                Settings.Aimbot.Nearest                     = bool.Parse(configData["Aimbot"]["Nearest"]);
                Settings.Aimbot.Weapons                     = configData["Aimbot"]["FovEnabled"];
                Settings.Aimbot.FovEnabled                  = bool.Parse(configData["Aimbot"]["FovEnabled"]);
                Settings.Aimbot.Fov                         = float.Parse(configData["Aimbot"]["Fov"]);
                Settings.Aimbot.Bone                        = int.Parse(configData["Aimbot"]["Bone"]);
                Settings.Aimbot.Smooth                      = float.Parse(configData["Aimbot"]["Smooth"]);
                Settings.Aimbot.RecoilControl               = bool.Parse(configData["Aimbot"]["RecoilControl"]);
                Settings.Aimbot.RCSValue                    = float.Parse(configData["Aimbot"]["RCSValue"]);

                Settings.Keys.Bunnyhop                      = (Keys)Convert.ToInt32(configData["Keys"]["Bunnyhop"]);
                Settings.Keys.Trigger                       = (Keys)Convert.ToInt32(configData["Keys"]["Trigger"]);
                Settings.Keys.Glow                          = (Keys)Convert.ToInt32(configData["Keys"]["Glow"]);
                Settings.Keys.Radar                         = (Keys)Convert.ToInt32(configData["Keys"]["Radar"]);
                Settings.Keys.Aimbot                        = (Keys)Convert.ToInt32(configData["Keys"]["Aimbot"]);
                Settings.Keys.Chams                         = (Keys)Convert.ToInt32(configData["Keys"]["Chams"]);

                Settings.FoV.Enabled                        = bool.Parse(configData["FoV"]["Enabled"]);
                Settings.FoV.Value                          = int.Parse(configData["FoV"]["Value"]);
                
                Settings.NoFlash.Enabled                    = bool.Parse(configData["NoFlash"]["Enabled"]);
                Settings.NoFlash.Value                      = float.Parse(configData["NoFlash"]["Value"]);


                Settings.AutoPistol.Key                         = (Keys)Convert.ToInt32(configData["AutoPistol"]["Key"]);
                Settings.AutoPistol.Enabled                     = bool.Parse(configData["AutoPistol"]["Enabled"]);


            

            Extensions.Information("[Config][Load] Loaded", true);
        }

        public static void Save(string path)
        {
            if (File.Exists(path)) 
            {
                if (!IsFileUsable(path))
                {
                    Extensions.Error("[Config][Save][Error] Config File is not usable", 0, false);
                    return;
                }
            }

            FileIniDataParser configParser = new FileIniDataParser();
            IniData configData = new IniData();

            configData["Radar"]["Enabled"]                      = Settings.Radar.Enabled.ToString();

            configData["Bunnyhop"]["Enabled"]                   = Settings.Bunnyhop.Enabled.ToString();

            configData["Trigger"]["Enabled"]                    = Settings.Trigger.Enabled.ToString();
            configData["Trigger"]["Key"]                        = ((int)Settings.Trigger.Key).ToString();
            configData["Trigger"]["Delay"]                      = Settings.Trigger.Delay.ToString();

            configData["Chams"]["Enabled"]                      = Settings.Chams.Enabled.ToString();

            configData["Chams"]["Allies"]                       = Settings.Chams.Allies.ToString();
            configData["Chams"]["Allies_Color_R"]               = Settings.Chams.Allies_Color_R.ToString();
            configData["Chams"]["Allies_Color_G"]               = Settings.Chams.Allies_Color_G.ToString();
            configData["Chams"]["Allies_Color_B"]               = Settings.Chams.Allies_Color_B.ToString();
            configData["Chams"]["Allies_Color_A"]               = Settings.Chams.Allies_Color_A.ToString();

            configData["Chams"]["Enemies"]                      = Settings.Chams.Enemies.ToString();
            configData["Chams"]["Enemies_Color_R"]              = Settings.Chams.Enemies_Color_R.ToString();
            configData["Chams"]["Enemies_Color_G"]              = Settings.Chams.Enemies_Color_G.ToString();
            configData["Chams"]["Enemies_Color_B"]              = Settings.Chams.Enemies_Color_B.ToString();
            configData["Chams"]["Enemies_Color_A"]              = Settings.Chams.Enemies_Color_A.ToString();

            configData["Glow"]["Enabled"]                       = Settings.Glow.Enabled.ToString();
            configData["Glow"]["PlayerColorMode"]               = Settings.Glow.PlayerColorMode.ToString();
            configData["Glow"]["FullBloom"]                     = Settings.Glow.FullBloom.ToString();

            configData["Glow"]["Allies"]                        = Settings.Glow.Allies.ToString();
            configData["Glow"]["Allies_Color_R"]                = Settings.Glow.Allies_Color_R.ToString();
            configData["Glow"]["Allies_Color_G"]                = Settings.Glow.Allies_Color_G.ToString();
            configData["Glow"]["Allies_Color_B"]                = Settings.Glow.Allies_Color_B.ToString();
            configData["Glow"]["Allies_Color_A"]                = Settings.Glow.Allies_Color_A.ToString();

            configData["Glow"]["Enemies"]                       = Settings.Glow.Enemies.ToString();
            configData["Glow"]["Enemies_Color_R"]               = Settings.Glow.Enemies_Color_R.ToString();
            configData["Glow"]["Enemies_Color_G"]               = Settings.Glow.Enemies_Color_G.ToString();
            configData["Glow"]["Enemies_Color_B"]               = Settings.Glow.Enemies_Color_B.ToString();
            configData["Glow"]["Enemies_Color_A"]               = Settings.Glow.Enemies_Color_A.ToString();
            
            configData["Glow"]["Spotted"]                       = Settings.Glow.Spotted.ToString();
            configData["Glow"]["Spotted_Color_R"]               = Settings.Glow.Spotted_Color_R.ToString();
            configData["Glow"]["Spotted_Color_G"]               = Settings.Glow.Spotted_Color_G.ToString();
            configData["Glow"]["Spotted_Color_B"]               = Settings.Glow.Spotted_Color_B.ToString();
            configData["Glow"]["Spotted_Color_A"]               = Settings.Glow.Spotted_Color_A.ToString();

            

            configData["Aimbot"]["Enabled"]                     = Settings.Aimbot.Enabled.ToString();
            configData["Aimbot"]["OnylVisible"]                 = Settings.Aimbot.OnylVisible.ToString();
            configData["Aimbot"]["Nearest"]                     = Settings.Aimbot.Nearest.ToString();
            configData["Aimbot"]["Weapons"]                     = Settings.Aimbot.Weapons;
            configData["Aimbot"]["FovEnabled"]                  = Settings.Aimbot.FovEnabled.ToString();
            configData["Aimbot"]["Fov"]                         = Settings.Aimbot.Fov.ToString();
            configData["Aimbot"]["Bone"]                        = Settings.Aimbot.Bone.ToString();
            configData["Aimbot"]["Smooth"]                      = Settings.Aimbot.Smooth.ToString();
            configData["Aimbot"]["RecoilControl"]               = Settings.Aimbot.RecoilControl.ToString();
            configData["Aimbot"]["RCSValue"]                    = Settings.Aimbot.RCSValue.ToString();


            configData["Keys"]["Bunnyhop"]  = ((int)Settings.Keys.Bunnyhop).ToString();
            configData["Keys"]["Trigger"]   = ((int)Settings.Keys.Trigger).ToString();
            configData["Keys"]["Glow"]      = ((int)Settings.Keys.Glow).ToString();
            configData["Keys"]["Radar"]     = ((int)Settings.Keys.Radar).ToString();
            configData["Keys"]["Aimbot"]    = ((int)Settings.Keys.Aimbot).ToString();
            configData["Keys"]["Chams"]     = ((int)Settings.Keys.Chams).ToString();

            configData["FoV"]["Enabled"]        = Settings.FoV.Enabled.ToString();
            configData["FoV"]["Value"]          = Settings.FoV.Value.ToString();

            configData["NoFlash"]["Enabled"]    = Settings.NoFlash.Enabled.ToString();
            configData["NoFlash"]["Value"]      = Settings.NoFlash.Value.ToString();

            configData["AutoPistol"]["Key"]      = ((int)Settings.AutoPistol.Key).ToString();
            configData["AutoPistol"]["Enabled"]      = Settings.AutoPistol.Enabled.ToString();



            configParser.WriteFile(path, configData);

            Extensions.Information("[Config][Save] Saved", true);
        }
    }
}
