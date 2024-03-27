using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    internal class FIF : ICachingAlgorithm
    {
        public static void Run(MemoryHierarchySimulator mhs)
        {
            List<string> cacheBuffer = new List<string>(mhs.capacityOfCache);
            int index = 0;
            int indexToRemove = 0;
            int initialIndex = 0;

            for (int i = 0; i < mhs.memoryAddresses.Count; i++)
            {
                // need to check when you get near the end of the list so that we do not go out of bounds when checking the future
                MemoryAddress addr = mhs.memoryAddresses[i];
                if (mhs.cache.ContainsKey(addr.virtualPageNumber))
                {
                    addr.isHit = true;

                }
                else
                {
                    addr.isHit = false;
                    if (mhs.cache.Count >= mhs.capacityOfCache)
                    {

                        //remove from cache the one that is not needed in the future
                        int j = 1;
                        cacheBuffer.Clear();
                        // can try to reset the cachebuffer here before each loop. probably more efficient ways to do this

                        while (cacheBuffer.Count < (mhs.capacityOfCache - 1) && (i + j) < (mhs.memoryAddresses.Count() - mhs.capacityOfCache - 1))
                        {     // attemping to run this until near the end of the cache, probably need to change the if statements around
                            if (mhs.cache.ContainsKey(mhs.memoryAddresses[i + j].virtualPageNumber) && !cacheBuffer.Contains(mhs.memoryAddresses[i + j].virtualPageNumber))
                            {
                                //add to cache buffer if it is used in future
                                cacheBuffer.Add(mhs.memoryAddresses[i + j].virtualPageNumber);
                            }
                            j++; 
                        }
                        // might need another statement to check the last elements of the memAddresses when close to the end?

                        // remove from the real cache
                        foreach (string key in mhs.cache.Keys)
                        {
                            if (!cacheBuffer.Contains(key))
                            {
                                indexToRemove = int.Parse(mhs.cache[key]);
                                mhs.cache.Remove(key);
                                break;
                                //intending for this to just remove the first item that is not in the cachebuffer and then exit the loop
                            }
                        }
                    }

                    if (initialIndex < mhs.capacityOfCache)
                    {
                        index++;
                        initialIndex++;
                    }
                    else
                    {
                        index = indexToRemove;
                    }
                    mhs.cache.Add(addr.virtualPageNumber, index.ToString());
                }
                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                addr.physicalPageOffset = addr.virtualPageOffset; //can do this in constructor
                mhs.UpdateStatistics(addr);
            }
        }
    }
}