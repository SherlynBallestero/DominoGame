namespace juego;
public class Program
{
    public static void Main(string[] argd)
    {
        //haciendo instancias de las clases a emplear
        Player A = new Player("01A");
        Player B = new Player("02B");
      //  Player C = new Player("03C");
       // Player D = new Player("04D");
        GameInformation gi = new GameInformation();
        Referee referee = new Referee(10);
        referee.DistributeRecord(A, 9);
        referee.DistributeRecord(B, 9);
      //  referee.DistributeRecord(C, 9);
       // referee.DistributeRecord(D, 9);

        while (!referee.EndGame(gi))
        {
            foreach (Player item in referee.AsignedRecords.Keys)
            {
                if (referee.HavesARecord(gi, item))
                {
                    
                    Records recordsaux = item.GiveMeRecords(gi, referee);
                    referee.validPlay(recordsaux, gi, out bool valid);
                    
                    if (valid)
                    {
                        referee.AsignedRecords[item].Remove(recordsaux);
                    }
                }
            }
        }
        referee.Win();
    }
}

