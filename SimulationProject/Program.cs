namespace SimulationProject
{
    class Auto
    {
        int autoID;
        int nosnost;//kilogramy
        int dobaCesty;//min
        int dobaNakládání; //min
        int dobaVykládání; //min
        Stav stav;
        int časDoDokončeníČinnosti; //min
        enum Stav { čeká, nakládá, jede, vykládá }

        void Nakládání(){ }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Auto> seznamAut = new List<Auto>();
            Queue<Auto> NakládacíStanice = new Queue<Auto>();
            Console.WriteLine("Jak dlouhá je cesta?");
            int vzdálenostCesty = Convert.ToInt32(Console.ReadLine());


        }
    }
}
