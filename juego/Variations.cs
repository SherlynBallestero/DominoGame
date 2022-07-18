using System.Runtime.InteropServices;

namespace juego;
#region winner
public class clasicWinner : IWinner
{
    public string Win(Referee referee, GameInformation gm)
    {

        double min = (double)int.MaxValue;
        double aux = 0;
        //Player playerW = new Player("this is for aux");
        string winner = "";
        foreach (var item in referee.AsignedRecords.Keys)
        {
            if (referee.AsignedRecords[item].Count != 0)
            {
                foreach (var item2 in referee.AsignedRecords[item])
                {
                    aux += gm.weight(item2);
                }
                if (aux < min)
                {
                    min = aux;
                    winner = item.id;
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                System.Console.WriteLine("the winneeeeer is " + item.id);
                Console.BackgroundColor = ConsoleColor.Black;
                return item.id;
            }
        }
        Console.BackgroundColor = ConsoleColor.Green;
        System.Console.WriteLine("the winneeeeer is " + winner);
        Console.BackgroundColor = ConsoleColor.Black;
        return winner;
    }
}
///<summary>
///este ganador se basa en dado el numero de fichas de los jugadores y el puntaje gana el que tenga mas baja la multiplicacion
///entre estos parametros.
///</summary>
public class Winner : IWinner
{

    public string Win(Referee referee, GameInformation gm)
    {
        double max = (double)int.MinValue;
        double aux = 0;
        string winner = "";
        foreach (var item in referee.AsignedRecords.Keys)
        {
            if (referee.AsignedRecords[item].Count != 0)
            {
                foreach (var item2 in referee.AsignedRecords[item])
                {
                    aux += gm.weight(item2);
                }
                aux = aux * referee.AsignedRecords[item].Count;
                if (aux > max)
                {
                    max = aux;
                    winner = item.id;
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                System.Console.WriteLine("the winneeeeer is " + item.id);
                Console.BackgroundColor = ConsoleColor.Black;
                return item.id;
            }
        }
        Console.BackgroundColor = ConsoleColor.Green;
        System.Console.WriteLine("the winneeeeer is " + winner);
        Console.BackgroundColor = ConsoleColor.Black;
        return winner;
    }
}

#endregion 
#region Finalized
///<summary>
///Variante clasica de finalizar. Se termina una partida cuado no hayan fichas que matcheen con las opciones a jugar  
///entre las fichas de los jugadores.
///</summary>
public class clasicEnd : IFinalized
{
    public match match;
    public clasicEnd(match match)
    {
        this.match = match;
    }
    public bool EndGame(GameInformation gm, Referee rf, ref int max)
    {
        if (gm.OptionsToPlay.Count == 0) return false;

        //se vera si todos los jugadores se pasaron(o sea de entre las opciones de juego ninguna ficha de ningun jugador 
        //satisface estas opciones) o si hay algun jugador que no tenga fichas.
        int aux1 = 0;
        foreach (var item in rf.AsignedRecords.Keys)
        {
            if (rf.AsignedRecords[item].Count == 0) return true;
            //asigned record en item me da las lista con fichas que tiene cada jugador
            int aux2 = 0;
            foreach (var item2 in rf.AsignedRecords[item])
            {
                //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
                if (gm.OptionsToPlay[0].option != item2.element1 && gm.OptionsToPlay[0].option != item2.element2 && gm.OptionsToPlay[1].option != item2.element1 && gm.OptionsToPlay[1].option != item2.element2)
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
///<summary>
///Variante de terminacion cuya condicion es que haya un numero random de puntos en mesa, es decir dado un numero entre 0 y
///total de punto entre todas las fichas jugables, finaliza cuando haya en mesa esa puntuacion de suma entre todas las fichas
///ya seleccionadas por los jugadores.
///</summary>
public class EndGameAbovePoints : IFinalized
{
    public match match;
    public EndGameAbovePoints(match match)
    {
        this.match = match;
    }
    //cosas para cambiar en este metodo: pasar weight en el contructor de gameInf para no tener que pasarselo ni a este
    //metodo ni a win, calcular el random una unica vez y que no varie
    public bool EndGame(GameInformation gm, Referee referee, ref int max)
    {
        //estableciendo limite superior(valor), para determinar un random correspondiente al numero maximo de puntos que
        //debe haber en mesa para que el juego finalice.Llevarlo por ref para que sea establecido una unica vez durante
        //el juego.
        int count = 0;
        foreach (var item in referee.AsignedRecords.Keys)
        {
            if (referee.HavesARecord(referee, gm, item, match)) count++;
        }
        if (count == 0) return true;
        if (max == 0)
        {
            max = gm.shuffledPoints(referee);
            Random random = new Random();
            max = random.Next(max);
        }
        System.Console.WriteLine(max);
        //comprobando que la mesa no sobrepase dicha soluction
        return (gm.PointsInGame() >= max);
    }
}

#endregion
#region  validator
///<summary>
//arreglar comentario.................................................
///Esta funcion retorna si una jugada es valida de acuedo con el estado del tablero y la jugada, la jugada
/// viene dada por una ficha y una de las opciones de juego, es decir, esta en el caso en que solo se puede
///jugar fichas que contengan 3 o 4, q son las opciones actualmente viables en el partido, si la opcion 
///seleccionada por el jugador, digase por ej 3 no se puede conectar con la ficha que juega ent la jugada 
///no es valida. 
///</summary>
//asegurarme que siempre se le pase a la clase validator un match en el constructor,analizar si es estrictamente
public class validator : IValidator
{
    public match match;
    public validator(match match)
    {
        this.match = match;
    }
    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        if (gi.OptionsToPlay.Count == 0) return true;
        //arreglar el match q no sea tan grande
        return (match(gi.OptionsToPlay[jugada.position % 2].records, gi.OptionsToPlay[jugada.position % 2].option, jugada.record, jugada.record.element1, gi.RecordsInGame) || match(gi.OptionsToPlay[jugada.position % 2].records, gi.OptionsToPlay[jugada.position % 2].option, jugada.record, jugada.record.element2, gi.RecordsInGame));
    }
}
///jugada valida cuando la ficha jugada tenga un peso par si una nueva ficha dada por las dos caras correspondiente
///con los dos int que tenemos como opciones de juego  es par e impar en caso 
///contrario ademas de que las fichas matcheen. 
public class validatorEvenOdd : IValidator
{
    public match match;
    public validatorEvenOdd(match match)
    {
        this.match = match;
    }

    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        //nueva ficha dada por la union de las caras opciones de juego y su peso correspondiente.

        int weightAux = gi.weight(gi.OptionsToPlay[jugada.position % 2].records);

        bool match1 = match(gi.OptionsToPlay[jugada.position % 2].records, gi.OptionsToPlay[jugada.position % 2].option, jugada.record, jugada.record.element1, gi.RecordsInGame);
        bool match2 = match(gi.OptionsToPlay[jugada.position % 2].records, gi.OptionsToPlay[jugada.position % 2].option, jugada.record, jugada.record.element2, gi.RecordsInGame);
        if (gi.OptionsToPlay.Count == 0) return true;
        if (match1 || match2)
        {
            return gi.weight(jugada.record) % 2 == weightAux % 2;
        }
        return false;
    }
}
#endregion
#region shuffler
public class shuffler : IShuffler
{
    ///<summary>
    /// Esta funcion reparte fichas a cierto jugador,luego registra al jugador si no ha sido registrado 
    ///y registra las fichas agregadas a su mano,index va a marcar la ultima ficha repartida,solo se podran repartir las que esten
    ///en la posicion index en adelante  
    ///<summary>
    public void Shuffle(Player player, GameInformation gi, ref int index, Referee referee, [Optional] int cant)
    {
        List<Records> records = gi.RecordsInOrder;
        if (cant == 0) cant = referee.numberOfOptions;
        List<Records> aux = new List<Records>();
        int indexaux = 0;

        //estamos verificando hasta que parte del conjunto de fichas ya barajeadas vamos a tomar para reprtir, aqui vemos que se 
        //repartan siempre por ejemplo si el dominó es 
        if (index < records.Count)
        {
            //primero comprobamos que tenemos fichas para dar, si no tenemos suficiente pero tenemos para dar damos todas
            if (index + cant <= records.Count)
            {
                indexaux = index + cant;
            }
            else
            {
                indexaux = records.Count;

            }
            //asignando la cantidad de fichas requeridas al jugador
            for (int i = index; i < indexaux; i++)
            {
                aux.Add(records[i]);
            }
            if (!referee.AsignedRecords.ContainsKey(player))
            {
                referee.AsignedRecords.Add(player, aux);
            }
            else
            {
                foreach (var item in aux)
                {
                    referee.AsignedRecords[player].Add(item);
                }
            }
            index = indexaux;


        }

    }
}
///<summary>
/// repartimos fichas que tengan suma de puntos par a jugadores de indice par e impar a jugadores de indice impar
///</summary>
public class shuffler2 : IShuffler
{
    public void Shuffle(Player player, GameInformation gi, ref int index, Referee rf, [Optional] int cant)
    {

        List<Records> records = gi.RecordsInOrder;
        //cada vez que se asigne ficha se borrara esa ficha de records in order
        if (cant == 0) cant = rf.numberOfOptions;
        bool even = rf.AsignedRecords.Count % 2 == 0;
        HashSet<Records> used = new HashSet<Records>();
        if (rf.AsignedRecords.ContainsKey(player))
        {
            int cont = 0;
            foreach (var item in rf.AsignedRecords.Keys)
            {
                if (item.id == player.id) cont++;
            }
            even = cont % 2 == 0;
        }
        else
        {
            rf.AsignedRecords.Add(player, new List<Records>());
        }

        int limit = 0;
        foreach (var item in records)
        {
            if (even)
            {
                if ((item.element1 + item.element2) % 2 == 0)
                {
                    if (!used.Contains(item))
                    {
                        rf.AsignedRecords[player].Add(item);
                        limit++;
                        used.Add(item);
                        gi.RecordsInGame.Add(item);

                    }
                }
            }
            else
            {
                if ((item.element1 + item.element2) % 2 != 0)
                {
                    if (!used.Contains(item))
                    {
                        rf.AsignedRecords[player].Add(item);
                        limit++;
                        used.Add(item);
                        gi.RecordsInGame.Add(item);
                    }

                }
            }
            if (limit == cant) break;
        }
        foreach (var item in used)
        {
            gi.RecordsInOrder.Remove(item);
        }



    }
}

#endregion
#region Turno

public class TurnPlayerClassic : ITurnPlayer
{
    public void Turn(int[] turns, int ind)
    {
    }
}

public class TurnPlayerInvert : ITurnPlayer
{
    public void Turn(int[] turns, int ind)
    {
        int i = ind;
        int j = ind;
        while (true)
        {
            i++;
            if (i == turns.Length) i = 0;
            if (i == j) break;
            j--;
            if (j < 0) j = turns.Length - 1;
            if (i == j) break;
            (turns[i], turns[j]) = (turns[j], turns[i]);
        }
    }
}

public class TurnPlayerRepeatPlay : ITurnPlayer
{
    public void Turn(int[] turns, int ind)
    {
        int i = ind;
        int j = ind - 1;
        int stop = (ind == turns.Length - 1) ? 0 : ind + 1;
        int change = turns[ind];
        while (true)
        {
            if (i == -1) i = turns.Length - 1;
            if (j == -1) j = turns.Length - 1;
            if (i == stop) break;
            turns[i] = turns[j];
            j--;
            i--;
        }

        turns[stop] = change;
    }
}
#endregion
public class makerRecords : IMakerRecords
{
    public List<Records> MakingRecords(int cant)
    {
        List<Records> records = new List<Records>();
        for (int i = 0; i < cant; i++)
        {
            for (int j = i; j < cant; j++)
            {

                records.Add(new Records(new List<int>() { i, j }));
            }
        }
        Random random = new Random();
        int n = records.Count;
        while (n > 1)
        {
            n--;
            int i = random.Next(n + 1);
            Records temp = records[i];
            records[i] = records[n];
            records[n] = temp;
        }
        return records;
    }
}
#region TicTacToe
public class PrienterTictactoe : IPrinter
{

    public void Print(GameInformation gm, Referee referee, int cant, Player[] players)
    {
        List<Records> records = gm.makerRecords.MakingRecords(cant);
        string answer = "";
        int count = 1;
        
        foreach (var item in records)
        {
            if (!(gm.turnPlayed is null) && gm.turnPlayed.Count != 0)
            {
                bool change=false;
                if (gm.turnPlayed.ContainsKey(players[0]))
                {
                    if (gm.turnPlayed[players[0]].Any(x => x.record.element1 == item.element1 && x.record.element2 == item.element2)){
                    answer += "O ";
                    change=true;
                        
                    } 

                }
                if (gm.turnPlayed.ContainsKey(players[1]))
                {
                    if (gm.turnPlayed[players[1]].Any(x => x.record.element1 == item.element1 && x.record.element2 == item.element2)) answer += "X ";
                    else if (!change)answer+="- ";
                }

            }
            else
            {
                answer += "- ";
            }
            count++;
            if (count == 4)
            {
                System.Console.WriteLine(answer);
                System.Console.WriteLine("");
                answer = "";
                count = 1;
            }

        }
    }
}
public class TurnFort : ITurnPlayer
{
    public void Turn(int[] turns, int ind)
    {
        if (ind % 2 == 0)
        {
            int aux = turns[0];
            turns[0] = turns[1];
            turns[1] = aux;
        }
    }
}
public class ValidPlatT : IValidator
{
    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        throw new NotImplementedException();
    }
}
public class WinnerT : IWinner
{
    public Dictionary<string, List<List<Records>>> winnersRecords = new Dictionary<string, List<List<Records>>>();
    public void CreaterWinner(GameInformation gm, int cant)
    {
        List<Records> records = gm.makerRecords.MakingRecords(cant);
        List<List<Records>> aux = new List<List<Records>>();
        //agregando filas ganadoras...
        foreach (var item in OrdenFile(records, cant))
        {
            aux.Add(item.ToList());
        }
        winnersRecords.Add("files", aux);
        //agregando columnas ganadoras...
        aux = new List<List<Records>>();
        foreach (var item in OrdenRow(records, cant))
        {
            aux.Add(item.ToList());
        }
        winnersRecords.Add("Row", aux);
        //agregando diagonal...
        aux = new List<List<Records>>();
        aux.Add(new List<Records> { records[0], records[4], records[8] });

        winnersRecords.Add("diagonal", aux);

    }
    //filas
    public IEnumerable<IEnumerable<Records>> OrdenFile(List<Records> records, int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            yield return records.Skip(i * cant).Take(cant + i);
        }

    }
    //columnas
    public IEnumerable<IEnumerable<Records>> OrdenRow(List<Records> records, int cant)
    {
        List<Records> row1 = new List<Records>();
        List<Records> row2 = new List<Records>();
        List<Records> row3 = new List<Records>();
        for (int i = 0; i < records.Count; i++)
        {
            if (i % 3 == 0) row1.Add(records[i]);
            else if (i % 3 == 1) row2.Add(records[i]);
            else row3.Add(records[i]);
        }
        yield return row1;
        yield return row2;
        yield return row3;

    }

    public string Win(Referee referee, GameInformation gm)
    {

        if (winnersRecords.Count == 0)
        {
            CreaterWinner(gm, 3);
        }
        //Localizando fichas jugadas por cada jugador...
        List<Records> PlayedX = new List<Records>();
        List<Records> PlayedO = new List<Records>();
        bool aux = false;
        if (gm.turnPlayed is null) return "Tabla";

        foreach (var item in gm.turnPlayed.Keys)
        {
            foreach (var item2 in gm.turnPlayed[item])
            {
                if (!aux)
                {
                    PlayedX.Add(item2.record);
                }
                else
                {
                    PlayedO.Add(item2.record);
                }
            }
            aux = true;
        }
        //Verificando si alguno de los jugadores contienen las jugadas ganadoras...
        foreach (var item in winnersRecords.Keys)
        {
            foreach (var item2 in winnersRecords[item])
            {
                if (item2.All(x => PlayedX.Any(y => y.element1 == x.element1 && y.element2 == x.element2))) return "X";
                if (item2.All(x => PlayedO.Any(y => y.element1 == x.element1 && y.element2 == x.element2))) return "O";

            }
        }
        return "Tabla";

    }
}
public class EndT : IFinalized
{
    public bool EndGame(GameInformation gm, Referee referee, ref int max)
    {

        if (referee.Winner.Win(referee, gm) != "Tabla") return true;

        foreach (var item in referee.AsignedRecords.Keys)
        {
            if (referee.AsignedRecords[item].Count != 0) return false;
        }
        return true;

    }
}
public class makerRecordsT : IMakerRecords
{
    public List<Records> MakingRecords(int cant)
    {
        List<Records> records = new List<Records>();
        for (int i = 0; i < cant; i++)
        {
            for (int j = 0; j < cant; j++)
            {

                records.Add(new Records(new List<int>() { i, j }));
            }
        }
        return records;
    }
}
public class shufflerT : IShuffler
{
    public void Shuffle(Player player, GameInformation gi, ref int index, Referee rf, [Optional] int cant)
    {
        rf.AsignedRecords.Add(player, gi.makerRecords.MakingRecords(cant));
    }
}
#endregion