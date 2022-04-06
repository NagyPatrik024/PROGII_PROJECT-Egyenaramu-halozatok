using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KCIBES_feleves
{
    class Program
    {
    
        static void VoltKiiro(IListaElem elem)
        {
            Console.WriteLine($"A {(elem as ListaElem).id} értéke {Math.Round(((elem as ListaElem).Tartalom.feszultseg), 3)} ");
        }
        static void AmperKiiro(IListaElem elem)
        {
            Console.WriteLine($"Az {(elem as ListaElem).id} értéke {Math.Round(((elem as ListaElem).Tartalom.amper), 3)}");
        }
        static void Main(string[] args)
        {
            LancoltLista lista = new LancoltLista();
            //Ha masik txt-t nézünk akkor két helyen kell változtatni, itt és lentebb  if (readline == "Ürít") alatt
            StreamReader sr = new StreamReader("feleves.txt");
            lista.ErtekEvent += VoltKiiro;
            lista.ErtekEvent2 += AmperKiiro;
            FajlBeolvaso();
            sr.Close();
            try
            {
                lista.Eredoszamolo();
                lista.AramAllito();
                Console.WriteLine("Felépítés:");
                Console.WriteLine(lista.Convert()); 
                Console.WriteLine();
                Console.WriteLine("Elérhető parancsok: 'Módosít'/'Ürít'/'Számol'/'Vége'");
                Console.WriteLine("Minden számolás után, ha módosít használja az Ürít funkciót elsőnek");
                string readline = "";
                while (readline != "Vége")
                {
                    readline = Console.ReadLine();
                    if (readline == "Ürít")
                    {
                        lista.ListaUrit();
                        //Itt is kell módosítani a txt-t
                        sr = new StreamReader("feleves.txt");
                        FajlBeolvaso();
                        sr.Close();
                        Console.WriteLine("Lista ürítve, alapértékek beállítva");
                    }
                    if (readline == "Módosít")
                    {
                        Console.WriteLine("Add meg melyik elemet szeretnéd módosítani(nev/ellenallas/fesz/aram)");
                        readline = Console.ReadLine();
                        string[] readlinetomb = readline.Split('/');
                        lista.Modosit(readlinetomb[0], Convert.ToDouble(readlinetomb[1]), Convert.ToDouble(readlinetomb[2]), Convert.ToDouble(readlinetomb[3]));
                        Console.WriteLine("Módosítva!");
                    }
                    else if (readline == "Számol")
                    {
                        lista.Eredoszamolo();
                        lista.AramAllito();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            void FajlBeolvaso()
            {
                string seged = "";
                seged = sr.ReadLine();
                HozzaadFeszgen(lista, Convert.ToDouble(seged));
                while (!sr.EndOfStream)
                {
                    seged = sr.ReadLine();
                    if (seged.Contains('/'))
                    {
                        Fajlbeolvasoseged(seged, lista);
                    }
                    else
                    {
                        if (seged == "szetagaz")
                        {
                            Szetvalaszt(lista.Szetagaz(), lista);
                        }
                    }
                }
            }

            void Szetvalaszt(IListaElem listaelem_, LancoltLista lista_)
            {

                //Elágazásnál létrehoz 2 listát, egy felsőt és egy alsót
                LancoltLista lista1 = new LancoltLista();
                LancoltLista lista2 = new LancoltLista();

                string seged = "";
                while (seged != "szetagazvege")
                {
                    //FELSŐ ÁG
                    seged = sr.ReadLine();
                    if (seged == "felso")
                    {
                        while (seged != "also")
                        {
                            seged = sr.ReadLine();
                            if (seged != "also")
                            { Fajlbeolvasoseged(seged, lista1); }
                            if (seged == "szetagaz")
                            {
                                Szetvalaszt(lista1.Szetagaz(), lista1);
                                seged = "kesz";
                            }
                        }
                    }
                    //ALSÓ ÁG
                    if (seged == "also")
                    {
                        while (seged != "szetagazvege")
                        {
                            seged = sr.ReadLine();
                            if (seged != "szetagazvege")
                            { Fajlbeolvasoseged(seged, lista2); }
                            if (seged == "szetagaz")
                            {
                                Szetvalaszt(lista2.Szetagaz(), lista2);
                                seged = "kesz";
                            }
                        }
                    }
                    //Ez beszúrja az eredeti listába a felső és az alsó listát
                    lista_.KetteValasztBeallit(lista1, lista2, listaelem_);
                }
            }
        }
        static void Fajlbeolvasoseged(string seged, LancoltLista lista_)
        {
            string[] segedtomb;
            segedtomb = seged.Split('/');
            if (segedtomb.Length == 2)
            {
                if (segedtomb[0] == "ampermero")
                {
                    HozzaadAmpermero(lista_, segedtomb[1]);
                }
                else { HozzaadVoltmero(lista_, segedtomb[1]); }
            }
            else
            {
                if (segedtomb[0] == "fogyaszto")
                {

                    HozzaadFogyaszto(lista_, Convert.ToDouble(segedtomb[1]), Convert.ToDouble(segedtomb[2]), Convert.ToDouble(segedtomb[3]), segedtomb[4]);
                }
            }
        }
        static void HozzaadFogyaszto(LancoltLista lista_, double res, double amper, double volt, string id)
        {
            Fogyaszto f = new Fogyaszto(res, amper, volt);
            lista_.ElemBeszuras(f, id);
        }
        static void HozzaadVoltmero(LancoltLista lista_, string id)
        {
            Voltmero v = new Voltmero();
            lista_.ElemBeszuras(v, id);
        }
        static void HozzaadAmpermero(LancoltLista lista_, string id)
        {
            Ampermero a = new Ampermero();
            lista_.ElemBeszuras(a, id);
        }
        static void HozzaadFeszgen(LancoltLista lista_, double fesz)
        {
            Feszultseggenerator f = new Feszultseggenerator(fesz);
            lista_.ElemBeszuras(f, "feszgen");
        }
    }
}

