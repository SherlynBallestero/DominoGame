namespace juego;
//Parte visual
public class Program
{
    public static void Main(string[] argd)
    {
        while (true)
        {
            System.Console.WriteLine("Que desea jugar? 1) TicTacToe  2)Dominó");
            string answer = Console.ReadLine();
            if (answer[0] == '1')
            {
                TicTacToe();

            }
            else if (answer[0] == '2')
            {
                WaysToPLay1();
            }
        }

    }
    #region Domino
    public static void WaysToPLay1()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        System.Console.WriteLine( "Instrucciones" );
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        System.Console.WriteLine( "1)Las opciones de juego son el lado de la mesa por las que puede seleccionar jugar" );
        System.Console.WriteLine( "Si selecciona opción 0 jugará por la 1ra y 1 por la 2da" );
        System.Console.WriteLine( "Podrá observar mientras juega todas sus fichas, pero solo podrá seleccionar de entre las fichas que son posible jugar, dada las condiciones");
        System.Console.WriteLine( "de enlace de una ficha a otra");
        System.Console.WriteLine("Espero que lo disfrute:)");
        Console.ForegroundColor = ConsoleColor.Gray;
        
        bool keepPlaying = true;
        while (keepPlaying)
        {


            //haciendo instancias de los jugadores
            System.Console.WriteLine("Seleccione la cantidad de jugadores");
            string aux = Console.ReadLine();
            int playercount = 0;
            while (!int.TryParse(aux, out playercount))
            {
                System.Console.WriteLine("Debe elegir un número");
                aux = System.Console.ReadLine();
            }
            System.Console.Clear();
            Player[] challengers = CreatePlayers(playercount);

            match match = new match(delegate (Records records1, int optionRecord1, Records records2, int optionRecord2, List<Records> rrec) { return optionRecord1 == optionRecord2; });
            //Repartiendo las fichas

            System.Console.WriteLine("Seleccione la cantidad de fichas a repartir");
            int numberOfOptions = 0;
            aux = Console.ReadLine();
            while (!int.TryParse(aux, out numberOfOptions))
            {
                System.Console.WriteLine("Debe elegir un número");
                aux = System.Console.ReadLine();
            }
            System.Console.Clear();
            GameInformation gi = new GameInformation(numberOfOptions, new weight(delegate (Records records) { return records.element1 + records.element2; }), new makerRecords());
            Referee referee = new Referee(numberOfOptions, new clasicEnd(match), new clasicWinner(), new validator(match), new shuffler(), new TurnPlayerClassic());
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
                        System.Console.WriteLine("Fichas del jugador " + item.id);
                        foreach (var item2 in referee.AsignedRecords[item])
                        {
                            System.Console.WriteLine(item2.element1 + "/" + item2.element2);
                        }
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
                            int a = 0;
                            foreach (var item2 in info.Options)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                System.Console.WriteLine(item2.Item2 + " option# " + a++);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        System.Console.WriteLine("Jugador " + item.id + " ha jugado: " + jugadaAux.record.element1 + " " + jugadaAux.record.element2);
                        referee.Play(jugadaAux, gi, new match(delegate (Records records1, int optionRecords1, Records records2, int optionRecord2, List<Records> records) { return optionRecords1 == optionRecord2; }));
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
            string answer = " ";
            System.Console.WriteLine("Quiere hacer nueva partida? Si:2,No:1");
           
            while (answer[0] != '1' && answer[0] != '2')
            {
                answer = Console.ReadLine();
                if (answer[0] == '1') keepPlaying = false;
                if (answer[0] == '2') keepPlaying = true;
                System.Console.WriteLine("Quiere hacer nueva partida? Si:2,No:1");
            }
        }

    }
    private static Player[] CreatePlayers(int count)
    {
        Player[] sol = new Player[count];
        int selection = 0;
        for (int i = 0; i < count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Seleccione tipo de jugador: ");
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Tipos de jugadores: 1:Manual 2:Random 3:Greedy 4:Data");
            Console.ForegroundColor = ConsoleColor.Gray;
            selection = -1;
            string aux = Console.ReadLine();
            string id = "";
            while (!int.TryParse(aux, out selection))
            {
                System.Console.WriteLine("debe seleccoinar alguna de las opciones anteriores");
                aux = Console.ReadLine();

            }
            System.Console.WriteLine("Nombre?");
            id = Console.ReadLine();
            if (selection % 4 == 1) { sol[i] = new ManualPlayer("ID(tipo manual): " + id); }
            else if (selection % 4 == 2) { sol[i] = new RandomPlayer("ID(tipo random): " + id); }
            else if (selection % 4 == 3) { sol[i] = new GreedyPlayer("ID(tipo Greedy): " + id); }
            else sol[i] = new DataPlayer("ID(Data) " + id);
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
    #endregion
    #region Tictactoe
    public static void TicTacToe()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("Instrucciones...");
        Console.ForegroundColor = ConsoleColor.Gray;
        System.Console.WriteLine("1)El primer jugador es la X y el segundo el O");
        System.Console.WriteLine("2)Cuando le toque  jugar le saldrá para seleccionar una opción de 0 a 8, usted deberá elegir la opción que");
        System.Console.WriteLine("corresponda con la posición que desee jugar teniendo en cuenta la siguiente asignación de posiciones");
        Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("0-0/0-1/0-2");
        System.Console.WriteLine("1-0/1-1/1-2");
        System.Console.WriteLine("2-0/2-1/2-2");
        Console.ForegroundColor = ConsoleColor.Gray;
        System.Console.WriteLine("seleccione cualquier tecla para continuar");
        System.Console.ReadLine();

        //variables necesarias...
        //pintador de tableros 
        IPrinter p = new PrienterTictactoe();
        //dos jugadores básicos del tictactoe...
        Player playerX = new ManualPlayer("X");
        Player playerO = new ManualPlayer("O");
        int numberOfOptions = 9;
        //arbitro 
        Referee referee = new Referee(numberOfOptions, new EndT(), new WinnerT(), new ValidPlatT(), new shufflerT(), new TurnFort());
        // información general del juego
        GameInformation gm = new GameInformation(3, new weight(x => 1), new makerRecordsT());
        int aux = 0;
        //inicialmente todos los jugadores tendrán el equivalente a todas las fichas asignadas a las entradas del tablero
        //se considera que siempre tendrán las fichas no jugadas anteriormente.
        //repartición de fichas...
        referee.shuffler.Shuffle(playerO, gm, ref aux, referee, 3);
        referee.shuffler.Shuffle(playerX, gm, ref aux, referee, 3);
        // apartir de turnoP y turnosInd se determina a quien le toca jugar, se verá según paridad de turno dado que en tictactoe siempre inicia el 
        //jugador X
        Player[] turnosP = { playerX, playerO };
        int[] turnosInd = { 0, 1 };
        //Creando info que se le dará al jugador, aquí solo interesa mantener actualizado las fichas que matchean durante el partido ya que es lo único que el jugador empleará
        //para determinar partida por el momento, esto puede variar.
        //Todas las fichas tendrán igual peso por el momento, esto puede variar.
        //Pesador...
        weight weight = new weight(x => 1);
        InformationForPlayer info = referee.ProvidedInformation(referee, gm, playerX, new match(delegate (Records records1, int optionRecord1, Records records2, int optionRecord2, List<Records> recordsOptions) { return recordsOptions.Contains(records1); }), weight);
        //mientras no haya terminado el juego...
        //termina cuando alguno de los jugadores hayan ganado o cuando ya no queden fichas para jugar...
        while (!referee.finalized.EndGame(gm, referee, ref aux))
        {
            Console.Clear();
            //imprime tablero
            p.Print(gm, referee, 3, turnosP);
            //determinando a quien le toca jugar.
            referee.turnPlayer.Turn(turnosInd, aux);
            info.matchedRec = new List<(Records rcd, int weight)>();
            foreach (var item in referee.AsignedRecords[playerO])
            {
                info.matchedRec.Add((item, weight(item)));
            }
            System.Console.WriteLine("Turno de " + turnosP[turnosInd[1]].id);
            jugada jug = turnosP[turnosInd[0]].GiveMeRecords(info, referee);
            //jugada jug=new jugada(0,new Records(new List<int>{0,1}));
            List<Records> aux2 = referee.AsignedRecords[playerO].ToList();
            foreach (var item in aux2)
            {
                if (item.element1 == jug.record.element1 && item.element2 == jug.record.element2)
                {
                    aux2.Remove(item);
                    break;
                }
            }
            aux2.Remove(jug.record);
            referee.AsignedRecords[playerO] = aux2.ToList();
            referee.AsignedRecords[playerX] = aux2.ToList();
            gm.PlayTtt(turnosP[0], turnosP, turnosInd, jug);
        }
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Clear();
        System.Console.WriteLine("El ganador es: " + referee.Winner.Win(referee, gm));
        p.Print(gm, referee, 3, turnosP);
        Console.ForegroundColor = ConsoleColor.Gray;


    }
    #endregion

}

