using juego;
namespace Rules;
public interface Iweighable
{
    public double weight();
}
public interface Idistribute
{
     public void DistributeRecord(Player player, int cant);
}