using CarMission.Client.Common.Messages;
using CitizenFX.Core;
using System;
using static CitizenFX.Core.Native.API;

namespace CarMission.Client.Admin.AdminCommands
{
    public class GodMode
    {
        private static bool Enabled = false;

        public static void Init()
        {
            RegisterCommand("god", new Action(ActivateGodMode), false);
        }

        public static void ActivateGodMode()
        {
            Enabled = !Enabled;

            try
            {
                var playerPed = PlayerId();

                if (Enabled)
                {
                    Game.PlayerPed.CanRagdoll = false;
                    SetPlayerInvincible(playerPed, true);
                    SetPedDiesWhenInjured(playerPed, false);

                    MessagesService.Notify(MessagesResource.MSG_GODMODE_ENABLED);
                }
                else
                {
                    Game.PlayerPed.CanRagdoll = true;
                    SetPlayerInvincible(playerPed, false);
                    SetPedDiesWhenInjured(playerPed, true);

                    MessagesService.Notify(MessagesResource.MSG_GODMODE_DISABLED);
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        public static void UpdateGodState()
        {
            try 
            {
                if (Enabled)
                {
                    Enabled = false;
                }
            }
            catch(Exception e)
            {
                Debug.Write(e.Message);
            }
        }
    }
}
