using CarMission.Client.Admin.AdminCommands;
using CarMission.Client.Missions.CarDeliveryMission;
using CarMission.Client.Peds;
using CarMission.Client.Vehicles.WeaponService;
using CarMission.Client.Weapons;

namespace CarMission.Client.Common
{
    public class ClassLoader
    {
        public static void Init()
        {
            // Vehicles
            VehicleServices.Init();

            // Weapons
            WeaponService.Init();

            // Peds
            AssassinPeds.Init();

            // Admin
            GodMode.Init();
            TeleportAndCoordinates.Init();
            Kill.Init();

            // Missions
            CarDelivery.Init();
        }
    }
}
