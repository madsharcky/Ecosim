using System;

namespace EcoSim
{
[Serializable]
    class Simulation
    {
        private Station workerStation;
        private Station scienceStation;
        public Simulation()
        {
            Ended = false;
            workerStation = new Station("workerStation",5,0,4);
            scienceStation = new Station("scienceStation",3,4,0,0,0,1000,0,3,4,0);            
        }

        public bool Ended { get; set; }

        public void start()
        {
                Console.WriteLine(workerStation.getStats());
                Console.WriteLine(scienceStation.getStats());       
        while (!Ended)
            {

                Console.WriteLine("want to end? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    Ended = true;
                }
                else
                {
                    Console.WriteLine(workerStation.nextRound());
                    Console.WriteLine(scienceStation.nextRound());
                }
            }
        }
    }
}
