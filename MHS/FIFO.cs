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
                if (queue.Count < mhs.capacityOfCache)
                {
                    if (mhs.cache.ContainsKey(address.virtualPageNumber))
                    {
                        address.isHit = true;
                        address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                        //address.physicalPageOffset = address.virtualPageOffset;  // i think we could just do this in the constructor and skip all of these physoffset = virtoffsets?
                    }
                    else
                    {
                        address.isHit = false;

                        index++;

                        mhs.cache.Add(address.virtualPageNumber, index.ToString());
                        queue.Enqueue(address.virtualPageNumber);

                        address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                        //address.physicalPageOffset = address.virtualPageOffset;
                    }
                }

                else if (mhs.cache.ContainsKey(address.virtualPageNumber))
                {
                    address.isHit = true;
                    address.physicalPageNumber = mhs.cache.GetValueOrDefault(address.virtualPageNumber);
                    //address.physicalPageOffset = address.virtualPageOffset;
                }
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
                    //address.physicalPageOffset = address.virtualPageOffset;
                }

                mhs.UpdateStatistics(address);
            }
        }
    }
}
