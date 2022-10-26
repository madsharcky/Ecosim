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
        private Random random = new Random();

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
            return name + " farmers:" + farmers + " scientists:" + scientists + " miners:" + miners + " money:" + money + " science:" + science + " food:" + food + " Docked Ships:" + ships.Count + " steel:"+ steel;
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
            steel += miners;
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
            if (food < 0)
            {
                int nrOfStarvingPeople = Math.Abs(food);
                food = 0;

                Console.WriteLine("People are starving on "+ name + "!! "  +killRandomPeople(nrOfStarvingPeople) + ".");
            }
            if (food > 10)
            {
                Console.WriteLine("A new " + createRandomPerson() + " has been born on "+ name +"!");
                food -= 10;
                
            }
        }
    
        private void buildShip()
        {
            ships.Add(new Ship(this));
            steel = steel - 100;

        }
        private String createRandomPerson()
        {
            switch (random.Next(1, 4))
            {
                case 1:
                    farmers++;
                    return "farmer";
                case 2:
                    scientists++;
                    return "scientist";
                case 3:
                    miners++;
                    return "miner";
                default:
                    return "bug";
            }
        }
        private String killRandomPeople(int amount)
        {
            int farmerDeath = 0;
            int scientistDeath = 0;
            int minerDeath = 0;
            
            for (int i = 0; i <= amount; i++)
            {
                switch (random.Next(1, 4))
                {
                    case 1:
                        if (farmers > 0)
                        {
                            farmers--;
                            farmerDeath++;
                        }
                        break;
                    case 2:
                        if (scientists > 0)
                        {
                            scientists--;
                            scientistDeath++;
                        }
                        break;
                    case 3:
                        if (miners > 0)
                        {
                            miners--;
                            minerDeath++;
                        }
                        break;
                }
            }
            return farmerDeath + " farmers, " + scientistDeath + " scientists and " + minerDeath + " miners have died";
        }

    }
}
