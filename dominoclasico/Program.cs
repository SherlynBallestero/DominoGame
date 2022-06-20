namespace juego;
public class Program
{
    public static void Main(string[] argd)
    {
        //haciendo instancias de las clases a emplear
        Player A = new Player("01A");
        Player B = new Player("02B");
       Player C = new Player("03C");
        Player D = new Player("04D");
        int numberOfOptions=7;
        GameInformation gi = new GameInformation(numberOfOptions);
        Referee referee = new Referee(numberOfOptions,new ter());
        int index=0;
        referee.Shuffle(A,gi,ref index);
        referee.Shuffle(B,gi,ref index);
    System.Console.WriteLine( "hasta aqui"  );

        while (!referee.classic.EndGame(gi,referee))
        {
            //caminando por cada jugador siempre que el juego no haya acabado
            foreach (Player item in referee.AsignedRecords.Keys)
            {    
                if(referee.classic.EndGame(gi,referee)) break;
                //verificar que el jugador no se pase
                if (referee.HavesARecord(gi, item))
                {
                    
                    jugada jugadaAux = item.GiveMeRecords(gi, referee);
                    while(!referee.ValidPlay(jugadaAux,gi))
                    {
                        Console.ForegroundColor=ConsoleColor.Red;
                        System.Console.WriteLine( "wrong!!!!!"  );
                        Console.ForegroundColor=ConsoleColor.Gray;

                        jugadaAux=item.GiveMeRecords(gi,referee);                       
                    }
                    referee.Play(jugadaAux,gi);
                    referee.AsignedRecords[item].Remove(jugadaAux.record);
                }
                else
                {
                    Console.ForegroundColor=ConsoleColor.Yellow;
                    System.Console.WriteLine( "paseeee para "+ item.id  );
                    Console.ForegroundColor=ConsoleColor.Gray;
                    
                }
            }
        }
        referee.Win();
     }
}

