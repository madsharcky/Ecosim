using EcoSim.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
[Serializable]
    class Simulation
    {
        private bool ended;
        private Station workerStation;
        private Station scienceStation;
        public Simulation()
        {
            ended = false;
            workerStation = new Station("workerStation",5,0,4);
            scienceStation = new Station("scienceStation",3,4,0,0,0,1000,0,3,4,0);            
        }
        public void start()
        {
        while (!ended)
            {
                Console.WriteLine(workerStation.getStats());
                Console.WriteLine(scienceStation.getStats());                

                Console.WriteLine("want to end? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    ended = true;
                }
                else if (Console.ReadLine() == "save")
                {
                    //TODO: Test Saving of class
                    IFormatter formatter = new BinaryFormatter();
                    GeneralFunctions.SerializeItem("SaveFile", formatter, this);
                }
                else if (Console.ReadLine() == "load")
                {
                    // TODO: Test Loading of class
                    IFormatter formatter = new BinaryFormatter();
                    GeneralFunctions.DeserializeItem("SaveFile", formatter,this);
                }
                else
                {
                    workerStation.nextRound();
                    scienceStation.nextRound();
                }
            }
        }
    }
}
