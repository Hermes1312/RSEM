using RSEM.Managers;
using RSEM.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSEM.Features
{
    internal class NoFlash
    {
        public static void Run()
        {
            while (true)
            {

                if (Settings.NoFlash.Enabled)
                {
                    if (MemoryManager.ReadMemory<float>(Structs.LocalPlayer.Base + Offsets.m_flFlashMaxAlpha) != Settings.NoFlash.Value)
                        MemoryManager.WriteMemory<float>(Structs.LocalPlayer.Base + Offsets.m_flFlashMaxAlpha, Settings.NoFlash.Value);
                }
                else
                    MemoryManager.WriteMemory<float>(Structs.LocalPlayer.Base + Offsets.m_flFlashMaxAlpha, 255);
            }
        }
    }
}

