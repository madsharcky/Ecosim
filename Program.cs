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
                //get a list of all the files in the save folder
                string[] files = System.IO.Directory.GetFiles(GeneralFunctions.GetPath() + "/savefiles/");
                //print the list of files
                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine(i + " " + files[i]);
                }
                //get the file the user wants to load
                Console.WriteLine("which file do you want to load?");
                int file = Convert.ToInt32(Console.ReadLine());
                //load the file
                simulation = GeneralFunctions.DeserializeItem(files[file]);
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
