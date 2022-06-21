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
#endregion 
#region Finalized
///<summary>
///Variante clasica de finalizar. Se termina una partida cuado no hayan fichas que matcheen con las opciones a jugar  
///entre las fichas de los jugadores.
///</summary>
public class clasicEnd : IFinalized
{
    public bool EndGame(GameInformation gm, Referee rf,ref int max)
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
///<summary>
///Variante de terminacion cuya condicion es que haya un numero random de puntos en mesa, es decir dado un numero entre 0 y
///total de punto entre todas las fichas jugables, finaliza cuando haya en mesa esa puntuacion de suma entre todas las fichas
///ya seleccionadas por los jugadores.
///</summary>
public class EndGameAbovePoints : IFinalized
{
    //cosas para cambiar en este metodo: pasar weight en el contructor de gameInf para no tener que pasarselo ni a este
    //metodo ni a win, calcular el random una unica vez y que no varie
    public bool EndGame(GameInformation gm, Referee referee,ref int max)
    {
        //estableciendo limite superior(valor), para determinar un random correspondiente al numero maximo de puntos que
        //debe haber en mesa para que el juego finalice.Llevarlo por ref para que sea establecido una unica vez durante
        //el juego.
        int count=0;
        foreach (var item in referee.AsignedRecords.Keys)
        {
            if(referee.HavesARecord(gm,item))count++;
        }
        if(count==0)return true;
        if(max==0)
        {
            max=gm.shuffledPoints(referee); 
            Random random=new Random();
            max=random.Next(max);
        }
        System.Console.WriteLine(max   );
        //comprobando que la mesa no sobrepase dicha soluction
        return(gm.PointsInGame()>=max);
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
public class validator : IValidator
{
    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        if (gi.OptionsToPlay.Count == 0) return true;
        return (gi.OptionsToPlay[jugada.position % 2] == jugada.record.element1 || gi.OptionsToPlay[jugada.position % 2] == jugada.record.element2);
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
#endregion
