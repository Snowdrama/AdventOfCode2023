using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days.Day5
{
    public class Day5
    {
        public Day5(string[] mapFiles)
        {
            string[] seedStrings = File.ReadAllLines(mapFiles[0]);
            long[] seeds = new long[seedStrings.Length];
            long[] output = new long[seedStrings.Length];

            for (int i = 0; i < seedStrings.Length; i++)
            {
                seeds[i] = long.Parse(seedStrings[i]);
                output[i] = seeds[i];
            }

            string[] seed_to_soil = File.ReadAllLines(mapFiles[1]);
            PlantMapCollection seed_to_soil_maps = new PlantMapCollection(seed_to_soil);
            output = seed_to_soil_maps.ConvertSeeds(output);
            Console.WriteLine("***seed_to_soil Conversion Complete***");


            string[] soil_to_fertilizer = File.ReadAllLines(mapFiles[2]);
            PlantMapCollection soil_to_fertilizer_maps = new PlantMapCollection(soil_to_fertilizer);
            output = soil_to_fertilizer_maps.ConvertSeeds(output);
            Console.WriteLine("***soil_to_fertilizer Conversion Complete***");

            string[] fertilizer_to_water = File.ReadAllLines(mapFiles[3]);
            PlantMapCollection fertilizer_to_water_maps = new PlantMapCollection(fertilizer_to_water);
            output = fertilizer_to_water_maps.ConvertSeeds(output);
            Console.WriteLine("***fertilizer_to_water Conversion Complete***");


            string[] water_to_light = File.ReadAllLines(mapFiles[4]);
            PlantMapCollection water_to_light_maps = new PlantMapCollection(water_to_light);
            output = water_to_light_maps.ConvertSeeds(output);
            Console.WriteLine("***water_to_light Conversion Complete***");


            string[] light_to_temprature = File.ReadAllLines(mapFiles[5]);
            PlantMapCollection light_to_temprature_maps = new PlantMapCollection(light_to_temprature);
            output = light_to_temprature_maps.ConvertSeeds(output);
            Console.WriteLine("***light_to_temprature Conversion Complete***");


            string[] temperature_to_humidity = File.ReadAllLines(mapFiles[6]);
            PlantMapCollection temperature_to_humidity_maps = new PlantMapCollection(temperature_to_humidity);
            output = temperature_to_humidity_maps.ConvertSeeds(output);
            Console.WriteLine("***temperature_to_humidity Conversion Complete***");


            string[] humidity_to_location = File.ReadAllLines(mapFiles[7]);
            PlantMapCollection humidity_to_location_maps = new PlantMapCollection(humidity_to_location);
            output = humidity_to_location_maps.ConvertSeeds(output);
            Console.WriteLine("***humidity_to_location Conversion Complete***");

            Console.WriteLine($"Smallest Location is: {output.Min()}");

        }
    }
    public class Range
    {
        public long min;
        public long max;

        public Range(long min, long rangeCount)
        {
            this.min = min;
            this.max = min + rangeCount;
        }
        public bool Contains(long input)
        {
            if (input >= min && input <= max)
            {
                return true;
            }
            return false;
        }
        public static long Remap(long input, Range current, Range target)
        {
            if (current.Contains(input))
            {
                long newInput = input;
                //get offset from range start
                //offset = 55 - 50 = 5
                long offset = input - current.min;
                //remap input to new range
                newInput = target.min + offset;

                return newInput;
            }
            return input;
        }
    }
    public class PlantMap
    {
        public Range InputRange;
        public Range OutputRange;

        public PlantMap(Range inputRange, Range outputRange)
        {
            InputRange = inputRange;
            OutputRange = outputRange;
        }
        public bool Contains(long inputValue)
        {
            return InputRange.Contains(inputValue);
        }
        public long Convert(long inputValue)
        {
            long output = Range.Remap(inputValue, InputRange, OutputRange);
            return Range.Remap(inputValue, InputRange, OutputRange);
        }
    }

    public class PlantMapCollection
    {
        List<PlantMap> maps = new List<PlantMap>();

        public PlantMapCollection(string[] mapInputStrings)
        {
            for (int i = 0; i < mapInputStrings.Length; i++)
            {
                var rangeValues = mapInputStrings[i].Split(' ');
                Range inputRange = new Range(long.Parse(rangeValues[1]), long.Parse(rangeValues[2]));
                Range outputRange = new Range(long.Parse(rangeValues[0]), long.Parse(rangeValues[2]));
                PlantMap newMap = new PlantMap(inputRange, outputRange);
                Console.WriteLine($"Adding New Map! Input: {inputRange.min} -> {inputRange.max} | Output  {outputRange.min} -> {outputRange.max}");
                maps.Add(newMap);
            }
        }

        public long[] ConvertSeeds(long[] input)
        {
            //for (int i = 0; i < input.Length; i++)
            //{
            //    Console.WriteLine($"Input Values: {input[i]}");
            //}
            long[] output = input.ToArray();
            for (int i = 0; i < input.Length; i++)
            {
                long seedInput = input[i];
                long seedOutput = seedInput;
                for (int j = 0; j < maps.Count; j++)
                {
                    if (maps[j].Contains(seedInput))
                    {
                        seedOutput = maps[j].Convert(seedInput);
                        break;
                    }
                }
                output[i] = seedOutput;
            }


            for (int i = 0; i < output.Length; i++)
            {
                Console.WriteLine($"New Values: {input[i]}->{output[i]}");
            }
            return output;
        }
    }
}
