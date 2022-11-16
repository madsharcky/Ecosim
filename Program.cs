using EcoSim.utils;
using System;

namespace EcoSim
{
    class Program
    {
        static void Main(string[] args)
        {            
            Simulation simulation = new Simulation();
            Console.WriteLine("wana load? y/n");
            if (Console.ReadLine() == "y")
            {
                simulation = (Simulation)GeneralFunctions.DeserializeItem("SaveFile");
            }

            simulation.start();

            Console.WriteLine("wana save? y/n");
            if (Console.ReadLine() == "y")
            {
                simulation.Ended = false;
                GeneralFunctions.SerializeItem("SaveFile", simulation);
            }
        }
    }
}
