using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MHS
{
    public class MemoryHierarchySimulator
    {
        //Initial Setup of properties.
        public Dictionary<string, string> cache = new Dictionary<string, string>();

        public List<MemoryAddress> memoryAddresses = new List<MemoryAddress>();

        public List<string> memoryReferences = new List<string>();
        public int totalHits { get; set; }
        public int totalMisses { get; set; }
        public int readAccesses { get; set; }
        public int writeAccesses { get; set; }
        public int capacityOfCache { get; set; }

        /// <summary>
        /// Default Constructor that sets properties to 0.
        /// </summary>
        public MemoryHierarchySimulator()
        {
            totalHits = 0;
            totalMisses = 0;
            readAccesses = 0;
            writeAccesses = 0;
            capacityOfCache = 4;
        }

        /// <summary>
        /// Reads the memory trace and converts the input file to a list of memory addresses.
        /// </summary>
        /// <param name="trace">The path of the trace to be read.</param>
        /// <returns>A list of strings of memory addresses.</returns>
        public List<string> ReadMemoryTrace(string trace)
        {
            List<string> memoryAddresses = new List<string>();

            using (StreamReader rdr = new StreamReader(@$"..\..\..\{trace}"))
            {
                //int i = 0;
                string line;
                string[] inputLines;


                while ((line = rdr.ReadLine()) != null)
                {
                    inputLines = line.Split(":");
                    memoryAddresses.Add(inputLines[0]);
                    memoryAddresses.Add(inputLines[1]);
                    //i += 2;
                }

                rdr.Close();
            }

            return memoryAddresses;
        }

        /// <summary>
        /// Parses a list of strings of memory address into a list of memory address objects.
        /// </summary>
        /// <param name="addresses">A list of strings of memory addresses.</param>
        /// <returns>A list of memory address objects.</returns>
        public List<MemoryAddress> ParseMemoryAddress(List<string> addresses)
        {
            for (int i = 0; i < addresses.Count; i += 2)
            {
                char accessType;
                string pageNumber;
                string pageOffset;

                accessType = char.Parse(addresses[i]);
                pageNumber = addresses[i + 1][0].ToString();
                pageOffset = addresses[i + 1].Substring(1);

                MemoryAddress memoryAddress = new MemoryAddress(accessType, pageNumber, pageOffset);

                memoryAddresses.Add(memoryAddress);
            }

            return memoryAddresses;
        }

        /// <summary>
        /// Takes in a memory address object and uses it to update the statistics of the simulator.
        /// </summary>
        /// <param name="memoryAddress">A memory address object.</param>
        public void UpdateStatistics(MemoryAddress memoryAddress)
        {
            string memoryReference = $"{memoryAddress.virtualPageNumber} , {memoryAddress.virtualPageOffset} , {memoryAddress.physicalPageNumber} , {memoryAddress.physicalPageOffset}\n";

            memoryReferences.Add(memoryReference);

            if (memoryAddress.isHit)
            {
                totalHits++;
            }
            else
            {
                totalMisses++;
            }

            if (memoryAddress.accessType == 'R')
            {
                readAccesses++;
            }
            else if (memoryAddress.accessType == 'W')
            {
                writeAccesses++;
            }
            Console.WriteLine(memoryReference);
        }

        //Pretty much just copied this from the old code. Maybe needs work.
        /// <summary>
        /// Creates a string of summary statistic information.
        /// </summary>
        /// <returns>A string of summary statistic information.</returns>
        public string DisplaySummaryStatistics()
        {
            string info = string.Empty;

            info += "\nVirtual Page Number, Virtual Page Offset, Physical Page Number, Physical Page Offset\n";

            //foreach (string reference in memoryReferences)
            //{
            //    info += reference;
            //}

            info += "\nSummary Statistics:\n";
            info += $"Total Hits: {totalHits}\n";
            info += $"Total Misses: {totalMisses}\n";
            info += $"Hit Ratio: {Math.Round(totalHits / (double)totalMisses, 2)}\n";
            info += $"Hit Percentage: {Math.Round(totalHits / (double)(totalHits + totalMisses)*100, 2)}%\n";
            info += $"Number of Read Accesses: {readAccesses}\n";
            info += $"Number of Write Accesses: {writeAccesses}\n";
            if (writeAccesses == 0)
            {
                info += $"Read/Write Ratio: {readAccesses}:{writeAccesses}\n";
            }
            else
            {
                info += $"Read/Write Ratio: {Math.Round(readAccesses / (double)writeAccesses, 2)}\n";
            }
            info += $"Total Number of Memory References: {readAccesses + writeAccesses}\n";

            return info;
        }

        public void ResetValues()
        {
            cache.Clear();
            memoryAddresses.Clear();
            memoryReferences.Clear();
            totalHits = 0;
            totalMisses = 0;
            readAccesses = 0;
            writeAccesses = 0;
        }
    }


    /*
    internal class MemoryHierarchySimulator
    {
        static Dictionary<int, int> pageTable = new Dictionary<int, int>();
        static LinkedList<int> lruQueue = new LinkedList<int>();
        static Queue<int> fifoQueue = new Queue<int>();

        static int totalHits = 0;
        static int totalMisses = 0;
        static int readAccesses = 0;
        static int writeAccesses = 0;


        public void ProcessMemoryTrace(string line, Func<int, bool> algorithm)
        {
            string[] parts = line.Split(':');
            char accessType = parts[0][0];
            int virtualAddress = Convert.ToInt32(parts[1], 16);

            int virtualPageNumber = virtualAddress >> 12;
            int pageOffset = virtualAddress & 0xFFF;

            bool isHit = algorithm(virtualPageNumber);

            Console.WriteLine($"{virtualAddress:X8} {virtualPageNumber:X6} {pageOffset:X4} {pageTable[virtualPageNumber]:X4}");

            UpdateStatistics(isHit, accessType);
        }

        public void OptimalGreedyAlgorithm()
        {
            Console.WriteLine("Simulating Optimal Greedy Algorithm");

            // Read memory traces from a file
            // needs to be a relative file path
            string[] lines = File.ReadAllLines(@"..\..\..\trunc_12.dat");

            foreach (string line in lines)
            {
                ProcessMemoryTrace(line, OptimalGreedyAlgorithm);
            }
        }

        public bool OptimalGreedyAlgorithm(int virtualPageNumber)
        {
            if (pageTable.ContainsKey(virtualPageNumber))
            {
                // Existing page, it's a hit
                return true;
            }
            else
            {
                // Page fault - update page table
                pageTable[virtualPageNumber] = pageTable.Count;
                return false;
            }
        }

        public bool OptimalFIFOAlgorithm(int virtualPageNumber)
        {
            bool isHit = pageTable.ContainsKey(virtualPageNumber);

            if (!isHit)
            {
                // Page fault - update page table and FIFO queue
                pageTable[virtualPageNumber] = pageTable.Count;
                fifoQueue.Enqueue(virtualPageNumber);
            }

            return isHit;
        }

        public bool OptimalLRUAlgorithm(int virtualPageNumber)
        {
            bool isHit = lruQueue.Contains(virtualPageNumber);

            if (!isHit)
            {
                // Page fault - update page table and LRU queue
                pageTable[virtualPageNumber] = pageTable.Count;
                lruQueue.RemoveLast(); // Remove the least recently used page
                lruQueue.AddFirst(virtualPageNumber);
            }
            else
            {
                // Update the order in the LRU queue for hits
                lruQueue.Remove(virtualPageNumber);
                lruQueue.AddFirst(virtualPageNumber);
            }

            return isHit;
        }

        public void UpdateStatistics(bool isHit, char accessType)
        {
            if (isHit)
                totalHits++;
            else
                totalMisses++;

            if (accessType == 'R')
                readAccesses++;
            else if (accessType == 'W')
                writeAccesses++;
        }

        public void DisplaySummaryStatistics()
        {
            Console.WriteLine("\nSummary Statistics:");
            Console.WriteLine($"Total Hits: {totalHits}");
            Console.WriteLine($"Total Misses: {totalMisses}");
            Console.WriteLine($"Hit Ratio: {totalHits / (double)totalMisses}");
            Console.WriteLine($"Number of Read Accesses: {readAccesses}");
            Console.WriteLine($"Number of Write Accesses: {writeAccesses}");
            Console.WriteLine($"Read/Write Ratio: {readAccesses / (double)writeAccesses}");
            Console.WriteLine($"Total Number of Memory References: {readAccesses + writeAccesses}");
        }
    }
    */

}

