
namespace juego;
public delegate int weight(Records records);
public interface IFinalized
{
     public bool EndGame(GameInformation gm, Referee referee );
}
public interface IWinner
{
     public Player Win(Referee referee, weight weight);
}