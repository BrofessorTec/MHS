using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS {
    internal class LRU : ICachingAlgorithm {
        public static void Run(MemoryHierarchySimulator mhs) {
            List<string> accessOrder = new List<string>();
            foreach (MemoryAddress addr in mhs.memoryAddresses) {
                if (mhs.cache.ContainsKey(addr.virtualPageNumber)) {
                    addr.isHit = true;
                    accessOrder.Remove(addr.virtualPageNumber);
                    accessOrder.Add(addr.virtualPageNumber);
                } else {
                    addr.isHit = false;
                    if (mhs.cache.Count >= mhs.capacityOfCache) {
                        mhs.cache.Remove(accessOrder[0]);
                        accessOrder.RemoveAt(0);
                    }
                    mhs.cache[addr.virtualPageNumber] = (accessOrder.Count + 1).ToString();
                    accessOrder.Add(addr.virtualPageNumber);
                }
                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                addr.physicalPageOffset = addr.virtualPageOffset;
                mhs.UpdateStatistics(addr);
            }
            Console.WriteLine(mhs.DisplaySummaryStatistics());
        }
    }
}
