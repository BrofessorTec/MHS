using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS {
    internal class LRU : ICachingAlgorithm {
        public static void Run(MemoryHierarchySimulator mhs) {
            List<string> accessOrder = new List<string>();
            string[] cache = new string[mhs.capacityOfCache];
            foreach (MemoryAddress addr in mhs.memoryAddresses) {
                if (cache.Contains(addr.virtualPageNumber)) {
                    addr.isHit = true;
                    // constantly removes the oldest element and renews it if it repeats so that it is considered recently used
                    accessOrder.Remove(addr.virtualPageNumber);
                    accessOrder.Add(addr.virtualPageNumber);                    
                } else {
                    addr.isHit = false;
                    if (mhs.cache.Count >= mhs.capacityOfCache) {
                        cache[Array.IndexOf(cache, accessOrder[0])] = null;
                        mhs.cache.Remove(accessOrder[0]);
                        accessOrder.RemoveAt(0);
                    }
                    int index = Array.IndexOf(cache, null);
                    cache[index] = addr.virtualPageNumber;
                    accessOrder.Add(addr.virtualPageNumber);
                    mhs.cache[addr.virtualPageNumber] = (index + 1).ToString();
                }
                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                mhs.UpdateStatistics(addr);
            }
        }
    }
}
