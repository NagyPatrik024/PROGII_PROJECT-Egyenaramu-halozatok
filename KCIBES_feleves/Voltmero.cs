using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCIBES_feleves
{
    class Voltmero : IKomponens
    {
        public double ellenallas { get; set; }
        public double feszultseg { get; set; }
        public double amper { get; set; }
        public Voltmero()
        {
            amper = 0;
            ellenallas = 0;
            feszultseg = 0;
        }
    }
}
