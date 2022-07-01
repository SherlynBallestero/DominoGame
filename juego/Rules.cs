using System.Runtime.InteropServices;

namespace juego;
public delegate int weight(Records records);
//cambiar esto a pasarle dos fichas
public delegate bool match(Records recordsOption,int option,Records records,int recordsPart);

public interface IFinalized
{
    public bool EndGame(GameInformation gm, Referee referee,ref int max);
}
public interface IWinner
{
    public Player Win(Referee referee,GameInformation gm);
}
public interface IValidator
{    
   public bool ValidPlay(jugada jugada, GameInformation gi );
}
public interface IShuffler
{
    public void Shuffle(Player player, GameInformation gi, ref int index, Referee rf, [Optional] int cant);

}