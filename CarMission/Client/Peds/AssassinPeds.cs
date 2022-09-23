using CitizenFX.Core;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace CarMission.Client.Peds
{
    public class AssassinPeds
    {
        private static List<int> PedsId = new List<int>();

        public static void Init()
        {
        }

        public static async void CreateAssassinPed(float X, float Y, float Z)
        {
            try
            {
                uint groupHash = 0;
                var relationHash = AddRelationshipGroup("HATES_PLAYER", ref groupHash);

                var modelName = "s_m_y_blackops_01";

                var pedModel = new Model(modelName);
                var pedModelHash = (uint)pedModel.Hash;

                var weaponHash = (uint)WeaponHash.CarbineRifle;

                var playerPed = PlayerPedId();

                var vcoords = GetEntityCoords(playerPed, true);

                RequestModel(pedModelHash);

                while (!HasModelLoaded(pedModelHash))
                {
                    await BaseScript.Delay(1);
                }

                var ped = CreatePed(4, pedModelHash, X, Y, Z + 0.0f, 0.0f, true, true);

                PedsId.Add(ped);

                SetPedSeeingRange(ped, 10000.0f);
                SetPedHearingRange(ped, 10000.0f);

                GiveWeaponToPed(ped, weaponHash, 2800, false, true);
                SetPedInfiniteAmmo(ped, true, weaponHash);
                SetPedAlertness(ped, 3);
                ResetAiWeaponDamageModifier();
                AddArmourToPed(ped, 100);
                SetPedArmour(ped, 100);
                SetAiWeaponDamageModifier(1.0f);

                SetModelAsNoLongerNeeded(pedModelHash);
                SetEntityAsMissionEntity(ped, true, true);
                SetPedAlertness(ped, 3);

                SetRelationshipBetweenGroups(5, (uint)GetHashKey("HATES_PLAYER"), (uint)GetHashKey("PLAYER"));
                SetPedRelationshipGroupHash(ped, (uint)GetHashKey("HATES_PLAYER"));

                SetPedAccuracy(ped, 80);
                SetPedFleeAttributes(ped, 0, true);
                SetPedCombatAttributes(ped, 5, true);
                SetPedCombatAttributes(ped, 16, true);
                SetPedCombatAttributes(ped, 46, true);
                SetPedCombatAttributes(ped, 26, true);
                SetPedCombatAttributes(ped, 2, true);
                SetPedCombatAttributes(ped, 1, true);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        public static void ClearPeds()
        {
            foreach(var pedId in PedsId)
            {
                var ped = pedId;
                DeleteEntity(ref ped);
            }
        }
    }
}
