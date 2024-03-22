using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS
{
    public class MemoryAddress
    {
        //Initial Setup of Properties
        public char accessType { get; set; }
        public string virtualPageNumber { get; set; }
        public string virtualPageOffset { get; set; }
        public string physicalPageNumber { get; set; }
        public string physicalPageOffset { get; set; }
        public bool isHit { get; set; }

        //Default Constructor
        public MemoryAddress()
        {
            virtualPageNumber = string.Empty;
            virtualPageOffset = string.Empty;
            isHit = false;
        }

        //Parameterized Constructor
        public MemoryAddress(char accessType, string virtualPageNumber, string virtualPageOffset)
        {
            this.accessType = accessType;
            this.virtualPageNumber = virtualPageNumber;
            this.virtualPageOffset = virtualPageOffset;
            isHit = false;
        }
    }
}
