
namespace juego;
public interface Iweighable
{
    public double weight();
}
public interface ITerminationConditioner
{
     public bool EndGame(GameInformation gm, Referee referee );
}