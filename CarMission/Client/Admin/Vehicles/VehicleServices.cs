using CarMission.Client.Common;
using CarMission.Client.Common.Messages;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMission.Client.Vehicles.WeaponService
{
    public class VehicleServices
    {
        private static string VehicleSpawnerTitle = "VehicleSpawner";
        public static void Init()
        {
            API.RegisterCommand("veh", new Action<int, List<object>, string>(VehicleSpawner), false);
        }

        public async static void VehicleSpawner(int src, List<object> args, string raw)
        {
            var argList = args.Select(o => o.ToString()).ToList();

            Enum.TryParse(argList[0], true, out VehicleHash vehicle);

            var model = new Model(vehicle);

            var generatedVehicle = await World.CreateVehicle(model, Game.PlayerPed.GetOffsetPosition(new Vector3(0f, 5f, 0f)), Game.PlayerPed.Heading);

            Game.PlayerPed.SetIntoVehicle(generatedVehicle, VehicleSeat.Driver);

            MessagesService.SendChatMessage(VehicleSpawnerTitle, $"You spawned a {vehicle}!", 0, 255, 0);
        }
    }
}
