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
            //string[] cache = new string[mhs.capacityOfCache];
            string[] cacheBuffer = new string[mhs.capacityOfCache];  //will add to this if something on the current cache is found in the future and needed?
            int index = 0;

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
                        /*
                        cache[Array.IndexOf(cache, accessOrder[0])] = null;
                        mhs.cache.Remove(accessOrder[0]);
                        accessOrder.RemoveAt(0);
                        */

                        //remove from cache the one that is not needed in the future
                        int j = 1;
                        cacheBuffer = new string[mhs.capacityOfCache];
                        // can try to reset the cachebuffer here before each loop
                        if (index == mhs.capacityOfCache)
                        {
                            index = 1;
                        }
                        else
                        {
                            index++;
                        }

                        while (cacheBuffer.Count() < mhs.capacityOfCache && i < (mhs.memoryAddresses.Count() - mhs.capacityOfCache))
                        {     // attemping to run this until near the end of the cache, probably need to change the if statements around
                            if (mhs.cache.ContainsKey(mhs.memoryAddresses[i + j].virtualPageNumber))
                            {
                                //add to cache buffer if it is used in future
                                cacheBuffer.Append(mhs.memoryAddresses[i + j].virtualPageNumber);
                            }
                            j++; 
                        }
                        // remove from the real cache
                        foreach (MemoryAddress address in mhs.memoryAddresses)
                        {
                            if (!cacheBuffer.Contains(address.virtualPageNumber))
                            {
                                mhs.cache.Remove(address.virtualPageNumber);
                                break;
                                //intending for this to just remove the first item that is not in the cachebuffer and then exit the loop
                            }
                        }

                        // could add a line if there is no change in the cache buffer then to not clear it and keep it until the end if values are the same
                        // or near the end of the addresses, skips some checking?

                    }
                    /*
                    int index = Array.IndexOf(cache, null);
                    cache[index] = addr.virtualPageNumber;
                    accessOrder.Add(addr.virtualPageNumber);
                    mhs.cache[addr.virtualPageNumber] = (index + 1).ToString();
                    */
                    // add the new item to the cache
                    mhs.cache.Add(addr.virtualPageNumber, index.ToString());
                }
                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                addr.physicalPageOffset = addr.virtualPageOffset; //can do this in constructor
                mhs.UpdateStatistics(addr);
            }
            Console.WriteLine(mhs.DisplaySummaryStatistics());
        }
    }
}