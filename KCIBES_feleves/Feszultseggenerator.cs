using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCIBES_feleves
{
    class Feszultseggenerator : IKomponens
    {
        public double feszultseg { get; set; }
        public double amper { get; set; }
        public double ellenallas { get; set; }
        public Feszultseggenerator(double fesz)
        {
            this.feszultseg = fesz;
            this.ellenallas = 0;
            this.amper = 0;
        }
    }
}
