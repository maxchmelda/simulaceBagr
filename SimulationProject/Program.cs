using System;
using System.Collections.Generic;

namespace SimulationProject
{
    class Auto
    {
        public int nosnost; // kilogramy
        public int dobaCesty; // min
        public int dobaNakládání; // min
        public int dobaVykládání; // min

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

            Console.WriteLine("Kolik chceš písku?");
            int písek = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Kolik chceš aut?");
            int početAut = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Jaký je tvoje auto?\nNosnost, Doba cesty, Doba nakládání, Doba vykládání\nKaždý údaj odděl mezerou");
            for (int i = 1; i <= početAut; i++)
            {
                Console.WriteLine("Tvoje " + i + ". auto:");
                int[] input = Array.ConvertAll(Console.ReadLine().Split(" "), int.Parse);
                Auta.Add(new Auto(input[0], input[1], input[2], input[3]));

                Kalendář.Enqueue(new Akce(i - 1, Akce.Typ.nalož, 0), 0);
            }

            int finalniCas = 0;

            while (písek > 0)
            {
                Akce aktualniAkce = Kalendář.Dequeue();
                Auto aktualniAuto = Auta[aktualniAkce.autoId];

                if (aktualniAkce.typ == Akce.Typ.nalož)
                {
                    int novyZacatek = aktualniAkce.zacatek + aktualniAuto.dobaNakládání + aktualniAuto.dobaCesty;
                    Kalendář.Enqueue(new Akce(aktualniAkce.autoId, Akce.Typ.vylož, novyZacatek), novyZacatek);
                }
                else
                {
                    int novyZacatek = aktualniAkce.zacatek + aktualniAuto.dobaVykládání + aktualniAuto.dobaCesty;
                    int pisekKodobrani = Math.Min(aktualniAuto.nosnost, písek);

                    písek -= pisekKodobrani;

                    if (písek <= 0)
                    {
                        finalniCas = novyZacatek;
                        break;
                    }

                    List<Akce> akceVKalendari = new List<Akce>(Kalendář.UnorderedItems.Select(a => a.Element));
                    int posledniCasNalozeni = 0;

                    foreach (var akceVKal in akceVKalendari)
                    {
                        if (akceVKal.typ == Akce.Typ.nalož)
                        {
                            int casNalozeni = akceVKal.zacatek + Auta[akceVKal.autoId].dobaNakládání;
                            if (casNalozeni > posledniCasNalozeni)
                            {
                                posledniCasNalozeni = casNalozeni;
                            }
                        }
                    }

                    if (posledniCasNalozeni < novyZacatek)
                    {
                        Kalendář.Enqueue(new Akce(aktualniAkce.autoId, Akce.Typ.nalož, novyZacatek), novyZacatek);
                    }
                    else
                    {
                        Kalendář.Enqueue(new Akce(aktualniAkce.autoId, Akce.Typ.nalož, posledniCasNalozeni), posledniCasNalozeni);
                    }
                }
            }

            Console.WriteLine($"Finální čas je {finalniCas}");
        }
    }
}
