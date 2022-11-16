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
        while (!Ended)
            {
                Console.WriteLine(workerStation.getStats());
                Console.WriteLine(scienceStation.getStats());                

                Console.WriteLine("want to end? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    Ended = true;
                }
                //else if (Console.ReadLine() == "save")
                //{
                //    //TODO: Test Saving of class
                //    IFormatter formatter = new BinaryFormatter();
                //    GeneralFunctions.SerializeItem("SaveFile", formatter, this);
                //}
                //else if (Console.ReadLine() == "load")
                //{
                //    // TODO: Test Loading of class
                //    IFormatter formatter = new BinaryFormatter();
                //    GeneralFunctions.DeserializeItem("SaveFile", formatter,this);
                //}
                else
                {
                    workerStation.nextRound();
                    scienceStation.nextRound();
                }
            }
        }
    }
}
