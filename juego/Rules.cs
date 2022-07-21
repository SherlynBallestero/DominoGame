using System.Runtime.InteropServices;

namespace juego;
public delegate int weight(Records records);
//cambiar esto a pasarle dos fichas
public delegate bool match(Records recordsOption,int option,Records records,int recordsPart,List<Records> recordsOptions);
public delegate bool matcht(Records records,List<Records> recordsOptions);

public interface IMakerRecords 
{
     public  List<Records>  MakingRecords(int cant);
}
public interface ITurnPlayer
{
    /// <summary>Determina la distibucion de los turnos de los jugadores</summary>
    /// <param name="turns">Distribucion de los turnos de los jugadores</param>
    /// <param name="ind">Indice del jugador que le corresponde jugar</param>
    public void Turn(int[] turns, int ind);
}
public interface IFinalized
{
    public bool EndGame(GameInformation gm, Referee referee,ref int max);
}
public interface IWinner
{
    public string Win(Referee referee,GameInformation gm);
}
public interface IValidator
{    
   public bool ValidPlay(jugada jugada, GameInformation gi );
}
public interface IShuffler
{
    public void Shuffle(Player player, GameInformation gi, ref int index, Referee rf, [Optional] int cant);

}
public interface IPrinter
{
    public void Print(GameInformation gm, Referee referee, int cant,params Player[] players);
}