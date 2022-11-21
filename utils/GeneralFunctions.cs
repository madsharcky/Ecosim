using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EcoSim.utils
{
    [Serializable]
    /// <summary>
    /// a class containing a number of usefull function to be used by everyone
    /// </summary>
    class GeneralFunctions
    {
        /// <summary>
        /// calculates the distance between 2 points in 3D space
        /// </summary>
        /// <param name="point1">The coordinates of the first point in 3D space, needs x,y and z coordinates</param>
        /// <param name="point2">The coordinates of the second point in 3D space, needs x,y and z coordinates</param>
        /// <returns>a double representing the distance between the 2 points</returns>
        public double getDistanceBetween2Points(Dictionary<string, int> point1, Dictionary<string, int> point2)
        {
            return Math.Pow((Math.Pow(point2["x"] - point1["x"], 2) + Math.Pow(point2["y"] - point1["y"], 2) + Math.Pow(point2["z"] - point1["z"], 2) * 1.0), 0.5);
        }

        //https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable?redirectedfrom=MSDN&view=net-7.0

        /// <summary>
        /// serializes an object to a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="classObject"></param>
        public static void SerializeItem(string fileName, Simulation classObject)
        {
            // Create an instance of the type and serialize it.
            String dateTime = DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss");
            String path = GetPath() + "/savefiles";
            Directory.CreateDirectory(path);
            path = GetPath() + "/savefiles/" +fileName + "_"+ dateTime + ".bin";
           
            System.IO.Stream stream = File.OpenWrite(path);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, classObject);

            Console.WriteLine("Saving succesfull");
            stream.Flush();
            stream.Close();
            stream.Dispose();
        }

        /// <summary>
        /// load a file from a serialized class
        /// </summary>
        /// <param name="filePath">the file from which should be loaded</param>
        /// <returns>a classobject</returns>
        public static Simulation DeserializeItem(string filePath)
        {
            String path = filePath;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            Simulation classObject = (Simulation)formatter.Deserialize(stream);
            Console.WriteLine("Loading succesfull");
            stream.Flush();
            stream.Close();
            stream.Dispose();
            return classObject;
        }

        /// <summary>
        /// returns the path of the current directory
        /// </summary>
        /// <returns>the path of the current directory</returns>
        public static string GetPath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }
}
