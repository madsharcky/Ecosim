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
            workerStation.setPeople("farmers", 5);
            workerStation.setPeople("miners", 4);
            workerStation.updateTradeList();
            scienceStation = new Station("scienceStation");
            scienceStation.setPeople("farmers", 3);
            scienceStation.setPeople("scientists", 4);
            scienceStation.updateTradeList();
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
