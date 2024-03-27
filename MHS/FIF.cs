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
            List<string> cacheBuffer = new List<string>(4);  //will add to this if something on the current cache is found in the future and needed?
            //int cacheBufferCount = 0; // probably not needed now for a list
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
                        /*
                        cache[Array.IndexOf(cache, accessOrder[0])] = null;
                        mhs.cache.Remove(accessOrder[0]);
                        accessOrder.RemoveAt(0);
                        */

                        //remove from cache the one that is not needed in the future
                        int j = 1;
                        cacheBuffer.Clear();
                        /*foreach (string key in mhs.cache.Keys)
                        {
                            // adds current cache to check if it is in the future?
                            cacheBuffer.Add(key);
                        }*/
                        //cacheBufferCount = 0;
                        // can try to reset the cachebuffer here before each loop

                        while (cacheBuffer.Count < (mhs.capacityOfCache - 1) && (i + j) < (mhs.memoryAddresses.Count() - mhs.capacityOfCache - 1))
                        {     // attemping to run this until near the end of the cache, probably need to change the if statements around
                            if (mhs.cache.ContainsKey(mhs.memoryAddresses[i + j].virtualPageNumber) && !cacheBuffer.Contains(mhs.memoryAddresses[i + j].virtualPageNumber))
                            {
                                //add to cache buffer if it is used in future
                                cacheBuffer.Add(mhs.memoryAddresses[i + j].virtualPageNumber);
                            }
                            j++; 
                        }
                        // might need another statement to check the last elements of the memAddresses when close to the end

                        // remove from the real cache
                        // is this the right "in" for the foreach?
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
                    /*if (cacheBuffer.Contains("-1"))
                    { // maybe not needed now with list?
                        index = cacheBuffer.ToList().IndexOf("-1");
                    }*/
                    if (initialIndex < mhs.capacityOfCache)
                    {
                        /*if (index == mhs.capacityOfCache)
                        {
                            index = 1;
                            initialIndex++;
                        }
                        else
                        {
                            index++;
                            initialIndex++;
                        }*/
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
            Console.WriteLine(mhs.DisplaySummaryStatistics());
        }
    }
}