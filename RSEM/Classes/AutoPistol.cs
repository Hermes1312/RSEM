using RSEM.Managers;
using RSEM.Other;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSEM.Features
{
    internal class AutoPistol
    {
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;

        public static void Run()
        {
            IntPtr csgo = Process.GetProcessesByName("csgo")[0].MainWindowHandle;

            while (true)
            {
                if(Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.AutoPistol.Key) & 0x8000) && IsUsingPistol())
                {



                    PostMessage(csgo, WM_LBUTTONUP, 0, 0);
                    //Globals.Imports.mouse_event(Mouse.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    Thread.Sleep(15);
                    PostMessage(csgo, WM_LBUTTONDOWN, 1, 0);
                    //Globals.Imports.mouse_event(Mouse.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

                    Debug.WriteLine(DateTime.Now.ToString() + " | Shoot1");

                }
            }
        }

        private static bool IsUsingPistol()
        {
            int     weapon      = MemoryManager.ReadMemory<int>(Structs.LocalPlayer.Base + Offsets.m_hActiveWeapon);
            int     weaponEnt   = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwEntityList + ((weapon & 0xFFF) - 1) * 0x10); //here was an error
            int     weaponID    = MemoryManager.ReadMemory<int>(weaponEnt + Offsets.m_iItemDefinitionIndex);
            string  eWeapon     = Enum.GetName(typeof(Enums.Weapons), weaponID),
                    pistols     = "deagle,berettas,fiveseven,glock,tec9,p2000,p250,usp,cz75";

            string[] aPistols = pistols.Split(',');


            foreach (string pistol in aPistols)
                if (eWeapon == pistol)
                    return true;

            return false;
        }
    }
}
