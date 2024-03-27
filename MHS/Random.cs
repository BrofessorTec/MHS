using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    internal class Random : ICachingAlgorithm
    {
        private static System.Random random = new System.Random();

        public static void Run(MemoryHierarchySimulator mhs)
        {
            List<string> accessOrder = new List<string>();
            string[] cache = new string[mhs.capacityOfCache];

            foreach (MemoryAddress addr in mhs.memoryAddresses)
            {
                if (cache.Contains(addr.virtualPageNumber))
                {
                    addr.isHit = true;
                }
                else
                {
                    addr.isHit = false;

                    if (mhs.cache.Count >= mhs.capacityOfCache)
                    {
                        // Cache is full, randomly select a page to evict
                        int randomIndex = random.Next(accessOrder.Count); // Use the count of elements in accessOrder
                        string pageToEvict = accessOrder[randomIndex];
                        int cacheIndex = Array.IndexOf(cache, pageToEvict); // Get the index of the page in cache
                        cache[cacheIndex] = null; // Evict the randomly selected page from cache
                        mhs.cache.Remove(pageToEvict);
                        accessOrder.RemoveAt(randomIndex); // Remove the randomly selected page from accessOrder
                    }

                    int index = Array.IndexOf(cache, null);
                    cache[index] = addr.virtualPageNumber;
                    accessOrder.Add(addr.virtualPageNumber);
                    mhs.cache[addr.virtualPageNumber] = (index + 1).ToString();
                }

                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                addr.physicalPageOffset = addr.virtualPageOffset;
                mhs.UpdateStatistics(addr);
            }
        }
    }
}
