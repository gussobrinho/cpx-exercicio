using CarMission.Client.Common.Messages;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using Enum = System.Enum;

namespace CarMission.Client.Weapons
{
    public class WeaponService
    {
        public static void Init()
        {
            API.RegisterCommand("weapon", new Action<int, List<object>, string>(GiveWeaponToPlayer), false);
        }

        public static void GiveWeaponToPlayer(int src, List<object> args, string raw)
        {
            try
            {
                var argList = args.Select(o => o.ToString()).ToList();

                var weaponName = argList[0];

                if (weaponName.ToLower() == "all")
                {
                    GiveAllWeaponsToPlayer();
                }
                else
                {
                    Enum.TryParse(weaponName, true, out WeaponHash weapon);

                    if (weapon != 0)
                    {
                        Game.PlayerPed.Weapons.Give(weapon, 999, false, true);

                        MessagesService.Notify(string.Format(MessagesResource.MSG_WEAPON_SUCCESS, weapon));
                    }
                    else
                    {
                        MessagesService.Notify(string.Format(MessagesResource.MSG_WEAPON_UNSUCCESSFULLY, weapon));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        public static void GiveAllWeaponsToPlayer()
        {
            try
            {
                foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
                {
                    Game.PlayerPed.Weapons.Give(weapon, 999, false, true);
                }

                MessagesService.Notify(MessagesResource.MSG_ALLWEAPONS_SUCCESS);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

        }
    }
}
