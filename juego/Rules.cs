using System.Runtime.InteropServices;

namespace juego;
public delegate int weight(Records records);
public interface IFinalized
{
    public bool EndGame(GameInformation gm, Referee referee,weight weight,ref int max);
}
public interface IWinner
{
    public Player Win(Referee referee, weight weight);
}
public interface IValidator
{
    public bool ValidPlay(jugada jugada, GameInformation gi);
}
public interface IContainer
{
    public bool HavesARecord(GameInformation gm, Player player);
}
public interface IShuffler
{
    public void Shuffle(Player player, GameInformation gi, ref int index, Referee rf, [Optional] int cant);

}