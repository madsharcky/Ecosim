using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
    [Serializable]
    class Ship
    {
        private int speed;
        private Dictionary<String, int> cargo;
        private int distanceToTarget;
        private Station targetStation;
        private Station homeStation;

        public Ship(Station homeStation)
        {
            speed = 1;
            cargo = new Dictionary<string, int>();
            distanceToTarget = 0;
            this.homeStation = homeStation;
            targetStation = homeStation;
        }

        public int DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
        internal Station TargetStation { get => targetStation; set => targetStation = value; }
        
        
        public bool moveForward() // returns true if ship has arrived and false if not
        {
            if (distanceToTarget < speed)
            {
                distanceToTarget = 0;
                return true;
            }
            else
            {
            distanceToTarget -= speed;
                return false;
            }
            
        }
        public bool isHome()
        {
            if (targetStation == homeStation && distanceToTarget == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void setTarget(Station targetStation)
        {
            this.targetStation = targetStation;            
        }
    }

}
