
namespace juego;
public interface Iweighable
{
    public double weight();
}
public interface IFinalized
{
     public bool EndGame(GameInformation gm, Referee referee );
}
public interface IWinner
{
     public Player Win(Referee referee);
}