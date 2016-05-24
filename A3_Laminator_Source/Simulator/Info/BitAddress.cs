using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Info
{
    class BitAddress
    {
        public readonly string address;
        public readonly int value;
        public BitAddress(string address, int value)
        {
            this.address = address;
            this.value = value;
        }
    }
}
