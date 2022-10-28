using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
    class Station
    {
        private String name;
        private float farmers;
        private float scientists;
        private float miners;
        private float steel;
        private float food;
        private float science;
        private float threshhold;
        private float money;
        private Dictionary<String, float> resources;
        private List<Ship> ships;
        private Dictionary<String, int> location;
        private Random random = new Random();
        private Dictionary<String, float> buying; //describes the need
        private Dictionary<String, float> selling; //describes the surplus

        public float Farmers { get => farmers; set => farmers = value; }
        public float Scientists { get => scientists; set => scientists = value; }
        public float Miners { get => miners; set => miners = value; }
        public string Name { get => name; set => name = value; }
        public void setResource(String resource, float value) { resources[resource] = value; }
        public float getResource(String resource) { return resources[resource]; }


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
            resources = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 1000 } };
            ships = new List<Ship>();
            location = new Dictionary<string, int>() { {"x",0},{ "y", 0 },{ "z", 0 } };
            buying = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 },{"science",0 } };
            selling = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 0 } };
            nextRound();
        }
        public String getStats()
        {
            String buyingString = "";
            for (int i = 0; i < buying.Count; i++)
            {
                buyingString += buying.ElementAt(i).Key + ": " + buying.ElementAt(i).Value + " ";
            }
            String sellingString = "";
            for (int i = 0; i < selling.Count; i++)
            {
                sellingString += selling.ElementAt(i).Key + ": " + selling.ElementAt(i).Value + " ";
            }
            String returnString = name + " farmers:" + farmers + " scientists:" + scientists + " miners:" + miners + " money:" + money + " science:" + science 
                + " food:" + food + " Docked Ships:" + ships.Count + " steel:"+ steel
                + "\nbuying: " + buyingString + " selling: " + sellingString+"\n";
            return returnString;
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
            updateEconomy();
            updateTradeList();
            
            if (steel > threshhold)
            {
                buildShip();
            }
            if (food < 0)
            {
                int nrOfStarvingPeople = (int)Math.Abs(food);
                food = 0;

                Console.WriteLine("People are starving on "+ name + "!! "  +killRandomPeople(nrOfStarvingPeople) + ".");
            }
            if (food > getPopulation()*2)
            {
                Console.WriteLine("A new " + createRandomPerson() + " has been born on "+ name +"!");
                food -= 10;
                
            }
        }
    
        private void buildShip()
        {
            if (steel > threshhold)
            {
                ships.Add(new Ship(this));
                steel -= threshhold;
            }

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
        /// <summary>
        /// updtate the buying and selling values depending on the current state and growth of the economy
        /// </summary>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public void updateTradeList() 
        {
            if (food < getPopulation() && foodgrowth() < 0){
                buying["food"] = getPopulation()*10;
                selling["food"] = 0;
            }
            else if (food < getPopulation() && foodgrowth() > 0)
            {
                buying["food"] = getPopulation() - food;
                selling["food"] = 0;
            }
            else if (food > getPopulation() && foodgrowth() < 0)
            {
                buying["food"] = 0;
                selling["food"] = 0;
            }
            else if (food > getPopulation() && foodgrowth() > 0)
            {
                buying["food"] = 0;
                selling["food"] = food - getPopulation();
            }
            if (ships.Count <1 && steelGrowth() < 1)
            {
                buying["steel"] = 100-steel;
                selling["steel"] = 0;
            }
            else if (ships.Count < 1 && steelGrowth() > 0)
            {
                buying["steel"] = 0;
                selling["steel"] = 0;
            }
            else if (ships.Count > 0 && steelGrowth() < 1)
            {
                buying["steel"] = 100-steel;
                selling["steel"] = 0;
            }
            else if (ships.Count > 0 && steelGrowth() > 0){ 
                buying["steel"] = 0;
                selling["steel"] = steel;
            }
            if (scienceGrowth() < 1)
            {
                buying["science"] = Math.Abs(scienceGrowth());
                selling["science"] = 0;
            }
            else if (scienceGrowth() > 0)
            {
                buying["science"] = 0;
                selling["science"] = science;
            }
            
            
        }
        private void updateEconomy()
        {
            food += foodgrowth();
            steel += steelGrowth();            
            if (science > scientists)
            {
                money += moneyGrowth();
                science += scienceGrowth();
            }
            else
            {
                money += science;
                science = 0;
            }
        }
        private float foodgrowth(){
            return (farmers * 2)-(farmers + scientists + miners);
        }
        private float steelGrowth()
        {
            return miners;
        }
        private float scienceGrowth()
        {
            return scientists*(-1);
        }
        private float moneyGrowth()
        {
            return scientists;
        }
        private int getPopulation()
        {
            return (int)farmers + (int)scientists + (int)miners;
        }
        private void calculateTradeAmount(String resource) //TODO implement 
        {
            
        }
    }
}
