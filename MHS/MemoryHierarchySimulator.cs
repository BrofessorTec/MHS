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
        public int pageLength { get; set; }
        public int totalHits { get; set; }
        public int totalMisses { get; set; }
        public int readAccesses { get; set; }
        public int writeAccesses { get; set; }
        public int capacityOfCache { get; set; }
        public string algoName {  get; set; }

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
            pageLength = 1;
        }

        /// <summary>
        /// Reads and Parses the memory trace and converts the input file to a list of memory addresses.
        /// </summary>
        /// <param name="trace">The path of the trace to be read.</param>
        /// <returns>A list of strings of memory addresses.</returns>
        public void ReadMemoryTrace(string trace) {
            using (StreamReader rdr = new StreamReader(@$"..\..\..\{trace}")) {                
                while (!rdr.EndOfStream) {                  
                    string line = rdr.ReadLine();
                    string[] input = line.Split(":");                   
                    char accessType = char.Parse(input[0]);
                    string pageNumber = input[1].Length >= pageLength ? input[1].Substring(0, pageLength) : input[1];
                    string pageOffset = input[1].Length > pageLength ? input[1].Substring(pageLength) : "";
                    MemoryAddress mA = new MemoryAddress(accessType, pageNumber, pageOffset);
                    memoryAddresses.Add(mA);
                }
            }
        }

        /// <summary>
        /// Takes in a memory address object and uses it to update the statistics of the simulator.
        /// </summary>
        /// <param name="memoryAddress">A memory address object.</param>
        public void UpdateStatistics(MemoryAddress memoryAddress)
        {
            string hitormiss = "";
            if (memoryAddress.isHit) {
                totalHits++;
                hitormiss = "Hit";
            } else {
                totalMisses++;
                hitormiss = "Miss";
            }
            
            if (memoryAddress.accessType == 'R')
                readAccesses++;
            else if (memoryAddress.accessType == 'W')
                writeAccesses++;            

            string memoryReference = $"{memoryAddress.accessType}:{memoryAddress.virtualPageNumber}{memoryAddress.virtualPageOffset} , {memoryAddress.virtualPageNumber} , {memoryAddress.virtualPageOffset} , {memoryAddress.physicalPageNumber} , {memoryAddress.physicalPageOffset}, {hitormiss}\n";

            memoryReferences.Add(memoryReference);
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
            info += "\nTrace, Virtual Page Number, Virtual Page Offset, Physical Page Number, Physical Page Offset, hit/miss\n";
            info += "\nSummary Statistics:\n";
            info += $"Algorithm: {algoName}\n";
            info += $"Cache Size: {capacityOfCache}\n";
            info += $"Total Hits: {totalHits}\n";
            info += $"Total Misses: {totalMisses}\n";
            info += $"Hit/Miss Ratio: {Math.Round(totalHits / (double)totalMisses, 2)}\n";
            info += $"Hit Percentage: {Math.Round(totalHits / (double)(totalHits + totalMisses)*100, 2)}%\n";
            info += $"Miss Percentage: {Math.Round(totalMisses / (double)(totalHits + totalMisses) * 100, 2)}%\n";
            info += $"Number of Read Accesses: {readAccesses}\n";
            info += $"Number of Write Accesses: {writeAccesses}\n";

            if (writeAccesses == 0)            
                info += $"Read/Write Ratio: {readAccesses}:{writeAccesses}\n";            
            else            
                info += $"Read/Write Ratio: {Math.Round(readAccesses / (double)writeAccesses, 2)}\n";
            
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
}
