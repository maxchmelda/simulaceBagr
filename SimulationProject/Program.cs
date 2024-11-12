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
        public int přejezd = 0; //počet kolikrát auto přejelo cestu

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

            // Vstup hodnot
            Console.WriteLine("Kolik chceš písku?");
            int písek = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Typ zadání?\n1 Několik různých aut, chci vědět čas\n 2 Několik stejných aut, chci vědět kolik");

            int výběrZadání = Convert.ToInt32(Console.ReadLine());

            switch (výběrZadání) 
            {
                case 1:
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
                    break;
                case 2:
                    Console.WriteLine("Jaký je tvoje auto?\nNosnost, Doba cesty, Doba nakládání, Doba vykládání\nKaždý údaj odděl mezerou");
                        int[] input2 = Array.ConvertAll(Console.ReadLine().Split(" "), int.Parse);
                        Auta.Add(new Auto(input2[0], input2[1], input2[2], input2[3]));
                        Kalendář.Enqueue(new Akce(0, Akce.Typ.nalož, 0), 0);
                    break;
            }
            

            int finalniCas = 0;

            // Dokud pisek neni prevezen tak delej
            while (písek > 0)
            {
                // Vem auto s nejblizsi akci z kalendare
                Akce aktualniAkce = Kalendář.Dequeue();
                Auto aktualniAuto = Auta[aktualniAkce.autoId];

                
                // pokud jeho akce je naloz tak ho posli vylozit, tzn pridej jeho akci vylozeni s zacatecnim casem: zacatek nalozeni + doba nalozeni + doba cesty 
                if (aktualniAkce.typ == Akce.Typ.nalož)
                {
                    aktualniAuto.přejezd++;
                    int novyZacatek = aktualniAkce.zacatek + aktualniAuto.dobaNakládání + aktualniAuto.dobaCesty;
                    Kalendář.Enqueue(new Akce(aktualniAkce.autoId, Akce.Typ.vylož, novyZacatek), novyZacatek);
                }
                else
                {

                    aktualniAuto.přejezd++;
                    // pokud jeho akce je vyloz tak ho posli nalozit
                    int novyZacatek = aktualniAkce.zacatek + aktualniAuto.dobaVykládání + aktualniAuto.dobaCesty;
                    int pisekKodobrani = Math.Min(aktualniAuto.nosnost, písek);

                    // odeber pisek
                    písek -= pisekKodobrani;

                    // pokud uz neni pisek tak finalni cas je cas kdy dokončil vykládání
                    if (písek <= 0)
                    {
                        finalniCas = aktualniAkce.zacatek + aktualniAuto.dobaVykládání;
                        break;
                    }

                    // vem vsechny uz naplanovane dalsi akce v kalendari
                    List<Akce> akceVKalendari = new List<Akce>(Kalendář.UnorderedItems.Select(a => a.Element));
                    int posledniCasNalozeni = 0;

                    //pokud je nejaka akce v kalendari naloz tzn auta uz jsou ve fronte na naklad tak cas zacatku noveho nalozeni tohodle auta je cas posledniho vylozeni auta uz v kalendari
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

                    // tady to zjisti jestli teda nejaky auto uz v kalendari nevyklada v tu dobu kdy prijede aktualni auto, pokud ne tak to prida nalozeni hned po prijezdu pokud jo tak to prida nalozeni az po nalozeni posledniho uz naplanovaneho auta
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

            switch (výběrZadání) 
            {
                case 1:
                    Console.WriteLine($"Finální čas je {finalniCas}");
                    int ii = 1;
                    foreach (Auto auto in Auta)
                    {
                        Console.WriteLine(ii + ". auto přejelo cestu " + auto.přejezd + "x");
                        ii++;
                    }
                    break;
                case 2:
                    Console.WriteLine("Byl by potřeba konvoj z " + Auta[0].přejezd/2 + " aut.");
                    break;
            }

            
        }
    }
}
