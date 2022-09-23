using CarMission.Client.Client;
using CarMission.Client.Common.Messages;
using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CarMission.Client.Peds;
using static CitizenFX.Core.Native.API;

namespace CarMission.Client.Missions.CarDeliveryMission
{
    public class CarDelivery
    {
        private static bool AlreadyCreatedVehicle = false;
        private static int AssassinPedsCount = 0;
        private static int PlayerChance = 0;
        private static bool MissionEnabled = false;
        private static Blip CarBlip = null;
        private static bool CanCheckChance = true;

        private static Vector3 initialCoords = new Vector3()
        {
            X = 101.9444f,
            Y = -1937.925f,
            Z = 20.37975f
        };

        private static Vector3 finalCoords = new Vector3()
        {
            X = 172.5095f,
            Y = -1708.211f,
            Z = 28.86889f
        };

        private static Vector3 carSpawningCoords = new Vector3()
        {
            X = 90.13769f,
            Y = -1966.31f,
            Z = 20.7475f
        };

        private static List<DumbPedCoordinates> PedCoordinates = new List<DumbPedCoordinates>()
        {
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
            new DumbPedCoordinates(172.5095f, -1708.211f, 28.86889f),
        };

        public static void Init()
        {
            ClientMain.GetInstance().RegisterTickHandler(InitiateMission);
            CreateMissionBlip();
        }

        public static void CreateMissionBlip()
        {
            //Creating mission blip just for locate

            var missionBlip = World.CreateBlip(initialCoords);
            missionBlip.Sprite = BlipSprite.Garage;
            missionBlip.Name = "Car Delivery Mission";
            missionBlip.RemoveNumberLabel();

            missionBlip.Scale = 0.7f;
        }

        private static async Task InitiateMission()
        {
            try
            {
                var playerPed = GetPlayerPed(PlayerId());

                var vehicle = 0;

                var playerCoords = Game.PlayerPed.Position;

                var beginDistance = World.GetDistance(playerCoords, initialCoords);

                var endDistance = World.GetDistance(playerCoords, finalCoords);

                if (beginDistance > 50 && !CanCheckChance)
                {
                    CanCheckChance = true;
                }

                if (beginDistance >= 10 && beginDistance <= 50 && !MissionEnabled)
                {
                    if (CanCheckChance)
                    {
                        CalculatePlayerChance();
                    }

                    if (PlayerChance <= 20)
                    {
                        MissionEnabled = !MissionEnabled;

                        AlreadyCreatedVehicle = !AlreadyCreatedVehicle;

                        var vehicleModel = new Model(VehicleHash.T20);

                        await World.CreateVehicle(vehicleModel, carSpawningCoords);

                        CarBlip = World.CreateBlip(carSpawningCoords);

                        MessagesService.Notify(MessagesResource.MSG_CARDELIVERYMISSION_GETCAR);
                    }
                    else
                    {
                        MessagesService.Notify(MessagesResource.MSG_CARDELIVERYMISSION_FAILED);
                    }
                }

                if (MissionEnabled)
                {
                    if (Game.PlayerPed.IsInVehicle() && ((VehicleHash)Game.PlayerPed.CurrentVehicle.Model).ToString() == VehicleHash.T20.ToString())
                    {
                        vehicle = GetVehiclePedIsIn(playerPed, false);

                        CarBlip.Delete();

                        World.DrawMarker(
                                MarkerType.CarSymbol,
                                finalCoords,
                                new Vector3(0.0f, 0.0f, 0.0f),
                                new Vector3(0.0f, 0.0f, 0.0f),
                                new Vector3(4.0f, 4.0f, 4.0f),
                                Color.FromArgb(255, 0, 0),
                                rotateY: true
                                );

                        SetNewWaypoint(finalCoords.X, finalCoords.Y);

                        MessagesService.Notify(MessagesResource.MSG_CARDELIVERYMISSION_DELIVERYCAR);

                        if (endDistance <= 5)
                        {
                            SetWaypointOff();
                            SetPoliceIgnorePlayer(PlayerId(), true);

                            BringVehicleToHalt(vehicle, 0.1f, 1, true);

                            SetVehicleUndriveable(vehicle, true);
                            SetVehicleEngineOn(vehicle, false, true, true);
                            SetVehicleDoorsLocked(vehicle, 2);
                            TaskLeaveVehicle(playerPed, vehicle, 0);
                            SetEntityAsMissionEntity(vehicle, true, true);
                            SetVehicleAsNoLongerNeeded(ref vehicle);

                            Game.PlayerPed.Weapons.Give(WeaponHash.CarbineRifle, 999, true, true);
                            Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
                            Game.PlayerPed.Armor = 100;
                        }
                    }

                    if (endDistance <= 5)
                    {
                        if (!Game.PlayerPed.IsInVehicle() && AssassinPedsCount < 10)
                        {
                            foreach (var coord in PedCoordinates)
                            {
                                AssassinPeds.CreateAssassinPed(coord.posX, coord.posY, coord.posZ);
                                AssassinPedsCount++;
                            }
                        }
                    }

                    if (Game.PlayerPed.IsDead)
                    {
                        await BaseScript.Delay(2000);

                        AssassinPeds.ClearPeds();
                        UpdateAttributes();
                        CarBlip.Delete();

                        if (vehicle > 0)
                        {
                            DeleteVehicle(ref vehicle);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        private static void UpdateAttributes()
        {
            AlreadyCreatedVehicle = false;
            AssassinPedsCount = 0;
            MissionEnabled = false;
            CanCheckChance = true;
        }

        private static void CalculatePlayerChance()
        {
            var random = new Random();

            CanCheckChance = false;

            PlayerChance = random.Next(0, 100);
        }
    }
}
