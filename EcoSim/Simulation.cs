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
            workerStation = new Station("workerStation");
            workerStation.Farmers = 5;
            workerStation.Miners = 4;
            scienceStation = new Station("scienceStation");
            scienceStation.Farmers = 3;
            scienceStation.Scientists = 4;
            scienceStation.setLocation("x", 3);
            scienceStation.setLocation("y", 4);
            distance = getDistanceBetween2Points(workerStation.getLocation("x"), workerStation.getLocation("y"), workerStation.getLocation("z"), scienceStation.getLocation("x"), scienceStation.getLocation("y"), scienceStation.getLocation("z"));

        }
        private double getDistanceBetween2Points(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            return Math.Pow((Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2) * 1.0), 0.5);
        }

        public void start()
        {
        while (!ended)
            {
                Console.WriteLine(workerStation.getStats());
                Console.WriteLine(scienceStation.getStats());
                workerStation.nextRound();
                scienceStation.nextRound();



                Console.WriteLine("want to end? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    ended = true;
                }
            }
        }
    }
}
