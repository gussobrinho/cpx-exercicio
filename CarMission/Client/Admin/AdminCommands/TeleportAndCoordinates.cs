using CarMission.Client.Common.Messages;
using CitizenFX.Core;
using System;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace CarMission.Client.Admin.AdminCommands
{
    public class TeleportAndCoordinates
    {
        public static void Init()
        {
            RegisterCommand("tpway", new Action(TeleportToWaypoint), false);
        }

        public static void TeleportToWaypoint()
        {
            try
            {
                var player = PlayerId();
                var playerInVehicle = Game.PlayerPed.IsInVehicle();
                var tpWithVehicle = false;
                var groundFound = false;

                var blip = GetFirstBlipInfoId(8);

                if (DoesBlipExist(blip))
                {
                    var blipX = 0.0f;
                    var blipY = 0.0f;
                    var blipZ = 0.0f;

                    if (blip != 0)
                    {
                        var coord = GetBlipCoords(blip);
                        blipX = coord.X;
                        blipY = coord.Y;
                    }

                    var groundCheckHeight = new float[]
                    {
                        100.0f, 150.0f, 50.0f, 0.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f,
                        450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f
                    };

                    if (playerInVehicle)
                    {
                        tpWithVehicle = true;
                    }

                    for (int i = 0; i < groundCheckHeight.Count(); i++)
                    {
                        SetEntityCoordsNoOffset(player, blipX, blipY, groundCheckHeight[i], false, false, false);

                        if (GetGroundZFor_3dCoord(blipX, blipY, groundCheckHeight[i], ref blipZ, true))
                        {
                            groundFound = true;
                            blipZ += 3.0f;
                            break;
                        }
                    }

                    if (!groundFound)
                    {
                        blipZ = 1000.0f;
                    }

                    StartPlayerTeleport(player, blipX, blipY, blipZ, 0.0f, tpWithVehicle, true, true);
                }
                else
                {
                    MessagesService.Notify(MessagesResource.MSG_BLIPNOTFOUND);
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }
    }
}
