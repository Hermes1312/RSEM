using System.Drawing;
using System.Threading;

using RSEM.Other;
using RSEM.Managers;

namespace RSEM.Features
{
    internal class Glow
    {
        public static void Run()
        {
            while (true) 
            {
                Thread.Sleep(10);

                if (!Checks.IsIngame) continue;

                int gObject = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwGlowObjectManager);
                int gCount = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwGlowObjectManager + 0x4);
                
                byte[] gEntities = MemoryManager.ReadMemory(gObject, gCount * 0x38);

                for (int i = 0; i < gCount; i++) 
                {
                    int gEntity = Math.GetInt(gEntities, i * 0x38);
                    if (gEntity == 0) continue;

                    int classID = Extensions.Other.GetClassID(gEntity);
                    if (classID < 0) continue;

                    bool defusing = MemoryManager.ReadMemory<bool>(gEntity + Offsets.m_bIsDefusing);
                    Structs.Enemy_t glowEntity = MemoryManager.ReadMemory<Structs.Enemy_t>(gEntity);

                    if (!glowEntity.Health.IsAlive()
                        || glowEntity.Dormant
                        || !glowEntity.Team.HasTeam()
                        || glowEntity.Dormant) continue;

                    if (Settings.Glow.Enemies && !glowEntity.Team.IsMyTeam())
                    {
                        if (Settings.Glow.PlayerColorMode == 0)
                        {
                            Structs.Glow_t currGlowObject = MemoryManager.ReadMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4));

                            Color color = Extensions.Colors.FromHealth(glowEntity.Health / 100f);

                            currGlowObject.r = color.R / 255f;
                            currGlowObject.g = color.G / 255f;
                            currGlowObject.b = color.B / 255f;
                            currGlowObject.a = Settings.Glow.Enemies_Color_A / 255f;
                            currGlowObject.m_bRenderWhenOccluded = true;
                            currGlowObject.m_bRenderWhenUnoccluded = false;

                            if (Settings.Glow.FullBloom) currGlowObject.m_bFullBloom = true;

                            MemoryManager.WriteMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4), currGlowObject);
                        }
                        else if (Settings.Glow.PlayerColorMode == 1)
                        {
                            Structs.Glow_t currGlowObject = MemoryManager.ReadMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4));

                            if (defusing)
                            {
                                currGlowObject.r = Settings.Glow.Spotted_Color_R / 255f;
                                currGlowObject.g = Settings.Glow.Spotted_Color_G / 255f;
                                currGlowObject.b = Settings.Glow.Spotted_Color_B / 255f;
                                currGlowObject.a = Settings.Glow.Spotted_Color_A / 255f;
                            }

                            else
                            {
                                currGlowObject.r = Settings.Glow.Enemies_Color_R / 255f;
                                currGlowObject.g = Settings.Glow.Enemies_Color_G / 255f;
                                currGlowObject.b = Settings.Glow.Enemies_Color_B / 255f;
                                currGlowObject.a = Settings.Glow.Enemies_Color_A / 255f;
                            }

                            currGlowObject.m_bRenderWhenOccluded = true;
                            currGlowObject.m_bRenderWhenUnoccluded = false;

                            if (Settings.Glow.FullBloom) currGlowObject.m_bFullBloom = true;

                            MemoryManager.WriteMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4), currGlowObject);
                        }
                    }

                    if (Settings.Glow.Allies && glowEntity.Team.IsMyTeam())
                    {
                        if (Settings.Glow.PlayerColorMode == 0)
                        {
                            Structs.Glow_t currGlowObject = MemoryManager.ReadMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4));

                            Color color = Extensions.Colors.FromHealth(glowEntity.Health / 100f);

                            currGlowObject.r = color.R / 255f;
                            currGlowObject.g = color.G / 255f;
                            currGlowObject.b = color.B / 255f;
                            currGlowObject.a = Settings.Glow.Allies_Color_A / 255f;
                            currGlowObject.m_bRenderWhenOccluded = true;
                            currGlowObject.m_bRenderWhenUnoccluded = false;

                            if (Settings.Glow.FullBloom) currGlowObject.m_bFullBloom = true;

                            MemoryManager.WriteMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4), currGlowObject);
                        }
                        else if (Settings.Glow.PlayerColorMode == 1)
                        {
                            Structs.Glow_t currGlowObject = MemoryManager.ReadMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4));

                            currGlowObject.r = Settings.Glow.Allies_Color_R / 255f;
                            currGlowObject.g = Settings.Glow.Allies_Color_G / 255f;
                            currGlowObject.b = Settings.Glow.Allies_Color_B / 255f;
                            currGlowObject.a = Settings.Glow.Allies_Color_A / 255f;
                            currGlowObject.m_bRenderWhenOccluded = true;
                            currGlowObject.m_bRenderWhenUnoccluded = false;

                            if (Settings.Glow.FullBloom) currGlowObject.m_bFullBloom = true;

                            MemoryManager.WriteMemory<Structs.Glow_t>((gObject + (i * 0x38) + 0x4), currGlowObject);
                        }
                    }
                }
            }
        }
    }
}
