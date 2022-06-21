using System.Runtime.InteropServices;
namespace juego;
//public  delegate int Factorial(int x);
public class Referee
{
    //condicion de terminado el juego
    public IFinalized finalized;
    //condicion de ganar juego
    public IWinner Winner;
    //condicion de juego valido
    public IValidator validator;
    //repartidor
    public IShuffler shuffler;
 
    public int numberOfOptions;
    //almacenar las fichas repartidas a cada jugador por torneo 
    public Dictionary<Player, List<Records>> AsignedRecords { get; set; }

    //constructor....
    public Referee(int numberOfOptions, IFinalized finalized, IWinner Winner, IValidator validator, IShuffler shuffler)
    {
        this.numberOfOptions = numberOfOptions;
        this.AsignedRecords = new Dictionary<Player, List<Records>>();
        this.finalized = finalized;
        this.Winner = Winner;
        this.validator = validator;
        this.shuffler = shuffler;
        // this.cantCaras=cantCaras;
    }


    ///<summary>
    ///Dado el jugador actual me dice a quien le toca
    ///y asociar el orden en que jugara cada uno. 
    //por ahora este metodo es inutil
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
            if (gi.OptionsToPlay[jugada.position % 2] == jugada.record.element1)
            {
                //la opcion de jugar ofrecida por el arbitro es movida de acuerdo a cual de los lados de la ficha machee
                //o sea es removida la opcion que el jugador seleccopnes garantizada que es valida por el arbitro,pero por ejemplo 
                //si decido jugar el 8,9 y la opcion seleccionada es 8 ahora la nueva opcion sera 9.
                gi.OptionsToPlay.Remove(jugada.record.element1);
                gi.OptionsToPlay.Add(jugada.record.element2);
                gi.RecordsInGame.Add(jugada.record);
            }
            else if (gi.OptionsToPlay[jugada.position % 2] == jugada.record.element2)
            {
                gi.OptionsToPlay.Remove(jugada.record.element2);
                gi.OptionsToPlay.Add(jugada.record.element1);
                gi.RecordsInGame.Add(jugada.record);
            }
        }
        else
        {
            gi.OptionsToPlay.Add(jugada.record.element1);
            gi.OptionsToPlay.Add(jugada.record.element2);
            gi.RecordsInGame.Add(jugada.record);
        }
    }

    public bool HavesARecord(GameInformation gm, Player player)
    {
        if (gm.OptionsToPlay.Count == 0) return true;
        int aux2 = 0;
        foreach (var item in AsignedRecords[player])
        {
            //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
            if (!gm.OptionsToPlay.Contains(item.element1) && !gm.OptionsToPlay.Contains(item.element2))
            {
                //verificando si el jugador contiene fichas validas para el juego.
                aux2++;
            }

        }
        if (aux2 == AsignedRecords[player].Count) return false;
        else return true;
    }

}

