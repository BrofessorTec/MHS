using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS {
    internal class LRU : ICachingAlgorithm {
        public static void Run(MemoryHierarchySimulator mhs) {
            List<string> accessOrder = new List<string>();
            string[] cache = new string[3]; //Dictionaries dont have an index so using this
            foreach (MemoryAddress addr in mhs.memoryAddresses) {
                if (cache.Contains(addr.virtualPageNumber)) {
                    addr.isHit = true;
                    /* constantly removes the oldest element and renews it if it repeats so that it is considered recently used
                     * ex: b, d, 6 -> d, 6, f
                     * ^ d and 6 are shifted left since the the b is removed, f (the most recently used) is added onto the end.
                     * ex2: b -> if the next element is also b, the b is removed and the new b is added
                     */
                    accessOrder.Remove(addr.virtualPageNumber);
                    accessOrder.Add(addr.virtualPageNumber);                    
                } else {
                    addr.isHit = false;
                    if (mhs.cache.Count >= mhs.capacityOfCache) {
                        cache[Array.IndexOf(cache, accessOrder[0])] = null;
                        mhs.cache.Remove(accessOrder[0]);
                        accessOrder.RemoveAt(0);
                    }
                    int correctIndex = Array.IndexOf(cache, null);
                    cache[correctIndex] = addr.virtualPageNumber;
                    accessOrder.Add(addr.virtualPageNumber);
                    mhs.cache[addr.virtualPageNumber] = (correctIndex + 1).ToString();
                }
                addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
                addr.physicalPageOffset = addr.virtualPageOffset;
                mhs.UpdateStatistics(addr);
            }
            Console.WriteLine(mhs.DisplaySummaryStatistics());
        }





        //public static void Run(MemoryHierarchySimulator mhs) {
        //    List<string> accessOrder = new List<string>();
        //    foreach (MemoryAddress addr in mhs.memoryAddresses) {
        //        if (mhs.cache.ContainsKey(addr.virtualPageNumber)) {
        //            /* constantly removes the oldest element and renews it if it repeats so that it is considered recently used
        //             * ex: b, d, 6 -> d, 6, f
        //             * ^ d and 6 are shifted left since the the b is removed, f (the most recently used) is added onto the end.
        //             * ex2: b -> if the next element is also b, the b is removed and the new b is added
        //             */
        //            addr.isHit = true;
        //            accessOrder.Remove(addr.virtualPageNumber); 
        //            accessOrder.Add(addr.virtualPageNumber);
        //        } else {
        //            addr.isHit = false;
        //            if (mhs.cache.Count >= mhs.capacityOfCache) {
        //                mhs.cache.Remove(accessOrder[0]);
        //                accessOrder.RemoveAt(0);
        //            }
        //            accessOrder.Add(addr.virtualPageNumber);
        //            mhs.cache[addr.virtualPageNumber] = (accessOrder.IndexOf(addr.virtualPageNumber) + 1).ToString();                    
        //        }
        //        addr.physicalPageNumber = mhs.cache[addr.virtualPageNumber];
        //        addr.physicalPageOffset = addr.virtualPageOffset;
        //        mhs.UpdateStatistics(addr);
        //    }
        //    Console.WriteLine(mhs.DisplaySummaryStatistics());
        //}
    }
}
