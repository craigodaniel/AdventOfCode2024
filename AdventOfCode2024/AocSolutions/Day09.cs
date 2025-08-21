using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day09
    {
        // --- Day 9: Disk Fragmenter ---
        // https://adventofcode.com/2024/day/9
        //
        // Part 1 runtime: 579.2069ms. The answer is: 6307275788409
        // Part 2 runtime: 1290.1249ms. The answer is: 6327174563252
        //
        // Comments: 
        // First try Part 1 runtime: 579.6445ms. The answer is: -2031169415. 
        // Second try changed answer from int to long Part 1 runtime: 579.2069ms. The answer is: 6307275788409
        // Part 2 first try 687736244 too low. changed answer from int to long. Correct.

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day09_sample.txt";
        //private static string fileName = "day09_sample_2.txt";
        private static string fileName = "day09_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            long answer = 0;
            int fileId = 0;
            int position = 0;
            List<int> freespace = new List<int>();
            List<int> occspace = new List<int>();
            var fileBlocks = new List<int[]>();
            int[] diskmap = AocLib.GetIntArrayFromCharArray(lines[0]);
            
            
            for (int i = 0;  i < diskmap.Length; i++)
            {
                if (i % 2 == 0) //map occ blocks
                {
                    int occLen = diskmap[i];
                    for (int j = position; j < position + occLen; j++)
                    {
                        occspace.Add(j);
                        fileBlocks.Add(new int[2]{ fileId, j});
                    }
                    position += occLen;
                    fileId++;
                }
                else //map free blocks
                {
                    int freeLen = diskmap[i];
                    for (int j = position;j < position + freeLen; j++)
                    {
                        freespace.Add(j);
                    }
                    position += freeLen;
                }
            }

            int fileToMoveIndex = fileBlocks.Count - 1;
            
            while (freespace.Count > 0 && fileToMoveIndex > 0)
            {
                int[] fileToMove = fileBlocks[fileToMoveIndex];
                if (fileToMove[1] > freespace[0])
                {
                    fileToMove[1] = freespace[0];
                    freespace.RemoveAt(0);
                }                
                fileToMoveIndex--;
            }

            foreach (int[] file in fileBlocks)
            {
                answer += file[0] * file[1];
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            long answer = 0;
            int fileId = 0;
            int position = 0;
            var fileBlocks = new List<FileBlock>();
            int[] diskmap = AocLib.GetIntArrayFromCharArray(lines[0]);


            //Read Diskmap and create file blocks
            for (int i = 0; i < diskmap.Length; i++)
            {
                int length = diskmap[i];

                if (i % 2 == 0) //map occupied blocks
                {
                    fileBlocks.Add(new FileBlock(fileId, position, length));
                    fileId++;
                }
                else //map free blocks
                {
                    fileBlocks.Add(new FileBlock(null, position, length));
                }

                position += length;
            }

            //Move files back to front 
            for (int j = fileId - 1; j > 0; j--)
            {
                foreach (FileBlock file in fileBlocks)
                {
                    if (file.ID == j)
                    {
                        int freeIndex = FindFirstOpenSlot(file.Length, fileBlocks);


                        if (freeIndex != -1 && freeIndex < file.StartIndex) //found free space
                        {
                            int freeFileBlockIndex = fileBlocks.IndexOf(fileBlocks.FirstOrDefault(f => f.ID is null && f.StartIndex == freeIndex));
                            FileBlock freeFileBlock = fileBlocks[freeFileBlockIndex];

                            file.StartIndex = freeIndex;
                            file.EndIndex = freeIndex + file.Length - 1; //update end index of file block

                            //adjust remaining free block if any
                            if (freeIndex + file.Length - 1 < freeFileBlock.EndIndex) //if there is still free space after the file
                            {
                                int remainingFreeLength = freeFileBlock.EndIndex - (freeIndex + file.Length) + 1;
                                fileBlocks[freeFileBlockIndex].StartIndex = freeIndex + file.Length; //update start index of free block
                                fileBlocks[freeFileBlockIndex].Length = remainingFreeLength; //update length of free block
                                //fileBlocks[freeFileBlockIndex].EndIndex = freeIndex + file.Length + remainingFreeLength - 1; //update end index of free block
                            }
                            else
                            {
                                fileBlocks[freeFileBlockIndex].StartIndex = -1;
                                fileBlocks[freeFileBlockIndex].Length = 0;
                                fileBlocks[freeFileBlockIndex].EndIndex = -1;//remove free block if no remaining space
                            }
                            //fileBlocks.Remove(file); //remove old file block
                        }
                        else
                        {
                            //Console.WriteLine("No free space found for file ID: {0}", j);
                        }
                    }
                }
            }


            //Calculate check sum

               
            foreach (FileBlock file in fileBlocks)
            {
                if (file.ID is not null)
                {
                    for (int i = file.StartIndex; i <= file.EndIndex; i++)
                        {
                            //Console.Write(file.ID.ToString() + " * " + i.ToString() + "\n");
                            answer += (int)file.ID * i;
                        }
                }

            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
                Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
         
        }


        private class FileBlock
        {
            public int? ID;
            public int StartIndex;
            public int Length;
            public int EndIndex;

            public FileBlock(int? ID, int StartIndex, int Length)
            {
                if (ID is null)
                {
                    this.ID = null;
                }
                else
                {
                    this.ID = ID;
                }
                this.StartIndex = StartIndex;
                this.Length = Length;
                this.EndIndex = StartIndex + Length - 1;
            }
        }

        private static int FindFirstOpenSlot(int fileLen, List<FileBlock> fileBlocks)
        {
            for (int i = 0; i < fileBlocks.Count; i++)
            {
                if (fileBlocks[i].ID is null && fileBlocks[i].Length >= fileLen)
                {
                    return fileBlocks[i].StartIndex;
                }
            }

            return -1;
        }
    }
}