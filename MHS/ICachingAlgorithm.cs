using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    public interface ICachingAlgorithm
    {
        public static abstract void Run(MemoryHierarchySimulator mhs);
    }

}
