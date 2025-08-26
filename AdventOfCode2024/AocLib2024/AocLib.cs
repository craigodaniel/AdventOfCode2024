using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.AocLib2024
{
    public class AocLib
    {
        public static string[] CleanInput(string[] inputArray)
        {
            for (int i = 0; i < inputArray.Length; i++)
            {
                inputArray[i] = inputArray[i].Trim();
            }

            return inputArray;
        }

        public static int[] GetIntArrayFromString(string inputString)
        {
            string[] stringArray = inputString.Split(" ");
            int[] intArray = new int[stringArray.Length];

            for (int i = 0; i < stringArray.Length; i++)
            {
                try
                {
                    intArray[i] = int.Parse(stringArray[i]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return intArray;
        }

        public static int[] GetIntArrayFromString(string inputString, string delimeter)
        {
            string[] stringArray = inputString.Split(delimeter);
            int[] intArray = new int[stringArray.Length];

            for (int i = 0; i < stringArray.Length; i++)
            {
                try
                {
                    intArray[i] = int.Parse(stringArray[i]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return intArray;
        }

        public static int[] GetIntArrayFromCharArray(string inputString)
        {
            char[] chars = inputString.ToCharArray();
            int[] intArray = new int[chars.Length];

            for (int i = 0; i < chars.Length; i++)
            {
                try
                {
                    intArray[i] = int.Parse(chars[i].ToString());
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return intArray;
        }

        public static char[,] GetChar2dArrayFromStringArray(string[] inputs)
        {
            char[,] chars = new char[inputs.Length, inputs[0].Length];

            for (int row = 0; row < inputs.Length; row++)
            {
                for (int col = 0; col < inputs[row].Length; col++)
                {
                    chars[row, col] = inputs[row][col];
                }
            }

            return chars;
        }

        public static List<int> GetListOfIntsFromString(string inputString, string delimeter)
        {
            List<int> list = new List<int>();
            string[] stringArray = inputString.Split(delimeter);

            foreach (string str in stringArray)
            {
                try
                {
                    list.Add(int.Parse(str));
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return list;
        }

        public static List<long> GetListOfLongsFromString(string inputString, string delimeter)
        {
            List<long> list = new List<long>();
            string[] stringArray = inputString.Split(delimeter);

            foreach (string str in stringArray)
            {
                try
                {
                    list.Add(long.Parse(str));
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return list;
        }

        public static int[,] GetInt2dArrayFromStringArray(string[] inputs)
        {
            int[,] ints = new int[inputs.Length, inputs[0].Length];
            for (int row = 0; row < inputs.Length; row++)
            {
                for (int col = 0; col < inputs[row].Length; col++)
                {
                    try
                    {
                        ints[row, col] = int.Parse(inputs[row][col].ToString());
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return ints;
        }
    }
}
