using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using RSEM.Other;

namespace RSEM.Managers
{
    internal class Watcher
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public static void Run()
        {
            while (true)
            {
                Thread.Sleep(75);

                //if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.OtherControls.LoadConfig) & 0x8000)) Config.Load();
                //if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.OtherControls.SaveConfig) & 0x8000)) Config.Save();

                if(GetAsyncKeyState(Settings.Keys.Bunnyhop) < 0)  ThreadManager.ToggleThread("Bunnyhop");
                if(GetAsyncKeyState(Settings.Keys.Trigger)  < 0)  ThreadManager.ToggleThread("Trigger");
                if(GetAsyncKeyState(Settings.Keys.Glow) < 0)      ThreadManager.ToggleThread("Glow"); 
                if(GetAsyncKeyState(Settings.Keys.Radar)    < 0)  ThreadManager.ToggleThread("Radar");
                if(GetAsyncKeyState(Settings.Keys.Aimbot)   < 0)  ThreadManager.ToggleThread("Aimbot");
                if(GetAsyncKeyState(Settings.Keys.Chams)    < 0)  ThreadManager.ToggleThread("Chams");

            }
        }
    }
}
