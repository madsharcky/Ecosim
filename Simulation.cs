using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSim
{
    class Simulation
    {
        private bool ended;
        private Station workerStation;
        private Station scienceStation;
        private double distance;
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
                else
                {
                    workerStation.nextRound();
                    scienceStation.nextRound();
                }
            }
        }
    }
}
