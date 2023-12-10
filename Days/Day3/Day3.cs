using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days.Day3
{
    

    public struct Vector2I
    {
        public int X;
        public int Y;
        public Vector2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2I operator +(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2I operator -(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X - b.X, a.Y - b.Y);
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public class Gear
    {
        public List<Part> Parts = new List<Part>();
        public Vector2I position;
        public Vector2I Min
        {
            get
            {
                return position - new Vector2I(1, 1);
            }
        }

        public Vector2I Max
        {
            get
            {
                return position + new Vector2I(2, 2);
            }
        }

        public static bool Overlap(Gear gear, Part part)
        {
            if(gear.Min.X < part.Max.X &&
               gear.Max.X > part.Min.X &&
               gear.Min.Y < part.Max.Y &&
               gear.Max.Y > part.Min.Y 
               )
            {
                return true;
            }
            return false;
        }
    }
    public class Part
    {
        public Vector2I position;

        public Vector2I Min
        {
            get
            {
                return position;
            }
        }

        public Vector2I Max
        {
            get
            {
                return position + new Vector2I(size, 1);
            }
        }

        public int size
        {
            get
            {
                return partNumberDigits.Count;
            }
        }
        public int partNumber
        {
            get
            {
                return int.Parse(this.ToString());
            }
        }
        public bool isValid
        {
            get
            {
                return partNumberCharacters.Where(x => x != '.' && !char.IsDigit(x)).Count() > 0;
            }
        }

        public bool IsGear
        {
            get
            {
                return partNumberCharacters.Where(x => x == '*').Count() > 0;
            }
        }

        public List<char> partNumberDigits = new List<char>();
        public List<char> partNumberCharacters = new List<char>();

        public override string ToString()
        {
            string partNumber = "";
            for (int i = 0; i < partNumberDigits.Count; i++)
            {
                partNumber += partNumberDigits[i];
            }
            return partNumber;
        }
    }
    public class Day3
    {
        char[,] characters;

        List<Part> parts = new List<Part>();
        public Day3(string[] lines)
        {
            characters = new char[lines[0].Length, lines.Length];

            //make a 2D array of all the chars
            for (int i = 0; i < lines.Length; i++)
            {
                char[] lineChars = lines[i].ToCharArray();
                for (int c = 0; c < lines[i].Length; c++)
                {
                    characters[c, i] = lineChars[c];
                }
            }
        }
        public void LoadPartNumbers()
        {
            parts.Clear();
            //itterate over each line and look for runs of numbers
            for (int y = 0; y < characters.GetLength(1); y++)
            {
                Part newPart = null;
                for (int x = 0; x < characters.GetLength(0); x++)
                {
                    Console.Write(characters[x, y]);
                    if (char.IsDigit(characters[x, y]))
                    {
                        //add to current part
                        if (newPart == null)
                        {
                            newPart = new Part();
                            newPart.position = new Vector2I(x, y);
                        }
                        newPart.partNumberDigits.Add(characters[x, y]);
                    }
                    else
                    {
                        if (newPart != null)
                        {
                            parts.Add(newPart);
                            newPart = null;
                        }
                    }
                }
                if (newPart != null)
                {
                    parts.Add(newPart);
                }

                Console.Write("\n");
            }

            for (int i = 0; i < parts.Count; i++)
            {
                Console.WriteLine($"Part[{i}]: {parts[i]}");
            }
            Console.WriteLine($"Total Part Count: {parts.Count}");
        }
    
        public void SumValidParts()
        {
            int sum = 0;
            foreach (var item in parts)
            {
                Console.WriteLine($"Part: {item}");

                for (int y = item.position.Y - 1; y < item.position.Y + 2; y++)
                {
                    Console.Write($"[P:{item}]:");
                    for (int x = item.position.X - 1; x < item.position.X + item.size + 1; x++)
                    {
                        if (x >= 0 && y >= 0 && x < characters.GetLength(0) && y < characters.GetLength(1))
                        {
                            Console.Write($"{characters[x, y]}");
                            item.partNumberCharacters.Add(characters[x, y]);
                        }
                    }
                    Console.Write("\n");
                }

                Console.Write($"[P:{item}] IsValid {item.isValid}\n");
                if (item.isValid)
                {
                    sum += item.partNumber;
                }
            }
            Console.WriteLine($"Part Number Sum: {sum}");
        }


        List<Gear> gears = new List<Gear>();
        public void LoadGears()
        {
            for (int y = 0; y < characters.GetLength(1); y++)
            {
                for (int x = 0; x < characters.GetLength(0); x++)
                {
                    if (characters[x, y] == '*') 
                    {
                        Gear g = new Gear();
                        g.position = new Vector2I(x, y);
                        gears.Add(g);
                    }
                }
            }
        }



        public void SumGearRatios()
        {
            foreach (var gear in gears)
            {
                foreach (var part in parts)
                {
                    if (Gear.Overlap(gear, part))
                    {
                        Console.WriteLine($"Gear at {gear.position} overlaps Part at {part.position}");
                        gear.Parts.Add(part);
                    }
                }
            }

            int ratioSum = 0;
            foreach (var gear in gears)
            {
                if(gear.Parts.Count == 2)
                {
                    int ratio = -1;
                    foreach (var part in gear.Parts)
                    {
                        if (ratio == -1)
                        {
                            ratio = part.partNumber;
                        }
                        else
                        {
                            ratio *= part.partNumber;
                        }
                    }
                    ratioSum += ratio;
                }
            }

            Console.WriteLine($"Gear Ratio Sum: {ratioSum}");
        }
    }
}
