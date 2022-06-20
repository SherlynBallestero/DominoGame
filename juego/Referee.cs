using System.Runtime.InteropServices;
namespace juego;
//public  delegate int Factorial(int x);
public class Referee
{
    public ITerminationConditioner classic;
    public int numberOfOptions;
    //constructor....
    public Referee(int numberOfOptions,ITerminationConditioner classic)
    {
        this.numberOfOptions = numberOfOptions;
        this.AsignedRecords = new Dictionary<Player, List<Records>>();
        this.classic=classic;
        // this.cantCaras=cantCaras;
    }
    

    //almacenar las fichas repartidas a cada jugador por torneo 
    public Dictionary<Player , List<Records>> AsignedRecords { get; set; }

    ///<summary>
    /// Esta funcion reparte fichas a cierto jugador,luego registra al jugador si no ha sido registrado 
    ///y registra las fichas agregadas a su mano,index va a marcar la ultima ficha repartida,solo se podran repartir las que esten
    ///en la posicion index en adelante  
    ///<summary>
    public void Shuffle(Player player, GameInformation gi, ref int index, [Optional] int cant )
    {
        List<Records> records=gi.RecordsInOrder;
        if(cant==0)cant=numberOfOptions;
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
            if (!AsignedRecords.ContainsKey(player))
            {
                AsignedRecords.Add(player, aux);
            }
            else
            {
                foreach (var item in aux)
                {
                    AsignedRecords[player].Add(item);
                }
            }
            index = indexaux;


        }


    }

    ///<summary>
    ///Dado el jugador actual me dice a quien le toca
    ///y asociar el orden en que jugara cada uno. 
    ///</summary>
    public Player NextTurn(Player actualPlayer)
    {
        bool aux = false;
        foreach (var item in AsignedRecords.Keys)
        {
            if (aux) return item;
            if (item == actualPlayer)
            {
                aux = true;
            }
        }

        Player auxplayer = actualPlayer;
        foreach (var item in AsignedRecords.Keys)
        {
            auxplayer = item; break;
        }
        return auxplayer;

    }

    ///<summary>
    ///Esta funcion retorna si una jugada es valida de acuedo con el estado del tablero y la jugada, la jugada
    /// viene dada por una ficha y una de las opciones de juego, es decir, esta en el caso en que solo se puede
    ///jugar fichas que contengan 3 o 4, q son las opciones actualmente viables en el partido, si la opcion 
    ///seleccionada por el jugador, digase por ej 3 no se puede conectar con la ficha que juega ent la jugada 
    ///no es valida. 
    ///</summary>
    public bool ValidPlay(jugada jugada, GameInformation gi)
    {
        if (gi.OptionsToPlay.Count == 0) return true;
        return (gi.OptionsToPlay[jugada.position % 2] == jugada.record.totalElements[0] || gi.OptionsToPlay[jugada.position % 2] == jugada.record.totalElements[1]);
    }
    ///<summary>
    /// esta funcion hace una jugada a partir de la jugada retornada por el player,o sea hace los cambios adecuados,como 
    /// darle a gameinformetion las nuevas opciones de juegpo y agregar al tablero la ficha en la posicion indicada por el jugador
    ///</summary>
    public void Play(jugada jugada, GameInformation gi)
    {
        //se ve si hay o no fichas en el tablero
        if (gi.OptionsToPlay.Count != 0)
        {
            //si hay fichas hay opciones en las que jugar, si no es libre de jugar cualquier ficha sin importar que opcion
            //sea seleccionada por el jugador
            if (gi.OptionsToPlay[jugada.position % 2] == jugada.record.totalElements[0])
            {
                //la opcion de jugar ofrecida por el arbitro es movida de acuerdo a cual de los lados de la ficha machee
                //o sea es removida la opcion que el jugador seleccopnes garantizada que es valida por el arbitro,pero por ejemplo 
                //si decido jugar el 8,9 y la opcion seleccionada es 8 ahora la nueva opcion sera 9.
                gi.OptionsToPlay.Remove(jugada.record.totalElements[0]);
                gi.OptionsToPlay.Add(jugada.record.totalElements[1]);
                gi.RecordsInGame.Add(jugada.record);
            }
            else if (gi.OptionsToPlay[jugada.position % 2] == jugada.record.totalElements[1])
            {
                gi.OptionsToPlay.Remove(jugada.record.totalElements[1]);
                gi.OptionsToPlay.Add(jugada.record.totalElements[0]);
                gi.RecordsInGame.Add(jugada.record);
            }
        }
        else
        {
            gi.OptionsToPlay.Add(jugada.record.totalElements[0]);
            gi.OptionsToPlay.Add(jugada.record.totalElements[1]);
            gi.RecordsInGame.Add(jugada.record);
        }
    }
    ///<summary>
    ///Nos dice true o false en dependencia de si el juego llego a su final, un juego llega a su final 
    ///si alguno de los jugadores se quedan sin ficha o si ya no se puede jugar mas, o sea todos los jugadores
    ///han pasado turno dado que no tienen fichas que encaje con las opciones disponibles en el juego.
    ///</summary>
    // // public bool EndGame(GameInformation gm)
    // // {
    // //     if (gm.OptionsToPlay.Count == 0) return false;

    // //     //se vera si todos los jugadores se pasaron(o sea de entre las opciones de juego ninguna ficha de ningun jugador 
    // //     //satisface estas opciones) o si hay algn jugador que no tenga fichas.
    // //     int aux1 = 0;
    // //     foreach (var item in AsignedRecords.Keys)
    // //     {
    // //         if (AsignedRecords[item].Count == 0) return true;
    // //         //asigned record en item me da las lista con fichas que tiene cada jugador
    // //         int aux2 = 0;
    // //         foreach (var item2 in AsignedRecords[item])
    // //         {
    // //             //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
    // //             if (!gm.OptionsToPlay.Contains(item2.totalElements[0]) && !gm.OptionsToPlay.Contains(item2.totalElements[1]))
    // //             {
    // //                 //verificando si el jugador contiene fichas validas para el juego.
    // //                 aux2++;
    // //             }

    // //         }
    // //         if (aux2 == AsignedRecords[item].Count) aux1++;
    // //     }
    // //     if (aux1 == AsignedRecords.Count)
    // //     {
    // //         return true;
    // //     }
    // //     return false;

    // // }
    public bool HavesARecord(GameInformation gm, Player player)
    {
        if (gm.OptionsToPlay.Count == 0) return true;
        int aux2 = 0;
        foreach (var item in AsignedRecords[player])
        {
            //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
            if (!gm.OptionsToPlay.Contains(item.totalElements[0]) && !gm.OptionsToPlay.Contains(item.totalElements[1]))
            {
                //verificando si el jugador contiene fichas validas para el juego.
                aux2++;
            }

        }
        if (aux2 == AsignedRecords[player].Count) return false;
        else return true;
    }
    ///<summary>
    /// Se dira cual jugador es el ganador,de acuerdo con el criterio de quien menso pesa tenga.
    ///</summary>
    public Player Win()
    {
        double min = (double)int.MaxValue;
        double aux = 0;
        Player playerW = new Player("this is for aux");
        foreach (var item in AsignedRecords.Keys)
        {
            if (AsignedRecords[item].Count != 0)
            {
                foreach (var item2 in AsignedRecords[item])
                {
                    aux += item2.weight();
                }
                if (aux < min)
                {
                    min = aux;
                    playerW = item;
                }
            }
            else
            {
                return item;
            }
        }
        ////
        Console.BackgroundColor=ConsoleColor.Green;
        System.Console.WriteLine("the winneeeeer is "+ playerW.id);
        Console.BackgroundColor=ConsoleColor.Black
        ;
        
        return playerW;
    }


}
public class ter:ITerminationConditioner
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
                if (!gm.OptionsToPlay.Contains(item2.totalElements[0]) && !gm.OptionsToPlay.Contains(item2.totalElements[1]))
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