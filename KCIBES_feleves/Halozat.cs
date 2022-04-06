using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCIBES_feleves
{
    class ListaElem : IListaElem
    {
        public string id { get; set; }
        public IKomponens Tartalom { get; set; }
        public IListaElem Kovetkezo { get; set; }

    }
    class ListaElemKette : IListaElem
    {
        public string id { get; set; }
        public IListaElem Kovetkezo1 { get; set; }
        public IListaElem Kovetkezo2 { get; set; }
    }
    class ListaElemEgybe : IListaElem
    {
        public string id { get; set; }
        public ListaElem Kovetkezo { get; set; }
    }
    public class LancoltLista
    {
        private IListaElem fej;
        public void ElemBeszuras(IKomponens tartalom, string id = null)
        {
            IListaElem p = fej;
            IListaElem e = null;
            ListaElem uj = new ListaElem();
            uj.Tartalom = tartalom;

            while (p != null)
            {
                if (p is ListaElem)
                {
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    e = p;
                    p = (p as ListaElemKette).Kovetkezo1;
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }

            if (e is null)
            {
                uj.Kovetkezo = fej;
                uj.id = id;
                fej = uj;
            }
            else
            {
                if (e is ListaElem)
                {
                    uj.Kovetkezo = p;
                    uj.id = id;
                    (e as ListaElem).Kovetkezo = uj;
                }
                else if (e is ListaElemEgybe)
                {
                    uj.Kovetkezo = p;
                    uj.id = id;
                    (e as ListaElemEgybe).Kovetkezo = uj;
                }
            }
        }
        public void KetteValasztBeallit(LancoltLista lista1, LancoltLista lista2, IListaElem listaelem_)
        {
            //Hozzáfűzöm a megkapott két listát az eredeti listához
            (listaelem_ as ListaElemKette).Kovetkezo1 = lista1.fej;
            (listaelem_ as ListaElemKette).Kovetkezo2 = lista2.fej;

            //A két lista vége ugyanarra mutat
            Egyesit(listaelem_);
        }
        public IListaElem Szetagaz()
        {
            //Beszur egy ListaElemKette elemet
            IListaElem p = fej;
            IListaElem e = null;
            ListaElemKette uj = new ListaElemKette();
            uj.id = "KetteValaszt";
            while (p != null)
            {
                if (p is ListaElem)
                {
                    e = p as ListaElem;
                    p = (p as ListaElem).Kovetkezo;
                }
            }
            if (e != null)
            {
                (e as ListaElem).Kovetkezo = uj;
            }
            if (e is null)
            {
                uj.Kovetkezo1 = fej;
                uj.Kovetkezo2 = fej;
                fej = uj;
            }
            return uj;
        }
        private void Egyesit(IListaElem listaelem_)
        {
            IListaElem p = (listaelem_ as ListaElemKette).Kovetkezo1;
            IListaElem e = null;
            ListaElemEgybe _uj = new ListaElemEgybe();
            _uj.id = "Egyesit";

            //bejárom a felső ágat
            while (p != null)
            {
                if (p is ListaElem)
                {
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    e = p;
                    p = (p as ListaElemKette).Kovetkezo1;
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            _uj.Kovetkezo = p as ListaElem;
            (e as ListaElem).Kovetkezo = _uj;

            //bejárom az alsó ágat
            p = (listaelem_ as ListaElemKette).Kovetkezo2;
            e = null;
            while (p != null)
            {

                if (p is ListaElem)
                {
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    e = p;
                    p = (p as ListaElemKette).Kovetkezo2;
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
             (e as ListaElem).Kovetkezo = _uj;
        }

        private ListaElemEgybe listaelemegybe_seged; //Minden olyan metódus meghívása után, ami párhuzamos kimentem a ListaElemegybe-t,hogy ne legyen végtelen ciklus, a p-t így viszem a következőre
        public void Eredoszamolo()
        {
            IListaElem p = fej;
            IListaElem e = null;
            double eredo = 0;
            //Végigmegyek a listán és összeadom az ellenállásokat egyszerűen, viszont ha párhuzamos, akkor meghívom a Replusz()-t
            while (p != null)
            {
                if (p is ListaElem)
                {
                    eredo += (p as ListaElem).Tartalom.ellenallas;
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    //Átadom a felső és alsó ágat
                    eredo += Replusz((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2);
                    p = listaelemegybe_seged.Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            //Feszgenre beállírom az eredot
            (fej as ListaElem).Tartalom.ellenallas = eredo;
        }
        private double Replusz(IListaElem p1, IListaElem p2)
        {
            IListaElem p = p1;
            IListaElem e = null;
            double eredo = 0;
            //Végigmegyek a felső ágon, összeadom az ellenállásokat
            while (!(p is ListaElemEgybe))
            {
                if (p is ListaElem)
                {
                    eredo += (p as ListaElem).Tartalom.ellenallas;
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                if (p is ListaElemKette)
                {
                    eredo += Replusz((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2);
                    p = listaelemegybe_seged.Kovetkezo;
                }
            }

            p = p2;
            e = null;
            double eredo2 = 0;
            //Végigmegyek az alsó ágon, összeadom az ellenállásokat
            while (!(p is ListaElemEgybe))
            {
                if (p is ListaElem)
                {
                    eredo2 += (p as ListaElem).Tartalom.ellenallas;
                    e = p;
                    p = (p as ListaElem).Kovetkezo;

                }
                if (p is ListaElemKette)
                {
                    eredo2 += Replusz((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2);
                    p = listaelemegybe_seged.Kovetkezo;
                }
            }
            listaelemegybe_seged = (p as ListaElemEgybe);
            //A felső ág ellenállását, és az alsó ág ellenállását repluszolom
            double replusz = ((eredo * eredo2) / (eredo + eredo2));

            return replusz;
        }

        public delegate void ErtekHandler(IListaElem elem);
        public event ErtekHandler ErtekEvent; //VoltmeroEvent
        public event ErtekHandler ErtekEvent2; //AmpermeroEvent
        public void AramAllito()
        {
            ;
            //Igazából ez kiszámolja a fogyasztókra eső áramot, és így a feszültséget is könnyen ki lehet számolni U=R*I
            IListaElem p = (fej as ListaElem).Kovetkezo;
            IListaElem e = fej;
            ;
            (fej as ListaElem).Tartalom.amper = (fej as ListaElem).Tartalom.feszultseg / (fej as ListaElem).Tartalom.ellenallas;
            while (p != null)
            {
                //ha ListaElem, leellenőrzöm az értékeket ha rossz adat van akkor exception, ha jó akkor beállítom a fogyasztóra az értékeket
                if (p is ListaElem)
                {
                    if ((p as ListaElem).Tartalom is Fogyaszto)
                    {
                        if ((p as ListaElem).Tartalom.amper != 0)
                        {
                            if (Math.Round((e as ListaElem).Tartalom.amper, 2) != (Math.Round((p as ListaElem).Tartalom.amper, 2)))
                            {
                                //Ha a beadott érték nem egyezik a kiszámolt értékkel, így hibás adat miatt kivételt dobok
                                throw new HibasAdatException((p as ListaElem).id);
                            }
                        }
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        if ((p as ListaElem).Tartalom.feszultseg != 0)
                        {
                            double seged = (p as ListaElem).Tartalom.amper * (p as ListaElem).Tartalom.ellenallas;
                            if ((p as ListaElem).Tartalom.feszultseg != Math.Round(seged, 2))
                            {
                                throw new HibasAdatException((p as ListaElem).id);
                            }
                        }
                        (p as ListaElem).Tartalom.feszultseg = (p as ListaElem).Tartalom.amper * (p as ListaElem).Tartalom.ellenallas;
                    }
                    else if ((p as ListaElem).Tartalom is Voltmero)
                    {
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        (p as ListaElem).Tartalom.feszultseg = (e as ListaElem).Tartalom.feszultseg;
                        ErtekEvent?.Invoke((p as ListaElem));
                    }
                    else if ((p as ListaElem).Tartalom is Ampermero)
                    {
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        (p as ListaElem).Tartalom.feszultseg = (e as ListaElem).Tartalom.feszultseg;
                        ErtekEvent2?.Invoke((p as ListaElem));
                    }
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }

                //ha lista
                else if (p is ListaElemKette)
                {
                    //Átadom a felső és alsó ágat, és a két ág között elosztandó áramot
                    AramAllitoParhuzamnal((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, (e as ListaElem).Tartalom.amper);
                    p = listaelemegybe_seged.Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
        }
        private double ListanEllenallasSzamolo(IListaElem p)
        {
            double ellenallas = 0;
            IListaElem e = null;
            while (p != null && !(p is ListaElemEgybe))
            {
                if (p is ListaElem)
                {
                    ellenallas += (p as ListaElem).Tartalom.ellenallas;
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    ellenallas += Replusz((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2);
                    p = listaelemegybe_seged.Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            return ellenallas;
        }
        private void AramAllitoParhuzamnal(IListaElem p1, IListaElem p2, double i1)
        {
            IListaElem p = p1;
            IListaElem e = new ListaElem();
            //Kiszámolom hogy az ághoz mennyi áram tartozik
            double p1amper = i1 * (ListanEllenallasSzamolo(p2) / ((ListanEllenallasSzamolo(p2) + ListanEllenallasSzamolo(p1))));
            double p2amper = i1 * (ListanEllenallasSzamolo(p1) / ((ListanEllenallasSzamolo(p2) + ListanEllenallasSzamolo(p1))));

            //Felso ágon végigmegyek
            Fogyaszto fseged = new Fogyaszto(0, 0, p1amper);
            (e as ListaElem).Tartalom = fseged;
            while (!(p is ListaElemEgybe))
            {
                if (p is ListaElem)
                {
                    if ((p as ListaElem).Tartalom is Fogyaszto)
                    {
                        if ((p as ListaElem).Tartalom.amper != 0)
                        {

                            if (Math.Round((e as ListaElem).Tartalom.amper, 2) != (Math.Round((p as ListaElem).Tartalom.amper, 2)))
                            {
                                throw new HibasAdatException((p as ListaElem).id);
                            }
                        }
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        if ((p as ListaElem).Tartalom.feszultseg != 0)
                        {
                            double seged = (p as ListaElem).Tartalom.amper * (p as ListaElem).Tartalom.ellenallas;
                            if ((p as ListaElem).Tartalom.feszultseg != Math.Round(seged, 2))
                            {
                                throw new HibasAdatException((p as ListaElem).id);
                            }
                        }
                        (p as ListaElem).Tartalom.feszultseg = (p as ListaElem).Tartalom.amper * (p as ListaElem).Tartalom.ellenallas;
                    }
                    else if ((p as ListaElem).Tartalom is Voltmero)
                    {
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        (p as ListaElem).Tartalom.feszultseg = (e as ListaElem).Tartalom.feszultseg;
                        ErtekEvent?.Invoke((p as ListaElem));
                    }
                    else if ((p as ListaElem).Tartalom is Ampermero)
                    {
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        (p as ListaElem).Tartalom.feszultseg = (e as ListaElem).Tartalom.feszultseg;
                        ErtekEvent2?.Invoke((p as ListaElem));
                    }
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    AramAllitoParhuzamnal((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, p1amper);
                    p = listaelemegybe_seged.Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    IListaElem eseged = new ListaElem();
                    (eseged as ListaElem).Tartalom = fseged;
                    e = eseged;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }

            //Alsó ágon végigmegyek
            p = p2;
            e = new ListaElem();
            Fogyaszto fseged2 = new Fogyaszto(0, 0, p2amper);
            (e as ListaElem).Tartalom = fseged2;
            while (!(p is ListaElemEgybe))
            {
                if (p is ListaElem)
                {
                    if ((p as ListaElem).Tartalom is Fogyaszto)
                    {
                        if ((p as ListaElem).Tartalom.amper != 0)
                        {
                            if (Math.Round((e as ListaElem).Tartalom.amper, 2) != (Math.Round((p as ListaElem).Tartalom.amper, 2)))
                            {
                                throw new HibasAdatException((p as ListaElem).id);
                            }
                        }
                       (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        if ((p as ListaElem).Tartalom.feszultseg != 0)
                        {
                            double seged = (p as ListaElem).Tartalom.amper * (p as ListaElem).Tartalom.ellenallas;
                            if ((p as ListaElem).Tartalom.feszultseg != Math.Round(seged, 2))
                            {
                                throw new HibasAdatException((p as ListaElem).id);
                            }
                        }
                        (p as ListaElem).Tartalom.feszultseg = (p as ListaElem).Tartalom.amper * (p as ListaElem).Tartalom.ellenallas;
                    }
                    else if ((p as ListaElem).Tartalom is Voltmero)
                    {
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        (p as ListaElem).Tartalom.feszultseg = (e as ListaElem).Tartalom.feszultseg;
                        ErtekEvent?.Invoke((p as ListaElem));
                    }
                    else if ((p as ListaElem).Tartalom is Ampermero)
                    {
                        (p as ListaElem).Tartalom.amper = (e as ListaElem).Tartalom.amper;
                        (p as ListaElem).Tartalom.feszultseg = (e as ListaElem).Tartalom.feszultseg;
                        ErtekEvent2?.Invoke((p as ListaElem));
                    }
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    AramAllitoParhuzamnal((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, p2amper);
                    p = listaelemegybe_seged.Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    IListaElem eseged2 = new ListaElem();
                    (eseged2 as ListaElem).Tartalom = fseged2;
                    e = eseged2;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            listaelemegybe_seged = (p as ListaElemEgybe);
        }
        public void ListaUrit()
        {
            fej = null;
        }
        public void Modosit(string id, double ellenallas = 0, double fesz = 0, double amper = 0)
        {
            //Módosítani lehet vele a komponensek értékeit
            IListaElem p = fej;
            IListaElem e = null;
            while (p != null && p.id != id)
            {
                if (p is ListaElem)
                {
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    p = ModositParhuzamosSeged((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, id, ellenallas, fesz, amper);
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            if (ellenallas != 0)
            {
                (p as ListaElem).Tartalom.ellenallas = ellenallas;
            }
            if (fesz != 0)
            {
                (p as ListaElem).Tartalom.feszultseg = fesz;
            }
            if (amper != 0)
            {
                (p as ListaElem).Tartalom.amper = amper;
            }
        }

        private IListaElem ModositParhuzamosSeged(IListaElem p1, IListaElem p2, string id, double ellenallas = 0, double fesz = 0, double amper = 0)
        {
            IListaElem p = p1;
            IListaElem e = null;
            //Felső ág
            while (p != null && p.id != id)
            {
                if (p is ListaElem)
                {
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    p = ModositParhuzamosSeged((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, id, ellenallas, fesz, amper);
                }
                else if (p is ListaElemEgybe)
                {
                    e = p;
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            //ha megvan a felső ágban akkor itt return, nem nézi meg az alsó ágban
            if (p != null && p.id == id)
            {
                return p;
            }

            //Alsó ág
            p = p2;
            e = null;
            while (!(p is ListaElemEgybe) && p.id != id)
            {
                if (p is ListaElem)
                {
                    e = p;
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemKette)
                {
                    p = ModositParhuzamosSeged((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, id, ellenallas, fesz, amper);

                }

            }
            return p;
        }
        public string Convert()
        {
            if (this.fej is null)
            {
                return null;
            }
            else
            {
                string sor = "";
                return Convert(fej, ref sor);
            }
        }
        private string Convert(IListaElem p, ref string sor)
        {
            while (p != null)
            {
                if (p is ListaElemKette)
                {
                    sor += p.id + "->";
                    sor += "(";
                    ConvertParhuzamosnal((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, ref sor);
                    sor += ")";
                    sor += "->";
                    p = listaelemegybe_seged;
                }
                if (p != null)
                {
                    sor += p.id + "->";
                }
                if (p is ListaElem)
                {
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            return sor;
        }
        private void ConvertParhuzamosnal(IListaElem p1, IListaElem p2, ref string sor)
        {
            IListaElem p = p1;
            //Felső ág
            sor += "(";
            while (!(p is ListaElemEgybe))
            {
                if (p is ListaElemKette)
                {
                    sor += p.id + "->";
                    sor += "(";
                    ConvertParhuzamosnal((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, ref sor);
                    sor += ")";
                    sor += "->";
                    p = listaelemegybe_seged;
                }
                if (p != null)
                {
                    sor += p.id + "->";
                }
                if (p is ListaElem)
                {
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            sor += ")";
            //Alsó ág
            p = p2;
            sor += "(";
            while (!(p is ListaElemEgybe))
            {
                if (p is ListaElemKette)
                {
                    sor += p.id + "->";
                    sor += "(";
                    ConvertParhuzamosnal((p as ListaElemKette).Kovetkezo1, (p as ListaElemKette).Kovetkezo2, ref sor);
                    sor += ")";
                    p = listaelemegybe_seged;
                }
                if (p != null)
                {
                    sor += p.id + "->";
                }
                if (p is ListaElem)
                {
                    p = (p as ListaElem).Kovetkezo;
                }
                else if (p is ListaElemEgybe)
                {
                    p = (p as ListaElemEgybe).Kovetkezo;
                }
            }
            listaelemegybe_seged = p as ListaElemEgybe;
            sor += ")";
        }
    }
}

