using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    internal class FIF : ICachingAlgorithm
    {
        public static void Run(MemoryHierarchySimulator mhs)
        {
            List<string> accessOrder = new();

            string[] cache = new string[mhs.capacityOfCache];
            string[] cacheBuffer = new string[mhs.capacityOfCache];  //will add to this if something on the current cache is found in the future and needed?

            for (int i = 0; i < mhs.memoryAddresses.Count; i++)
            {
                // need to check when you get near the end of the list so that we do not go out of bounds when checking the future
                MemoryAddress addr = mhs.memoryAddresses[i];
                if (cache.Contains(addr.virtualPageNumber))
                {
                    addr.isHit = true;

                }
                else
                {
                    addr.isHit = false;
                    if (mhs.cache.Count >= mhs.capacityOfCache)
                    {
                        // TODO add to this for evicition method

                    }
                    // TODO add here
                }
                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                addr.physicalPageOffset = addr.virtualPageOffset; //can do this in constructor
                mhs.UpdateStatistics(addr);
            }
            Console.WriteLine(mhs.DisplaySummaryStatistics());
        }
    }
}