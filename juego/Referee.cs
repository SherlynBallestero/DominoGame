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
    /// Esta funcion hace una jugada a partir de la jugada retornada por el player,o sea hace los cambios adecuados,como 
    /// darle a gameinformetion las nuevas opciones de juegpo y agregar al tablero la ficha en la posicion indicada por el jugador
    ///</summary>
    public void Play(jugada jugada, GameInformation gi, match match)
    {
        //se ve si hay o no fichas en el tablero por las que jugar.
        if (gi.OptionsToPlay.Count != 0)
        {
            //si hay fichas hay opciones en las que jugar, si no es libre de jugar cualquier ficha sin importar que opcion
            //sea seleccionada por el jugador

            if (match(gi.OptionsToPlay[jugada.position % 2].records, gi.OptionsToPlay[jugada.position % 2].option, jugada.record, jugada.record.element1))
            {
                //la opcion de jugar ofrecida por el arbitro es movida de acuerdo a cual de los lados de la ficha machee
                //o sea es removida la opcion que el jugador seleccopnes garantizada que es valida por el arbitro,pero por ejemplo 
                //si decido jugar el 8,9 y la opcion seleccionada es 8 ahora la nueva opcion sera 9.
                (Records, int) t = (jugada.record, jugada.record.element2);
                gi.OptionsToPlay.Remove(gi.OptionsToPlay[jugada.position % 2]);
                gi.OptionsToPlay.Add(t);
                gi.RecordsInGame.Add(jugada.record);
            }
            else if (match(gi.OptionsToPlay[jugada.position % 2].records, gi.OptionsToPlay[jugada.position % 2].option, jugada.record, jugada.record.element2))
            {
                (Records, int) t = (jugada.record, jugada.record.element1);
                gi.OptionsToPlay.Remove(gi.OptionsToPlay[jugada.position % 2]);
                gi.OptionsToPlay.Add(t);
                gi.RecordsInGame.Add(jugada.record);
            }
        }
        else
        {
            gi.OptionsToPlay.Add((jugada.record, jugada.record.element1));
            gi.OptionsToPlay.Add((jugada.record, jugada.record.element1));
            gi.RecordsInGame.Add(jugada.record);
        }
    }
    public bool HavesARecord(Referee referee, GameInformation gm, Player player, match match)
    {
        if (gm.OptionsToPlay.Count == 0) return true;
        int aux2 = 0;
        foreach (var item in referee.AsignedRecords[player])
        {
            //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
            //arreglar esto q esta muy feo
            if (!match(gm.OptionsToPlay[0].records, gm.OptionsToPlay[0].option, item, item.element1) && !match(gm.OptionsToPlay[1].records, gm.OptionsToPlay[1].option, item, item.element1) && !match(gm.OptionsToPlay[0].records, gm.OptionsToPlay[0].option, item, item.element2) && !match(gm.OptionsToPlay[1].records, gm.OptionsToPlay[1].option, item, item.element2))
            {
                //verificando si el jugador contiene fichas validas para el juego.
                aux2++;
            }

        }
        if (aux2 == referee.AsignedRecords[player].Count) return false;
        else return true;
    }
    public InformationForPlayer ProvidedInformation(Referee referee, GameInformation gm,Player player,match match,weight weight)
    {
        //formando diccionario de turnos pasados
        Dictionary<Player,List<int>> turnPass=new Dictionary<Player, List<int>>();
        foreach (var item in gm.turnPass.Keys)
        {
            turnPass.Add(item,gm.turnPass[item].ToList<int>());
        }
        //formando dictonary de jugada de cada player
        Dictionary<Player,List<jugada>> turnPlayed=new Dictionary<Player, List<jugada>>();
        foreach (var item in gm.turnPlayed.Keys)
        {    
            turnPlayed.Add(item,gm.turnPlayed[item].ToList<jugada>());
        }
         // formando lista de fichas que tiene el jugador con sus valores correspondientes
        List<(Records rcd,int weight)> records=new List<(Records rcd, int weight)>();
        //Buscando indices de las fichas que matchean con las opciones para jugar dada la lista anterior
         List<Records> matchedRec=new List<Records>();
        foreach (var item in referee.AsignedRecords[player])
        {
            records.Add((item,weight(item)));
            if(match(gm.OptionsToPlay[0].records,gm.OptionsToPlay[0].option,item,item.element1)||match(gm.OptionsToPlay[0].records,gm.OptionsToPlay[0].option,item,item.element2)||match(gm.OptionsToPlay[1].records,gm.OptionsToPlay[1].option,item,item.element1)||match(gm.OptionsToPlay[1].records,gm.OptionsToPlay[1].option,item,item.element2))
            {
                matchedRec.Add(item);
            }
        }
        return new InformationForPlayer(turnPass,turnPlayed,records,matchedRec);

    }
}

