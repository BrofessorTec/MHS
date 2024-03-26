using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    internal class Random : ICachingAlgorithm
    {
        private static System.Random random = new System.Random();

        public static void Run(MemoryHierarchySimulator mhs)
        {
            // Initialize cache
            Dictionary<string, string> cache = new Dictionary<string, string>();

            foreach (MemoryAddress addr in mhs.memoryAddresses)
            {
                // Check if the virtual page is in the cache
                if (cache.ContainsKey(addr.virtualPageNumber))
                {
                    // Cache hit
                    addr.isHit = true;
                }
                else
                {
                    // Cache miss
                    addr.isHit = false;

                    if (cache.Count >= mhs.capacityOfCache)
                    {
                        // Cache is full, randomly select a page to evict
                        int randomIndex = random.Next(cache.Count);
                        string pageToEvict = cache.Keys.ElementAt(randomIndex);
                        cache.Remove(pageToEvict); // Evict the randomly selected page
                    }

                    // Add the new page to the cache
                    cache[addr.virtualPageNumber] = addr.virtualPageNumber;
                }

                // Update physical page number
                addr.physicalPageNumber = mhs.cache.GetValueOrDefault(addr.virtualPageNumber);
                addr.physicalPageOffset = addr.virtualPageOffset;

                // Update statistics
                mhs.UpdateStatistics(addr);
            }
        }
    }
}
