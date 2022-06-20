namespace juego;
public class clasicWinner : IWinner
{
    public Player Win(Referee referee,weight weight)
    {

        double min = (double)int.MaxValue;
        double aux = 0;
        Player playerW = new Player("this is for aux");
        foreach (var item in referee.AsignedRecords.Keys)
        {
            if (referee.AsignedRecords[item].Count != 0)
            {
                foreach (var item2 in referee.AsignedRecords[item])
                {
                    aux += weight(item2);
                }
                if (aux < min)
                {
                    min = aux;
                    playerW = item;
                }
            }
            else
            {
                Console.BackgroundColor=ConsoleColor.Green;
                System.Console.WriteLine("the winneeeeer is "+ item.id);
                Console.BackgroundColor=ConsoleColor.Black;
                return item;
            }
        }
        Console.BackgroundColor=ConsoleColor.Green;
        System.Console.WriteLine("the winneeeeer is "+ playerW.id);
        Console.BackgroundColor=ConsoleColor.Black;
        return playerW;
    }
}
public class clasicEnd:IFinalized
{
      public bool EndGame(GameInformation gm, Referee rf)
    {
        if (gm.OptionsToPlay.Count == 0) return false;

        //se vera si todos los jugadores se pasaron(o sea de entre las opciones de juego ninguna ficha de ningun jugador 
        //satisface estas opciones) o si hay algn jugador que no tenga fichas.
        int aux1 = 0;
        foreach (var item in rf.AsignedRecords.Keys)
        {
            if (rf.AsignedRecords[item].Count == 0) return true;
            //asigned record en item me da las lista con fichas que tiene cada jugador
            int aux2 = 0;
            foreach (var item2 in rf.AsignedRecords[item])
            {
                //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
                if (!gm.OptionsToPlay.Contains(item2.element1) && !gm.OptionsToPlay.Contains(item2.element2))
                {
                    //verificando si el jugador contiene fichas validas para el juego.
                    aux2++;
                }

            }
            if (aux2 == rf.AsignedRecords[item].Count) aux1++;
        }
        if (aux1 == rf.AsignedRecords.Count)
        {
            return true;
        }
        return false;

    }
}