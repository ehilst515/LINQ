using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LINQ
{
    class Program
    {
        public static RootObject Root = JsonConvert.DeserializeObject<RootObject>(Data.data);

        static void Main(string[] args)
        {
            Consoler();
        }

        public static void Consoler()
        {

            int fullCount = Count(Neighborhoods);
            Console.WriteLine($"There are {fullCount} listed neighborhoods total: \n");
            Console.WriteLine(string.Join(", ", Neighborhoods));

            Pause();

            int noEmptyCount = Count(NoEmptyNeighborhoods);
            Console.WriteLine("Chaining query... \n");
            Console.WriteLine($"There are {noEmptyCount} listed neighborhoods with names: \n");
            Console.WriteLine(string.Join(", ", NoEmptyNeighborhoods));

            Pause();

            int noDuplicateCount = Count(NoDuplicateNeighborhoods);
            Console.WriteLine("Chaining query... \n");
            Console.WriteLine($"There are {noDuplicateCount} non-duplicate neighborhoods");
            Console.WriteLine(string.Join(", ", NoDuplicateNeighborhoods));

            Pause();

            int uniqueNeighborhoodsCount = Count(UniqueNewYork);
            Console.WriteLine("Single query... \n");
            Console.WriteLine($"There are {uniqueNeighborhoodsCount} unique neighborhoods: \n");
            Console.WriteLine(string.Join(", ", UniqueNewYork));

            Pause();

            int noEmptyMethodCount = Count(NoEmptyMethod);
            Console.WriteLine("Using method calls to filter out nameless neighboorhoods... \n");
            Console.WriteLine($"There are {noEmptyMethodCount} listed neighborhoods with names: \n");
            Console.WriteLine(string.Join(", ", NoEmptyMethod));

            Pause();
        }

        public static int Count(IEnumerable<string> List)
        {
            int counter = 0;

            foreach (var n in List.Select((value, index) => new { value, index }))
            {
                counter = n.index;
            }

            return counter + 1;
        }

        public static void Pause()
        {
            Console.WriteLine("\n \n Press enter to continue.");
            Console.ReadLine();
        }

        public class Data
        {
            public static string fileName = "data.json";
            public static string data = File.ReadAllText(fileName);

            public JObject json = JObject.Parse(data);

        }

        public class RootObject
        {
            public string type { get; set; }
            public List<Feature> features { get; set; }
        }

        public class Feature
        {
            public string type { get; set; }
            public Geometry geometries { get; set; }
            public Property properties { get; set; }
        }

        public class Point
        {
            public string type { get; set; }
        }

        public class Geometry
        {
            public string type { get; set; }
            public double coordinates { get; set; }
        }

        public class Property
        {
            public string zip { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string address { get; set; }
            public string borough { get; set; }
            public string neighborhood { get; set; }
            public string county { get; set; }
        }

        // Queries 
        public static IEnumerable<string> Neighborhoods =
            from all in Root.features
            select all.properties.neighborhood;

        public static IEnumerable<string> NoEmptyNeighborhoods =
            from noE in Neighborhoods
            where noE !=""
            select noE;

        public static IEnumerable<string> NoDuplicateNeighborhoods =
             NoEmptyNeighborhoods
            .Select(n => n)
            .Distinct()
            .Where(a => a != "");

        public static IEnumerable<string> UniqueNewYork =
            Root.features
            .Select(n => n.properties.neighborhood)
            .Distinct()
            .Where(a => a != "");

        public static IEnumerable<string> NoEmptyMethod =
            Neighborhoods
            .Select(n => n)
            .Where(a => a != "");


    }

}
