using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMission.Client.Missions.CarDeliveryMission
{
    public class DumbPedCoordinates
    {
        public float posX { get; }
        public float posY { get; }
        public float posZ { get; }

        public DumbPedCoordinates(float posX, float posY, float posZ)
        {
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;
        }
    }
}
