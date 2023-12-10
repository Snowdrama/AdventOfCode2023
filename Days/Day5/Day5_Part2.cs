using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days.Day5_Part2
{
    public class Day5_Part2
    {
        public Day5_Part2(string[] mapFiles)
        {
            string[] seedStrings = File.ReadAllLines(mapFiles[0]);
            long[] seeds = new long[seedStrings.Length];

            for (int i = 0; i < seedStrings.Length; i++)
            {
                seeds[i] = long.Parse(seedStrings[i]);
            }

            List<Range> seedRanges = new List<Range>();
            for (int i = 0; i < seeds.Length; i += 2)
            {
                Console.WriteLine($"Seed Range: {seeds[i]} -> {seeds[i] + seeds[i + 1]}");
                seedRanges.Add(new Range(seeds[i], seeds[i] + seeds[i + 1]));
            }

            //for (int i = 0; i < seedRanges.Count; i++)
            //{
            //    Range testMap = new Range(90, 100);

            //    if (seedRanges[i].Overlaps(testMap))
            //    {
            //        Console.WriteLine($"Splitting: {seedRanges[i]} by {testMap}");
            //        var remainingRanges = testMap.SplitRange(seedRanges[i]);
            //        foreach (var item in remainingRanges)
            //        {
            //            Console.WriteLine($"Result Range: {item}");
            //        }
            //        break;
            //    }
            //}


            //Console.WriteLine();



            string[] seed_to_soil = File.ReadAllLines(mapFiles[1]);
            PlantMapCollection seed_to_soil_maps = new PlantMapCollection(seed_to_soil);
            seedRanges = seed_to_soil_maps.ConvertRanges(seedRanges);

            foreach (var item in seedRanges)
            {
                Console.WriteLine($"Ranges After Soil Map: {item}");
            }
            Console.WriteLine("***seed_to_soil Conversion Complete*************************************************\n\n");


            string[] soil_to_fertilizer = File.ReadAllLines(mapFiles[2]);
            PlantMapCollection soil_to_fertilizer_maps = new PlantMapCollection(soil_to_fertilizer);
            seedRanges = soil_to_fertilizer_maps.ConvertRanges(seedRanges);
            foreach (var item in seedRanges)
            {
                Console.WriteLine($"Ranges After fert Map: {item}");
            }
            Console.WriteLine("***soil_to_fertilizer Conversion Complete*************************************************\n\n");

            string[] fertilizer_to_water = File.ReadAllLines(mapFiles[3]);
            PlantMapCollection fertilizer_to_water_maps = new PlantMapCollection(fertilizer_to_water);
            seedRanges = fertilizer_to_water_maps.ConvertRanges(seedRanges);
            foreach (var item in seedRanges)
            {
                Console.WriteLine($"Ranges After Water Map: {item}");
            }
            Console.WriteLine("***fertilizer_to_water Conversion Complete*************************************************\n\n");


            string[] water_to_light = File.ReadAllLines(mapFiles[4]);
            PlantMapCollection water_to_light_maps = new PlantMapCollection(water_to_light);
            seedRanges = water_to_light_maps.ConvertRanges(seedRanges);
            foreach (var item in seedRanges)
            {
                Console.WriteLine($"Ranges After water_to_light Map: {item}");
            }
            Console.WriteLine("***water_to_light Conversion Complete*************************************************\n\n");


            string[] light_to_temprature = File.ReadAllLines(mapFiles[5]);
            PlantMapCollection light_to_temprature_maps = new PlantMapCollection(light_to_temprature);
            seedRanges = light_to_temprature_maps.ConvertRanges(seedRanges);
            foreach (var item in seedRanges)
            {
                Console.WriteLine($"Ranges After light_to_temprature Map: {item}");
            }
            Console.WriteLine("***light_to_temprature Conversion Complete*************************************************\n\n");


            string[] temperature_to_humidity = File.ReadAllLines(mapFiles[6]);
            PlantMapCollection temperature_to_humidity_maps = new PlantMapCollection(temperature_to_humidity);
            seedRanges = temperature_to_humidity_maps.ConvertRanges(seedRanges);
            Console.WriteLine("***temperature_to_humidity Conversion Complete*************************************************\n\n");


            string[] humidity_to_location = File.ReadAllLines(mapFiles[7]);
            PlantMapCollection humidity_to_location_maps = new PlantMapCollection(humidity_to_location);
            seedRanges = humidity_to_location_maps.ConvertRanges(seedRanges);
            Console.WriteLine("***humidity_to_location Conversion Complete*************************************************\n\n");

            long minValue = long.MaxValue;
            foreach (var item in seedRanges)
            {
                if (item.min < minValue) minValue = item.min;
            }
            Console.WriteLine($"Smallest Location is: {minValue}");
        }
    }

    public class Range
    {
        public long min;
        public long max;

        public Range(long min, long max)
        {
            this.min = min;
            this.max = max;
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
            Console.WriteLine($"{input} -> {current} -> {target}");
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
        public bool Overlaps(Range range)
        {
            if(range.min <= max && range.max >= min)
            {
                return true;
            }
            return false;
        }

        public List<Range> SplitRange(Range inputRange)
        {
            List<Range> remainingRanges = new List<Range>();

            if (!inputRange.Overlaps(this))
            {
                remainingRanges.Add(inputRange);
                return remainingRanges;
            }
            Console.WriteLine($"Splitting {inputRange} by [{min}->{max}]");
            long largestMin = long.Max(inputRange.min, min);
            long smallestMax = long.Min(inputRange.max, max);

            Range overlapRange = new Range(largestMin, smallestMax);
            remainingRanges.Add(overlapRange);

            Console.WriteLine($"overlapRange {overlapRange}");
            if (overlapRange.min > inputRange.min)
            {
                Range minRange = new Range(inputRange.min, overlapRange.min-1);
                Console.WriteLine($"minRange {minRange}");
                remainingRanges.Add(minRange);
            }

            if(overlapRange.max < inputRange.max)
            {
                Range maxRange = new Range(overlapRange.max+1, inputRange.max);
                Console.WriteLine($"maxRange {maxRange}");
                remainingRanges.Add(maxRange);
            }
            return remainingRanges;
        }

        public override string ToString()
        {
            return $"Range:[{min}->{max}]";
        }
    }
    public class PlantMap
    {
        public Range InputMapRange;
        public Range OutputMapRange;

        public PlantMap(Range inputRange, Range outputRange)
        {
            InputMapRange = inputRange;
            OutputMapRange = outputRange;
        }
        public bool Contains(long inputValue)
        {
            return InputMapRange.Contains(inputValue);
        }
        public long Convert(long inputValue)
        {
            return Range.Remap(inputValue, InputMapRange, OutputMapRange);
        }

        public List<Range> SplitRanges(List<Range> inputRanges)
        {
            List<Range> leftoverRanges = new List<Range>();

            foreach (var item in inputRanges)
            {
                var splitRanges = InputMapRange.SplitRange(item);
                leftoverRanges.AddRange(splitRanges);
            }
            return leftoverRanges;
        }

        public (List<Range>, List<Range>) ConvertRanges(List<Range> ranges)
        {
            var splitRanges = SplitRanges(ranges);

            List<Range> converted = new List<Range>();
            List<Range> notConverted = new List<Range>();

            foreach (var item in splitRanges)
            {
                Console.WriteLine($"BeforeConversion: {item}");
                if (item.Overlaps(InputMapRange))
                {
                    item.min = Convert(item.min);
                    item.max = Convert(item.max);
                    converted.Add(item);
                }
                else
                {
                    notConverted.Add(item);
                }
                Console.WriteLine($"Converted: {item}");
            }


            return (converted, notConverted);
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
                Range inputRange = new Range(long.Parse(rangeValues[1]), long.Parse(rangeValues[1]) + long.Parse(rangeValues[2]));
                Range outputRange = new Range(long.Parse(rangeValues[0]), long.Parse(rangeValues[0]) + long.Parse(rangeValues[2]));
                PlantMap newMap = new PlantMap(inputRange, outputRange);
                Console.WriteLine($"Adding New Map! Input: {inputRange.min} -> {inputRange.max} | Output  {outputRange.min} -> {outputRange.max}");
                maps.Add(newMap);
            }
        }

        //public long[] ConvertSeeds(long[] input)
        //{
        //    //for (int i = 0; i < input.Length; i++)
        //    //{
        //    //    Console.WriteLine($"Input Values: {input[i]}");
        //    //}
        //    long[] output = input.ToArray();
        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        long seedInput = input[i];
        //        long seedOutput = seedInput;
        //        for (int j = 0; j < maps.Count; j++)
        //        {
        //            if (maps[j].Contains(seedInput))
        //            {
        //                seedOutput = maps[j].Convert(seedInput);
        //                break;
        //            }
        //        }
        //        output[i] = seedOutput;
        //    }


        //    for (int i = 0; i < output.Length; i++)
        //    {
        //        Console.WriteLine($"New Values: {input[i]}->{output[i]}");
        //    }
        //    return output;
        //}

        public List<Range> ConvertRanges(List<Range> inputRanges)
        {
            List<Range> segmentsToProcess = new List<Range>(inputRanges);

            List<Range> converted = new List<Range>();

            for (int i = 0; i < maps.Count; i++)
            {
                Console.WriteLine($"Converting Map: {i}");
                var (convertedSegments, leftoverSegments)= maps[i].ConvertRanges(segmentsToProcess);
                segmentsToProcess = leftoverSegments;
                converted.AddRange(convertedSegments);
            }

            List<Range> output = new List<Range>();

            output.AddRange(segmentsToProcess);
            output.AddRange(converted);

            return output;
        }
    }
}
