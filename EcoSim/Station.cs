using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
    class Station
    {
        private String name;
        private int farmers;
        private int scientists;
        private int miners;
        private int steel;
        private int food;
        private int science;
        private int threshhold;
        private float money;
        private List<Ship> ships;
        private Dictionary<String, int> location;

        public int Farmers { get => farmers; set => farmers = value; }
        public int Scientists { get => scientists; set => scientists = value; }
        public int Miners { get => miners; set => miners = value; }
        public string Name { get => name; set => name = value; }

        public Station(String name)
        {
            this.name = name;
            threshhold = 100;
            farmers = 0;
            scientists = 0;
            miners = 0;
            steel = 0;
            food = 0;
            science = 1000;
            money = 0;
            ships = new List<Ship>();
            location = new Dictionary<string, int>() { {"x",0},{ "y", 0 },{ "z", 0 } };
        }
        public String getStats()
        {
            return name + " farmers:" + farmers + " scientists:" + scientists + " miners:" + miners + " money:" + money + "science:" + science;
        }

        public bool setLocation(String axis, int value)
        {
            if (location.ContainsKey(axis))
            {
            location[axis] = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        public int getLocation(String axis)
        {
            return location[axis];
        }

        public void nextRound()
        {
            food += farmers * 2;
            food -= (farmers + scientists + miners);
            if (science > scientists)
            {
                money += scientists;
                science -= scientists;
            }
            else
            {
                money += science;
                science = 0;
            }
            if (steel > threshhold)
            {
                buildShip();
            }
        }
    
        private void buildShip()
        {
            ships.Append(new Ship(this));

        }

    }
}
