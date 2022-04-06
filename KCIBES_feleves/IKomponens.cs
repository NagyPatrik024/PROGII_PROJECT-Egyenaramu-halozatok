using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCIBES_feleves
{
    public interface IKomponens
    {
        double feszultseg { get; set; }
        double amper { get; set; }
        double ellenallas { get; set; }
    }
}
