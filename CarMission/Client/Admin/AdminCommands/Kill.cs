using CitizenFX.Core;
using System;
using static CitizenFX.Core.Native.API;

namespace CarMission.Client.Admin.AdminCommands
{
    public class Kill
    {
        public static void Init()
        {
            RegisterCommand("kill", new Action(KillPlayer), false);
        }

        public static void KillPlayer()
        {
            try
            {
                Game.PlayerPed.Kill();
                GodMode.UpdateGodState();
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }
    }
}
