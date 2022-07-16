namespace juego;
//Parte visual
public class Program
{
    public static void Main(string[] argd)
    {
        WaysToPLay1();

    }
    public static void WaysToPLay1()
    {
        //haciendo instancias de los jugadores
        System.Console.WriteLine("Seleccione la cantidad de jugadores");
        int playercount = 0;
        while (playercount <= 0)
        {
            playercount = int.Parse(System.Console.ReadLine());
        }
         System.Console.Clear();
        Player[] challengers = CreatePlayers(playercount);
    
        match match = new match(delegate (Records records1, int optionRecord1, Records records2, int optionRecord2) { return optionRecord1 == optionRecord2; });
        //Repartiendo las fichas

        System.Console.WriteLine("Seleccione la cantidad de fichas a repartir");
        // int numberOfOptions = 0;
        int numberOfOptions = 0;
        while (numberOfOptions <= 0)
        {
            numberOfOptions = int.Parse(System.Console.ReadLine());
        }
        System.Console.Clear();
        GameInformation gi = new GameInformation(numberOfOptions, new weight(delegate (Records records) { return records.element1 + records.element2; }));
        Referee referee = new Referee(numberOfOptions, new clasicEnd(match), new clasicWinner(), new validator(match), new shuffler());
        int index = 0;
        DealRecords(challengers, index, referee, gi);
        int max = 0;
        while (!referee.finalized.EndGame(gi, referee, ref max))
        {
            //caminando por cada jugador siempre que el juego no haya acabado
            foreach (Player item in referee.AsignedRecords.Keys)
            {
                if (referee.finalized.EndGame(gi, referee, ref max)) break;
                //verificar que el jugador no se pase
                if (referee.HavesARecord(referee, gi, item, match))
                {

                    InformationForPlayer info = referee.ProvidedInformation(referee, gi, item, match, gi.weight);
                    jugada jugadaAux = item.GiveMeRecords(info, referee);
                    while (!referee.validator.ValidPlay(jugadaAux, gi))
                    {
                        if (item is ManualPlayer)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("La ficha jugada no es correcta, vuelva a jugar.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        jugadaAux = item.GiveMeRecords(info, referee);
                    }
                    if (gi.OptionsToPlay.Count != 0)
                    {
                        foreach (var item2 in info.Options)
                        {
                            System.Console.WriteLine(item2.Item2 + " option# ");
                        }
                    }
                    System.Console.WriteLine("Jugador " + item.id + " ha jugado: " + jugadaAux.record.element1 + " " + jugadaAux.record.element2);
                    referee.Play(jugadaAux, gi, new match(delegate (Records records1, int optionRecords1, Records records2, int optionRecord2) { return optionRecords1 == optionRecord2; }));
                    referee.AsignedRecords[item].Remove(jugadaAux.record);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine("pase para " + item.id);
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
            }
        }
        System.Console.WriteLine("Se termino el juego");
        referee.Winner.Win(referee, gi);
    }
    private static Player[] CreatePlayers(int count)
    {
        Player[] sol = new Player[count];
        int selection = 0;
        System.Console.WriteLine("Types of player: 1:Manual 2:Random 3:Greedy 4:Data");
        for (int i = 0; i < count; i++)
        {
            selection = -1;
            System.Console.WriteLine("Select type of player: ");
            while (selection < 0)
            {
                selection = int.Parse(System.Console.ReadLine());

            }
            if (selection % 4 == 1) { sol[i] = new ManualPlayer("ID " + count + 1); }
            else if (selection % 4 == 2) { sol[i] = new RandomPlayer("ID " + count + 1); }
            else if (selection % 4 == 3) { sol[i] = new GreedyPlayer("ID " + count + 1); }
            else sol[i] = new DataPlayer("ID " + count + 1);
        }
        return sol;
    }
    private static void DealRecords(Player[] challengers, int index, Referee referee, GameInformation gi)
    {
        for (int i = 0; i < challengers.Length; i++)
        {
            referee.shuffler.Shuffle(challengers[i], gi, ref index, referee);
        }
    }
}

