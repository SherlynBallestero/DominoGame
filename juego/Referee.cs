
namespace juego;
//public  delegate int Factorial(int x);
public class Referee
{
    public int numberOfOptions;
    //constructor....
    public Referee(int numberOfOptions)
    {
        this.numberOfOptions = numberOfOptions;
        this.AsignedRecords = new Dictionary<Player, List<Records>>();
        // this.cantCaras=cantCaras;
    }
    //formula de combinacion con repeticion para saber las cantidad de fichas total que se pueden jugar
    // public int TotalRecords{get{return Factorial(numberOfOPtions+cantCaras-1)/Factorial(cantCaras)*Factorial(numberOfOPtions-1); }}

    //almacenar las fichas repartidas a cada jugador por torneo 
    public Dictionary<Player, List<Records>> AsignedRecords { get; set; }

    //public int  Factorial(int a){int b=0;for(int i=1;i<=a;i++) {b =b*i;}return b;}
    //trabajar este metodo como delegado o lambda o lo que sea


    ///<summary>
    /// Esta funcion reparte fichas a cierto jugador,luego registra al jugador si no ha sido registrado 
    ///y registra las fichas agregadas a su mano   
    ///</summary>
    // // // public void DistributeRecord(Player player, int cant)
    // // // {
    // // //     //cont sirve para asegurar crear el numero de fichas indicado.
    // // //     int cont = 0;
    // // //     List<Records> aux = new List<Records>();
    // // //     //creando ficha... 
    // // //     while (cont < cant + 1)
    // // //     {
    // // //         List<int> ElementsAux = new List<int>();
    // // //         //haciendo random los numeros para cada cara de la ficha...
    // // //         Random random = new Random();
    // // //         bool correct = true;
    // // //         while (correct)
    // // //         {
    // // //             ElementsAux.Add(random.Next(0, numberOfOptions));
    // // //             ElementsAux.Add(random.Next(0, numberOfOptions));
    // // //             if (!aux.Contains(new Records(ElementsAux))) correct = false;//hacer un brake
    // // //             if (AsignedRecords.Count != 0)
    // // //             {
    // // //                 foreach (var item in AsignedRecords.Keys)
    // // //                 {

    // // //                     foreach (var item2 in AsignedRecords[item])
    // // //                     {
    // // //                         if (ElementsAux.Contains(item2.totalElements[0]) && ElementsAux.Contains(item2.totalElements[1]))
    // // //                         {
    // // //                             correct = true;
    // // //                         }

    // // //                     }
    // // //                 }
    // // //             }
    // // //         }

    // // //         //ent cuenta si se vuelve a crear ficha
    // // //         //si no se ha asignado a ningun jugador o se encuentra en las asignadas a este antes 
    // // //         Records auxRecords = new Records(ElementsAux);
    // // //         aux.Add(auxRecords);
    // // //         cont++;

    // // //     }
    // // //     AsignedRecords.Add(player, aux);

    // // //     //  System.Console.WriteLine(AsignedRecords.Count); 
    // // // }
    public void DistributeRecord(Player player, int cant)
    {
        bool[,]recordUsed=new bool[cant+1,cant+1];
        if(AsignedRecords.Count!=0)
        {
            foreach (var item in AsignedRecords.Keys)
            {
                foreach (var item2 in AsignedRecords[item])
                {
                    recordUsed[item2.totalElements[0],item2.totalElements[1]]=true;
                    recordUsed[item2.totalElements[1],item2.totalElements[0]]=true;
                }
            }
        }
       
        List<Records> aux = new List<Records>();
        int cont = 0;
        while (cont < cant + 1)
        {
             bool done=false;
            List<int> ElementsAux = new List<int>();
            while (!done)
            {

                ElementsAux = new List<int>();
                Random random = new Random();
                ElementsAux.Add(random.Next(0, numberOfOptions-1));
                ElementsAux.Add(random.Next(0, numberOfOptions-1));
                if (recordUsed[ElementsAux[0], ElementsAux[1]])
                {
                    continue;
                }
                    recordUsed[ElementsAux[0], ElementsAux[1]] = true;
                    recordUsed[ElementsAux[1], ElementsAux[0]] = true;
                    done = true;

            }
            Records records=new Records(ElementsAux);
            aux.Add(records);
            cont++;
        }
        AsignedRecords.Add(player, aux);
        

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
                gi.totalRecords.Add(jugada.record);
            }
            else if (gi.OptionsToPlay[jugada.position % 2] == jugada.record.totalElements[1])
            {
                gi.OptionsToPlay.Remove(jugada.record.totalElements[1]);
                gi.OptionsToPlay.Add(jugada.record.totalElements[0]);
                gi.totalRecords.Add(jugada.record);
            }
        }
        else
        {
            gi.OptionsToPlay.Add(jugada.record.totalElements[0]);
            gi.OptionsToPlay.Add(jugada.record.totalElements[1]);
            gi.totalRecords.Add(jugada.record);
        }
    }
    ///<summary>
    ///Nos dice true o false en dependencia de si el juego llego a su final, un juego llega a su final 
    ///si alguno de los jugadores se quedan sin ficha o si ya no se puede jugar mas, o sea todos los jugadores
    ///han pasado turno dado que no tienen fichas que encaje con las opciones disponibles en el juego.
    ///</summary>
    public bool EndGame(GameInformation gm)
    {
        if (gm.OptionsToPlay.Count == 0) return false;

        //se vera si todos los jugadores se pasaron(o sea de entre las opciones de juego ninguna ficha de ningun jugador 
        //satisface estas opciones) o si hay algn jugador que no tenga fichas.
        int aux1 = 0;
        foreach (var item in AsignedRecords.Keys)
        {
            if (AsignedRecords[item].Count == 0) return true;
            //asigned record en item me da las lista con fichas que tiene cada jugador
            int aux2 = 0;
            foreach (var item2 in AsignedRecords[item])
            {
                //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
                if (!gm.OptionsToPlay.Contains(item2.totalElements[0]) && !gm.OptionsToPlay.Contains(item2.totalElements[1]))
                {
                    //verificando si el jugador contiene fichas validas para el juego.
                    aux2++;
                }

            }
            if (aux2 == AsignedRecords[item].Count) aux1++;
        }
        if (aux1 == AsignedRecords.Count)
        {
            return true;
        }
        return false;

    }
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
        System.Console.WriteLine(playerW.id);
        return playerW;
    }


}
