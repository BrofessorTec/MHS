using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    internal class Greedy : ICachingAlgorithm
    {
        public static void Run(MemoryHierarchySimulator mhs)
        {
            Dictionary<string, int> cache = new Dictionary<string, int>();
            Dictionary<string, int> futureAccess = new Dictionary<string, int>();

            // Step 1: Analyze future access patterns
            for (int i = 0; i < mhs.memoryAddresses.Count; i++)
            {
                MemoryAddress address = mhs.memoryAddresses[i];
                if (!futureAccess.ContainsKey(address.virtualPageNumber))
                    futureAccess.Add(address.virtualPageNumber, i);
            }

            int index = 0; // Initialize the index

            foreach (MemoryAddress address in mhs.memoryAddresses)
            {
                if (cache.ContainsKey(address.virtualPageNumber))
                {
                    address.isHit = true;
                    address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                    address.physicalPageOffset = address.virtualPageOffset;
                }
                else
                {
                    address.isHit = false;

                    if (cache.Count >= mhs.capacityOfCache)
                    {
                        // Find the page to evict (the one with the lowest insertion order)
                        //Essentially LRU
                        string pageToEvict = null;
                        int minInsertionOrder = int.MaxValue;

                        foreach (var kvp in cache)
                        {
                            if (kvp.Value < minInsertionOrder)
                            {
                                minInsertionOrder = kvp.Value;
                                pageToEvict = kvp.Key;
                            }
                        }

                        // Ensure we found a page to evict
                        if (pageToEvict != null)
                        {
                            cache.Remove(pageToEvict);
                        }
                    }

                    // Add the accessed page to the cache with the current index
                    cache[address.virtualPageNumber] = index;

                    // Increment the index for the next page
                    index++;
                }

                // Update the address information
                address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                address.physicalPageOffset = address.virtualPageOffset;

                // Update statistics
                mhs.UpdateStatistics(address);

                // Print summary statistics
                Console.WriteLine(mhs.DisplaySummaryStatistics());
            }
        }
    }
}