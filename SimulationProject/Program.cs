using System.Collections.Generic;

namespace SimulationProject
{
    class Auto
    {
        public int nosnost;//kilogramy
        public int dobaCesty;//min
        public int dobaNakládání; //min
        public int dobaVykládání; //min

        void Nakládání(){ }

        public Auto(int nosnost1, int dobaCesty1, int dobaNakládání1, int dobaVykládání1)
        {
            nosnost = nosnost1;
            dobaCesty = dobaCesty1;
            dobaNakládání = dobaNakládání1;
            dobaVykládání = dobaVykládání1;
        }
    }

    class Akce
    {
        public int autoId;
        public Typ typ;
        public int zacatek;
        public Akce(int id, Typ typp, int zacatekk) 
        {
            autoId = id;
            typ = typp;
            zacatek = zacatekk;

        }
        public enum Typ { nalož, vylož }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Auto> Auta = new List<Auto>();
            PriorityQueue<Akce, int> Kalendář = new PriorityQueue<Akce, int>();


            Console.WriteLine("Kolik chceš pysku");
            int písek = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Kolik chceš aut?");
            int početAut = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Jaký je tvoje auto?\n Nosnost, Doba cesty, Doba nakládání, Doba vykládábní\n Každý údaj odděl mezerou");
            int startTime = 0;
            for (int i = 1; i <= početAut; i++) 
            {
                Console.WriteLine("Tvoje "+ i +". auto:");
                int[] input = Array.ConvertAll(Console.ReadLine().Split(" "), int.Parse);
                Auta.Add(new Auto(input[0], input[1], input[2], input[3]));

                Kalendář.Enqueue(new Akce(i - 1, Akce.Typ.nalož, startTime), startTime);
                startTime = Auta[i - 1].dobaNakládání;
            }

            int finalniCas = 0;

            while (písek > 0)
            {
                Akce aktualníAkce = Kalendář.Dequeue();
                Auto aktuálníAuto = Auta[aktualníAkce.autoId];

                if (aktualníAkce.typ == Akce.Typ.nalož)
                {
                    int novýZačátek = aktualníAkce.zacatek + aktuálníAuto.dobaNakládání + aktuálníAuto.dobaCesty;
                    Kalendář.Enqueue(new Akce(aktualníAkce.autoId, Akce.Typ.vylož, novýZačátek), novýZačátek);
                }
                else 
                {
                    int novýZačátek = aktualníAkce.zacatek + aktuálníAuto.dobaVykládání + aktuálníAuto.dobaCesty;

                    List<Akce> akceVKalendari = new List<Akce>();

                    foreach (var akce in Kalendář.UnorderedItems)
                    {
                        akceVKalendari.Add(akce.Element);
                    }

                    int posledniCasNalozeni = 0;

                    foreach(var akceVkal in akceVKalendari)
                    {
                        if (akceVKal.zacatek + Auta[akceVkal.autoId].dobaNakládání > posledniCasNalozeni)
                        {
                            posledniCasNalozeni = akceVKal.zacatek + Auta[akceVkal.autoId].dobaNakládání;
                        }
                    }

                    int pisekKodebrani = aktuálníAuto.nosnost;

                    if (posledniCasNalozeni < novýZačátek) {
                        Kalendář.Enqueue(new Akce(aktualníAkce.autoId, Akce.Typ.nalož, novýZačátek), novýZačátek);
                        if (písek - pisekKodebrani <= 0)
                        {
                            finalniCas = novýZačátek + aktuálníAuto.dobaNakládání;
                            break;
                        }
                        else
                        {
                            písek -= pisekKodebrani;
                        }
                    } else
                    {
                        if (písek - pisekKodebrani <= 0)
                        {
                            finalniCas = novýZačátek + posledniCasNalozeni;
                            break;
                        }
                        else
                        {
                            písek -= pisekKodebrani;
                        }
                        Kalendář.Enqueue(new Akce(aktualníAkce.autoId, Akce.Typ.nalož, posledniCasNalozeni), posledniCasNalozeni);
                    }
                    
                }

            }

            Console.WriteLine("Finalni cas je", finalniCas);
        }
    }
}
