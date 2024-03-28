using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    public class FIFO : ICachingAlgorithm
    {
        public static void Run(MemoryHierarchySimulator mhs)
        {
            Queue<string> queue = new Queue<string>();
            int index = 0;

            foreach (MemoryAddress address in mhs.memoryAddresses)
            {
                //Initial Filling of the Cache.
                if (queue.Count < mhs.capacityOfCache)
                {
                    //If the address is in the cache, mark it as a hit.
                    if (mhs.cache.ContainsKey(address.virtualPageNumber))
                    {
                        address.isHit = true;
                        address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                    }
                    //If the address is not in the cache, add it to the cache and the queue.
                    else
                    {
                        address.isHit = false;

                        index++;

                        mhs.cache.Add(address.virtualPageNumber, index.ToString());
                        queue.Enqueue(address.virtualPageNumber);

                        address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                    }
                }

                //Algorithm Running once Cache is at capacity.
                //If the address is in the cache, mark it as a hit.
                else if (mhs.cache.ContainsKey(address.virtualPageNumber))
                {
                    address.isHit = true;
                    address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                }
                //If the address is not in the cache, figure out what to remove using from the cache using the queue then add it to the cache and the queue.
                else
                {
                    address.isHit = false;

                    if (index == mhs.capacityOfCache)
                    {
                        index = 1;
                    }
                    else
                    {
                        index++;
                    }

                    mhs.cache.Remove(queue.Dequeue());
                    mhs.cache.Add(address.virtualPageNumber, index.ToString());
                    queue.Enqueue(address.virtualPageNumber);

                    address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                }

                mhs.UpdateStatistics(address);
            }
        }
    }
}
