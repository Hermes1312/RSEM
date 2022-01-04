using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;

using RSEM.Other;
using RSEM.Managers;

using Math = RSEM.Other.Math;
using System.Diagnostics;

namespace RSEM.Features
{
    internal class Aimbot
    {
        // TODO LIST:
        // -Visibility Check (Not that important)
        // -Weapon Check

        public static void Run()
        {
            while (true)
            {
                Offsets ofs = new Offsets();

                Thread.Sleep(Settings.Aimbot.Smooth == 0f ? 1 : 45);

                if (!Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(System.Windows.Forms.Keys.LButton) & 0x8000) 
                    || Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.Trigger.Key) & 0x8000)
                    || !Checks.IsIngame
                    || !Structs.LocalPlayer.Health.IsAlive()) continue;

                int maxPlayers = Structs.ClientState.MaxPlayers;

                byte[] entities = MemoryManager.ReadMemory((int)Structs.Base.Client + Offsets.dwEntityList, maxPlayers * 0x10);

                int weapon = MemoryManager.ReadMemory<int>(Structs.LocalPlayer.Base + Offsets.m_hActiveWeapon);
                int weaponEnt = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwEntityList + ((weapon & 0xFFF) - 1) * 0x10); //here was an error
                int weaponID = MemoryManager.ReadMemory<int>(weaponEnt + Offsets.m_iItemDefinitionIndex);

                string eWeapon = Enum.GetName(typeof(Enums.Weapons), weaponID);

                if (IsAimbotWeapon(eWeapon))
                {

                    Dictionary<float, Vector3> possibleTargets = new Dictionary<float, Vector3> { };
                    Dictionary<float, Vector3> nearestTargets = new Dictionary<float, Vector3> { };



                    for (int i = 0; i < maxPlayers; i++)
                    {
                        int cEntity = Math.GetInt(entities, i * 0x10);

                        bool spotted = MemoryManager.ReadMemory<bool>(cEntity + Offsets.m_bSpottedByMask);

                        Structs.Enemy_t entityStruct = MemoryManager.ReadMemory<Structs.Enemy_t>(cEntity);

                        if (!entityStruct.Team.HasTeam()
                            || entityStruct.Team.IsMyTeam()
                            || !entityStruct.Health.IsAlive()
                            || entityStruct.Dormant) continue;

                        Vector3 bonePosition = Extensions.Other.GetBonePos(cEntity, Settings.Aimbot.Bone);

                        if (bonePosition == Vector3.Zero) continue;

                        Vector3 destination = Settings.Aimbot.RecoilControl
                            ? Math.CalcAngle(Structs.LocalPlayer.Position, bonePosition, Structs.LocalPlayer.AimPunch, Structs.LocalPlayer.VecView, Settings.Aimbot.YawRecoilReductionFactory, Settings.Aimbot.PitchRecoilReductionFactory)
                            : Math.CalcAngle(Structs.LocalPlayer.Position, bonePosition, Structs.LocalPlayer.AimPunch, Structs.LocalPlayer.VecView, 0f, 0f);

                        if (destination == Vector3.Zero) continue;

                        float distance = Math.GetDistance3D(destination, Structs.ClientState.ViewAngles);

                        Debug.WriteLine(DateTime.Now.ToString() + " | " + distance.ToString());

                        float nDistance = Math.GetDistance3D(bonePosition, Structs.LocalPlayer.Position);


                        if (Settings.Aimbot.OnylVisible)
                        {
                            if (spotted)
                            {
                                if (Settings.Aimbot.FovEnabled && (distance <= Settings.Aimbot.Fov))
                                {
                                    possibleTargets.Add(distance, destination);
                                    nearestTargets.Add(nDistance, destination);
                                }

                                else if (!Settings.Aimbot.FovEnabled)
                                {
                                    possibleTargets.Add(distance, destination);
                                    nearestTargets.Add(nDistance, destination);
                                }
                            }
                        }
                        else
                        {
                            if (Settings.Aimbot.FovEnabled && (distance <= Settings.Aimbot.Fov))
                            {
                                possibleTargets.Add(distance, destination);
                                nearestTargets.Add(nDistance, destination);
                            }

                            else if (!Settings.Aimbot.FovEnabled)
                            {
                                possibleTargets.Add(distance, destination);
                                nearestTargets.Add(nDistance, destination);
                            }
                        }
                    }

                    if (!possibleTargets.Any()) continue;

                    Vector3 aimAngle;

                    if (Settings.Aimbot.Nearest)
                        aimAngle = nearestTargets.OrderByDescending(x => x.Key).LastOrDefault().Value;
                    else
                        aimAngle = possibleTargets.OrderByDescending(x => x.Key).LastOrDefault().Value;



                    if (Settings.Aimbot.Curve)
                    {
                        Vector3 qDelta = aimAngle - Structs.ClientState.ViewAngles;
                        qDelta += new Vector3(qDelta.Y / Settings.Aimbot.CurveY, qDelta.X / Settings.Aimbot.CurveX, qDelta.Z);

                        aimAngle = Structs.ClientState.ViewAngles + qDelta;
                    }

                    aimAngle = Math.NormalizeAngle(aimAngle);
                    aimAngle = Math.ClampAngle(aimAngle);


                    MemoryManager.WriteMemory<Vector3>(Structs.ClientState.Base + Offsets.dwClientState_ViewAngles, Settings.Aimbot.Smooth == 0f
                        ? aimAngle
                        : Math.SmoothAim(Structs.ClientState.ViewAngles, aimAngle, Settings.Aimbot.Smooth));
                }
            }
        }
    
        public static Vector3 Nearest(Dictionary<float, Vector3> targets)
        {
            List<float> dists = new List<float>();

            Dictionary<float, Vector3> dTargets = new Dictionary<float, Vector3>();

            foreach (KeyValuePair<float, Vector3> entry in targets)
                dTargets.Add(Math.GetDistance3D(entry.Value, Structs.LocalPlayer.Position), entry.Value);

            return dTargets.OrderByDescending(x => x.Key).LastOrDefault().Value;
        }

        public static bool IsAimbotWeapon(string weapon)
        {
            string[] weapons = Settings.Aimbot.Weapons.Split(',');

            foreach (string fWeapon in weapons)
                if (weapon == fWeapon)
                    return true;

            return false;
        }
    }
}