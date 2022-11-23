using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
    [Serializable]
    
    class Station
    {
        private float threshhold;
        private Dictionary<string, float> people;
        private Dictionary<string, float> resources;
        private List<Ship> ships;
        private Dictionary<string, int> location;
        private Random random = new Random();
        private Dictionary<string, float> need; //describes the need
        private Dictionary<string, float> surplus; //describes the surplus
        private Dictionary<string, float> buying; //describes the list of goods that want to be bought
        private Dictionary<string, float> selling; //describes the list of goods that want to be sold

        public string Name { get; set; }
        public void setResource(string resource, float value) { resources[resource] = value; }
        public float getResource(string resource) { return resources[resource]; }
        public void setPeople(string person, float value) { people[person] = value; }
        public float getPeople(string person) { return people[person]; }

        /// <summary>
        /// Creates a station containing its own economics and people as well as ships
        /// </summary>
        /// <param name="name">The name of the station</param>
        /// <param name="farmerAmount">The amount of farmers on the Station</param>
        /// <param name="scientistAmount">The amount of scientists on the Station</param>
        /// <param name="minerAmount">The amount of miners on the Station</param>
        /// <param name="steelAmount">The amount of steel on the Station</param>
        /// <param name="foodAmount">The amount of food on the Station</param>
        /// <param name="scienceAmount">The amount of science on the station</param>
        /// <param name="moneyAmount">The amount of money on the station</param>
        /// <param name="xLocation">The location of the station in the x-axis</param>
        /// <param name="yLocation">The location of the station in the y-axis</param>
        /// <param name="zLocation">The location of the station in the z-axis</param>
        public Station(string name, float farmerAmount = 1, float scientistAmount = 0, float minerAmount = 0, float steelAmount = 0, float foodAmount = 0, float scienceAmount = 1000, float moneyAmount = 0, int xLocation = 0, int yLocation= 0, int zLocation = 0)
        {
            this.Name = name;
            threshhold = 100;
            people = new Dictionary<string, float>() { { "farmer", farmerAmount }, { "scientist", scientistAmount }, { "miner", minerAmount } };
            resources = new Dictionary<string, float>() { { "steel", steelAmount }, { "food", foodAmount }, { "science", scienceAmount }, { "money", moneyAmount }, {"promisedMoney", 0 } };
            location = new Dictionary<string, int>() { {"x", xLocation },{ "y", yLocation },{ "z", zLocation } };
            need = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 0 } };
            surplus = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 0 } };
            buying = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 },{"science",0 } };
            selling = new Dictionary<string, float>() { { "steel", 0 }, { "food", 0 }, { "science", 0 } };
            ships = new List<Ship>();
            updateTradeList();
        }

        /// <summary>
        /// returns a String with all the statistics of the Station
        /// </summary>
        /// <param name="born"> a sentence telling who was born</param>
        /// <param name="died"> a sentence saying who died and why</param>
        /// <returns>a String with all the statistics of the station</returns>
        public string getStats(String born = "", String died = "")
        {
            string buyingstring = "";
            string sellingstring = "";
            for (int i = 0; i < buying.Count; i++)
            {
                buyingstring += buying.ElementAt(i).Key + ": " + buying.ElementAt(i).Value + " ";
            }
            for (int i = 0; i < selling.Count; i++)
            {
                sellingstring += selling.ElementAt(i).Key + ": " + selling.ElementAt(i).Value + " ";
            }
            string returnstring = "At " + Name + born + died + " we have " + people["farmer"] + " farmers, " + people["scientist"] + " scientists, and " + people["miner"] + " miners. There is " 
                + resources["money"] + " money avalable, " + resources["science"] +" science, " + resources["food"] + " food and " + resources["steel"]+" steel. The number of Docked Ships is " + ships.Count
                + "\nThe buying list: " + buyingstring + "\nThe selling list: " + sellingstring+"\n";
            return returnstring;
        }

        /// <summary>
        /// Sets the location of the Station in a 3D plane
        /// </summary>
        /// <param name="axis">The axis (x,y or z) in the 3D Space</param>
        /// <param name="value">The location value in 3D Space</param>
        /// <returns>
        /// A boolean that describes if the value has been set or not
        /// </returns>
        public bool setLocation(string axis, int value)
        {
            try
            {
                location[axis] = value;
                return true;

            }
            catch
            {
                return false;
            }            
        }
        /// <summary>
        /// Returns the location of the Station in a 3D plane
        /// </summary>
        /// <param name="axis">The axis (x,y or z) in the 3D Space</param>
        /// <returns>
        /// An Integer value describing the location in 3D Space
        /// </returns>
        public int getLocation(string axis)
        {
            return location[axis];
        }

        /// <summary>
        /// Advances the Station to the next round, calculationg all the Economic changes
        /// </summary>
        /// <returns>a String with all the current stats of the station including changes in population</returns>
        public String nextRound()
        {
            String born = "";
            String died = "";
            updateEconomy();
            updateTradeList();
            
            if (resources["food"] > threshhold)
            {
                buildShip(this);
            }
            if (resources["food"] < 0)
            {
                int nrOfStarvingPeople = (int)Math.Abs(resources["food"]);
                resources["food"] = 0;

                died = " "+killRandomPeople(nrOfStarvingPeople) + " due to starvation, ";
            }
            if (resources["food"] > getPopulation()*2)
            {
                born = " a new " + createRandomPerson() + " has been born, ";
                resources["food"] -= 10;
                
            }
            return getStats(born,died);
        }

        /// <summary>
        /// Creates a new Ship object with a declared Station as its homebase.
        /// </summary>
        /// <param name="homebase">the homebase of the built ship (usually this)</param>
        private void buildShip(Station homebase)
        {
            if (resources["steel"] > threshhold)
            {
                ships.Add(new Ship(homebase));
                resources["steel"] -= threshhold;
            }

        }
        /// <summary>
        /// creates a random person
        /// </summary>
        /// <returns>returns a String containing the type of person ex: farmer</returns>
        private string createRandomPerson()
        {
            String mostNeeded = need.Aggregate((v1, v2) => v1.Value > v2.Value ? v1 : v2).Key;
            switch (mostNeeded)
            {
                case "food":
                    people["farmer"]++;
                    return "farmer";
                case "steel":
                    people["miner"]++;
                    return "miner";
                default:
                    switch (random.Next(1, 4))
                    {
                        case 1:
                            people["farmer"]++;
                            return "farmer";
                        case 2:
                            people["scientist"]++;
                            return "scientist";
                        case 3:
                            people["miner"]++;
                            return "miner";
                        default:
                            return "bug";
                    }
            }
            
        }

        /// <summary>
        /// Kills a number of random peoplpe
        /// </summary>
        /// <param name="amount">the amount of people to be killed</param>
        /// <returns>
        /// A string describing the Amount of people that have died
        /// </returns>
        private string killRandomPeople(int amount)
        {
            Dictionary<string, int> killedPeople = new Dictionary<string, int>();
            String returnString = "";
            foreach (KeyValuePair<string, float> entry in people)
            {
                if (entry.Value > 0)
                {
                    killedPeople.Add(entry.Key, 0);
                }
            }

            for (int i = 0; i < amount; i++)
            {
                int randomIndex = random.Next(killedPeople.Count);
                string person = killedPeople.Keys.ElementAt(randomIndex);
                if (killedPeople[person] == people[person])
                {
                    i--;
                }
                else
                {
                    killedPeople[person]++;
                }
            }

            foreach (String person in killedPeople.Keys)
            {
                if (killedPeople[person] > 0)
                {
                    people[person] -= killedPeople[person];
                    if (killedPeople[person] > 1)
                    {
                        returnString += " and " + killedPeople[person] + " " + person + "s have died";
                    }
                    else
                    {
                        returnString += " and a " + person + " has died";
                    }
                }
            }
                       
            return returnString.Remove(0, 5);
        }
        /// <summary>
        /// Updtate the needs and surpluses values depending on the current state and growth of the economy
        /// </summary>
        /// <returns>
        /// </returns>
        public void updateNeeds()
        {
            if (resources["food"] < getPopulation() && foodgrowth() < 0){
                need["food"] = getPopulation()*10;
                surplus["food"] = 0;
            }
            else if (resources["food"] < getPopulation() && foodgrowth() > 0)
            {
                need["food"] = getPopulation() - resources["food"];
                surplus["food"] = 0;
            }
            else if (resources["food"] > getPopulation() && foodgrowth() < 0)
            {
                need["food"] = 0;
                surplus["food"] = 0;
            }
            else if (resources["food"] > getPopulation() && foodgrowth() > 0)
            {
                need["food"] = 0;
                surplus["food"] = resources["food"] - getPopulation();
            }
            if (ships.Count <1 && steelGrowth() < 1)
            {
                need["steel"] = 100- resources["steel"];
                surplus["steel"] = 0;
            }
            else if (ships.Count < 1 && steelGrowth() > 0)
            {
                need["steel"] = 0;
                surplus["steel"] = 0;
            }
            else if (ships.Count > 0 && steelGrowth() < 1)
            {
                need["steel"] = 100- resources["steel"];
                surplus["steel"] = 0;
            }
            else if (ships.Count > 0 && steelGrowth() > 0)
            {
                need["steel"] = 0;
                surplus["steel"] = resources["steel"];
            }
            if (scienceGrowth() < 1)
            {
                need["science"] = Math.Abs(scienceGrowth());
                surplus["science"] = 0;
            }
            else if (scienceGrowth() > 0)
            {
                need["science"] = 0;
                surplus["science"] = resources["science"];
            }            
        }
        
        private void updateTradeList()
        {
            foreach (var key in selling.Keys) //reset the selling list
            {
                selling[key] = 0;
            }
            foreach (var key in buying.Keys) //reset the buying list
            {
                buying[key] = 0;
            }
            
            selling = surplus;
            
            String mostNeeded = need.Aggregate((v1, v2) => v1.Value > v2.Value ? v1 : v2).Key;
            if (resources["money"] - need[mostNeeded] <= 0) //spend all money on most needed item
            {
                buying[mostNeeded] = need[mostNeeded] - resources["money"];
                resources["promisedMoney"] = resources["money"];                
            }
            else
            {
                buying[mostNeeded] = need[mostNeeded];
                resources["promisedMoney"] += buying[mostNeeded];
                // select random item from need Dictionary that is not the most needed item
                string randomKey = //TODO: finish this

            }

        }
        /// <summary>
        /// Updates the amount of resources on the station
        /// </summary>
        private void updateEconomy()
        {
            resources["food"] += foodgrowth();
            resources["steel"] += steelGrowth();
            if (resources["science"] > people["scientist"])
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
        /// <summary>
        /// calculates the amount of foodgrowth
        /// </summary>
        /// <returns>
        /// a float value containing the positive or negative growth of food
        /// </returns>
        private float foodgrowth(){
            return (people["farmer"] * 2)-(people["farmer"] + people["scientist"] + people["miner"]);
        }
        /// <summary>
        /// calculates the amount of steelgrowth
        /// </summary>
        /// <returns>
        /// a float value containing the positive or negative growth of steel
        /// </returns>
        private float steelGrowth()
        {
            return people["miner"];
        }
        /// <summary>
        /// calculates the amount of sciencegrowth
        /// </summary>
        /// <returns>
        /// a float value containing the positive or negative growth of science
        /// </returns>
        private float scienceGrowth()
        {
            return (people["scientist"] * (-1))+1;
        }
        /// <summary>
        /// calculates the amount of moneygrowth
        /// </summary>
        /// <returns>
        /// a float value containing the positive or negative growth of money
        /// </returns>
        private float moneyGrowth()
        {
            return people["scientist"];
        }
        /// <summary>
        /// returns the total amount of people living on the station rounded to the nearest int value
        /// </summary>
        /// <returns>
        /// an integer representing the number of people on the station 
        /// </returns>
        private int getPopulation()
        {
            return (int)people["farmer"] + (int)people["scientist"] + (int)people["miner"];
        }
        /// <summary>
        /// send a docked ship from the station to a target location
        /// </summary>
        /// <param name="location">the destination location, needs x,y and z coordinates</param>
        /// <returns>the ship thhat has been sent or null if no ship has been sent</returns>
        private Ship sendShip(Dictionary<string,int> location){
            if (ships.Count > 0)
            {
                return ships[0];
            }
            else
            {
                return null;
            }
        }
    }
}
