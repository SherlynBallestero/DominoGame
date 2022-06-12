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
        GameInformation gi = new GameInformation();
        Referee referee = new Referee(10);
        referee.DistributeRecord(A, 9);
        referee.DistributeRecord(B, 9);
        referee.DistributeRecord(C, 9);
        referee.DistributeRecord(D, 9);
      //  referee.DistributeRecord(C, 9);
       // referee.DistributeRecord(D, 9);

        while (!referee.EndGame(gi))
        {
            //caminando por cada jugador siempre que el juego no haya acabado
            foreach (Player item in referee.AsignedRecords.Keys)
            {
                //verificar que el jugador no se pase
                if (referee.HavesARecord(gi, item))
                {
                    
                    jugada jugadaAux = item.GiveMeRecords(gi, referee);
                    while(!referee.ValidPlay(jugadaAux,gi))
                    {
                        System.Console.WriteLine( "wrong!!!!!"  );
                        jugadaAux=item.GiveMeRecords(gi,referee);                       
                    }
                    referee.Play(jugadaAux,gi);
                    referee.AsignedRecords[item].Remove(jugadaAux.record);
                }
                else
                {
                    System.Console.WriteLine( "paseeee para "+ item.id  );
                }
            }
        }
        referee.Win();
    }
}

