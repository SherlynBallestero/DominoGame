using System.Runtime.InteropServices;

namespace juego;
#region winner
public class clasicWinner : IWinner
{
    public Player Win(Referee referee, GameInformation gm)
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
                    aux += gm.weight(item2);
                }
                if (aux < min)
                {
                    min = aux;
                    playerW = item;
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                System.Console.WriteLine("the winneeeeer is " + item.id);
                Console.BackgroundColor = ConsoleColor.Black;
                return item;
            }
        }
        Console.BackgroundColor = ConsoleColor.Green;
        System.Console.WriteLine("the winneeeeer is " + playerW.id);
        Console.BackgroundColor = ConsoleColor.Black;
        return playerW;
    }
}
///<summary>
///este ganador se basa en dado el numero de fichas de los jugadores y el puntaje gana el que tenga mas baja la multiplicacion
///entre estos parametros.
///</summary>
public class Winner : IWinner
{
    
    public Player Win(Referee referee, GameInformation gm)
    {
        double max = (double)int.MinValue;
        double aux = 0;
        Player playerW = new Player("this is for aux");
        foreach (var item in referee.AsignedRecords.Keys)
        {
            if (referee.AsignedRecords[item].Count != 0)
            {
                foreach (var item2 in referee.AsignedRecords[item])
                {
                    aux += gm.weight(item2);
                }
                aux=aux*referee.AsignedRecords[item].Count;
                if (aux > max)
                {
                    max = aux;
                    playerW = item;
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                System.Console.WriteLine("the winneeeeer is " + item.id);
                Console.BackgroundColor = ConsoleColor.Black;
                return item;
            }
        }
        Console.BackgroundColor = ConsoleColor.Green;
        System.Console.WriteLine("the winneeeeer is " + playerW.id);
        Console.BackgroundColor = ConsoleColor.Black;
        return playerW;
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
    public  clasicEnd(match match)
    {
        this.match=match;
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
                if (gm.OptionsToPlay[0].option!=item2.element1 && gm.OptionsToPlay[0].option!=item2.element2 && gm.OptionsToPlay[1].option!=item2.element1 && gm.OptionsToPlay[1].option!=item2.element2)
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
    public  EndGameAbovePoints(match match)
    {
        this.match=match;
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
            if (referee.HavesARecord(referee,gm, item,match)) count++;
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
    public  validator(match match)
    {
        this.match=match;
    }
    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        if (gi.OptionsToPlay.Count == 0) return true;
        //arreglar el match q no sea tan grande
        return (match(gi.OptionsToPlay[jugada.position % 2].records,gi.OptionsToPlay[jugada.position % 2].option,jugada.record, jugada.record.element1) || match(gi.OptionsToPlay[jugada.position % 2].records,gi.OptionsToPlay[jugada.position % 2].option,jugada.record, jugada.record.element2));
    }
}
///jugada valida cuando la ficha jugada tenga un peso par si una nueva ficha dada por las dos caras correspondiente
///con los dos int que tenemos como opciones de juego  es par e impar en caso 
///contrario ademas de que las fichas matcheen. 
public class validatorEvenOdd : IValidator
{
    public match match;
    public  validatorEvenOdd(match match)
    {
        this.match=match;
    }

    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        //nueva ficha dada por la union de las caras opciones de juego y su peso correspondiente.
        
        int weightAux = gi.weight(gi.OptionsToPlay[jugada.position%2].records);

        bool match1 = match(gi.OptionsToPlay[jugada.position % 2].records,gi.OptionsToPlay[jugada.position % 2].option,jugada.record, jugada.record.element1);
        bool match2 = match(gi.OptionsToPlay[jugada.position % 2].records,gi.OptionsToPlay[jugada.position % 2].option,jugada.record, jugada.record.element2);
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
