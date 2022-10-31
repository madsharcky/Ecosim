using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
    class Station
    {
        private String name;
        private float threshhold;
        private Dictionary<String, float> people;
        private Dictionary<String, float> resources;
        private List<Ship> ships;
        private Dictionary<String, int> location;
        private Random random = new Random();
        private Dictionary<String, float> buying; //describes the need
        private Dictionary<String, float> selling; //describes the surplus

        public string Name { get => name; set => name = value; }
        public void setResource(String resource, float value) { resources[resource] = value; }
        public float getResource(String resource) { return resources[resource]; }
        public void setPeople(String person, float value) { people[person] = value; }
        public float getPeople(String person) { return people[person]; }


        public Station(String name)
        {
            this.name = name;
            threshhold = 100;
            people = new Dictionary<string, float>() { { "farmers", 0 }, { "scientists", 0 }, { "miners", 0 } };
            resources = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 1000 }, { "money", 0 } };
            location = new Dictionary<string, int>() { {"x",0},{ "y", 0 },{ "z", 0 } };
            buying = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 },{"science",0 } };
            selling = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 0 } };
            ships = new List<Ship>();
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
            String returnString = name + " farmers:" + people["farmers"] + " scientists:" + people["scientists"] + " miners:" + people["miners"] + " money:" + resources["money"] + " science:" + resources["science"] 
                + " food:" + resources["food"] + " Docked Ships:" + ships.Count + " steel:"+ resources["steel"]
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
            
            if (resources["food"] > threshhold)
            {
                buildShip();
            }
            if (resources["food"] < 0)
            {
                int nrOfStarvingPeople = (int)Math.Abs(resources["food"]);
                resources["food"] = 0;

                Console.WriteLine("People are starving on "+ name + "!! "  +killRandomPeople(nrOfStarvingPeople) + ".");
            }
            if (resources["food"] > getPopulation()*2)
            {
                Console.WriteLine("A new " + createRandomPerson() + " has been born on "+ name +"!");
                resources["food"] -= 10;
                
            }
        }
    
        private void buildShip()
        {
            if (resources["steel"] > threshhold)
            {
                ships.Add(new Ship(this));
                resources["steel"] -= threshhold;
            }

        }
        private String createRandomPerson()
        {
            switch (random.Next(1, 4))
            {
                case 1:
                    people["farmers"]++;
                    return "farmer";
                case 2:
                    people["scientists"]++;
                    return "scientist";
                case 3:
                    people["miners"]++;
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
                        if (people["farmers"] > 0)
                        {
                            people["farmers"]--;
                            farmerDeath++;
                        }
                        break;
                    case 2:
                        if (people["scientists"] > 0)
                        {
                            people["scientists"]--;
                            scientistDeath++;
                        }
                        break;
                    case 3:
                        if (people["miners"] > 0)
                        {
                            people["miners"]--;
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
            if (resources["food"] < getPopulation() && foodgrowth() < 0){
                buying["food"] = getPopulation()*10;
                selling["food"] = 0;
            }
            else if (resources["food"] < getPopulation() && foodgrowth() > 0)
            {
                buying["food"] = getPopulation() - resources["food"];
                selling["food"] = 0;
            }
            else if (resources["food"] > getPopulation() && foodgrowth() < 0)
            {
                buying["food"] = 0;
                selling["food"] = 0;
            }
            else if (resources["food"] > getPopulation() && foodgrowth() > 0)
            {
                buying["food"] = 0;
                selling["food"] = resources["food"] - getPopulation();
            }
            if (ships.Count <1 && steelGrowth() < 1)
            {
                buying["steel"] = 100- resources["steel"];
                selling["steel"] = 0;
            }
            else if (ships.Count < 1 && steelGrowth() > 0)
            {
                buying["steel"] = 0;
                selling["steel"] = 0;
            }
            else if (ships.Count > 0 && steelGrowth() < 1)
            {
                buying["steel"] = 100- resources["steel"];
                selling["steel"] = 0;
            }
            else if (ships.Count > 0 && steelGrowth() > 0){ 
                buying["steel"] = 0;
                selling["steel"] = resources["steel"];
            }
            if (scienceGrowth() < 1)
            {
                buying["science"] = Math.Abs(scienceGrowth());
                selling["science"] = 0;
            }
            else if (scienceGrowth() > 0)
            {
                buying["science"] = 0;
                selling["science"] = resources["science"];
            }
            
            
        }
        private void updateEconomy()
        {
            resources["food"] += foodgrowth();
            resources["steel"] += steelGrowth();         
            if (resources["science"] > people["scientists"])
            {
                resources["money"] += moneyGrowth();
                resources["science"] += scienceGrowth();
            }
            else
            {
                resources["money"] += resources["science"];
                resources["science"] = 0;
            }
        }
        private float foodgrowth(){
            return (people["farmers"] * 2)-(people["farmers"] + people["scientists"] + people["miners"]);
        }
        private float steelGrowth()
        {
            return people["miners"];
        }
        private float scienceGrowth()
        {
            return people["scientists"] * (-1);
        }
        private float moneyGrowth()
        {
            return people["scientists"];
        }
        private int getPopulation()
        {
            return (int)people["farmers"] + (int)people["scientists"] + (int)people["miners"];
        }
        private bool sendShip(){
            if (ships.Count > 0)
            {
                
            }
            else
            {
                return false;
            }
        }
    }
}
