using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCIBES_feleves
{
    class HibasAdatException : Exception
    {
        public HibasAdatException(string komponensnev) : base($"Hibas adat a(z) {komponensnev} komponensen")
        {

        }
    }
}
