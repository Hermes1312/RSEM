using System;
using System.Threading;
using RSEM.Managers;
using RSEM.Other;

namespace RSEM.Features
{
    internal class Trigger
    {
        public static void Run()
        {
            while (true) 
            {
                Thread.Sleep(1);

                if (!Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.Trigger.Key) & 0x8000) 
                    || !Checks.IsIngame 
                    || !Structs.LocalPlayer.Health.IsAlive()
                    || !Structs.Enemy_Crosshair.Health.IsAlive()
                    || !Structs.Enemy_Crosshair.Team.HasTeam()
                    || Structs.Enemy_Crosshair.Team.IsMyTeam()
                    || Structs.Enemy_Crosshair.Dormant) continue;

                int weapon = MemoryManager.ReadMemory<int>(Structs.LocalPlayer.Base + Offsets.m_hActiveWeapon);
                int weaponEnt = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwEntityList + ((weapon & 0xFFF) - 1) * 0x10);
                int weaponID = MemoryManager.ReadMemory<int>(weaponEnt + Offsets.m_iItemDefinitionIndex);

                string eWeapon = Enum.GetName(typeof(Enums.Weapons), weaponID);

                if (Settings.Trigger.Delay > 0 && IsTriggerWeapon(eWeapon)) Thread.Sleep(Settings.Trigger.Delay);

                Globals.Imports.mouse_event(Mouse.MOUSEEVENTF_LEFTDOWN | Mouse.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public static bool IsTriggerWeapon(string weapon)
        {
            string[] weapons = Settings.Trigger.Weapons.Split(',');

            foreach (string fWeapon in weapons)
                if (weapon == fWeapon)
                    return true;

            return false;
        }
    }
}
