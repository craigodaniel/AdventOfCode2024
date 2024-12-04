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
    }
}
