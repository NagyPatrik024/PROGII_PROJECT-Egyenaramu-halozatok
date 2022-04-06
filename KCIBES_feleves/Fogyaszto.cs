using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCIBES_feleves
{
    class Fogyaszto : IKomponens
    {
        public double ellenallas { get; set; }
        public double feszultseg { get; set; }
        public double amper { get; set; }
        public Fogyaszto(double ellenallas = 0, double feszultseg = 0, double amper = 0)
        {
            this.ellenallas = ellenallas;
            this.feszultseg = feszultseg;
            this.amper = amper;
        }
    }
}
