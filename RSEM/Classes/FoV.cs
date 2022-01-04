using RSEM.Managers;
using RSEM.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSEM.Features
{
    internal class FoV
    {
        public static void Run()
        {
            while (true)
            {
                bool isScoped = MemoryManager.ReadMemory<bool>(Structs.LocalPlayer.Base + Offsets.m_bIsScoped);

                if (Settings.FoV.Enabled)
                {
                    if(!isScoped)
                        if (MemoryManager.ReadMemory<int>(Structs.LocalPlayer.Base + Offsets.m_iDefaultFOV) != Settings.FoV.Value)
                            MemoryManager.WriteMemory<int>(Structs.LocalPlayer.Base + Offsets.m_iDefaultFOV, Settings.FoV.Value);
                }
                else
                    MemoryManager.WriteMemory<int>(Structs.LocalPlayer.Base + Offsets.m_iDefaultFOV, 90);
            }
        }
    }
}
