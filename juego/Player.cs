namespace juego;
public class Player
{
    public  string id;
    public Player(string id)
    {
       this.id=id;
    }
    public Records GiveMeRecords(GameInformation gm, Referee rf)
    {
        int cont = 0;
        System.Console.WriteLine("do you have ?"+" "+ this.id);
        foreach (var item in gm.OptionsToPlay)
        {
            System.Console.WriteLine(item + " ");
        }
        System.Console.WriteLine("select a record to play");
        foreach (var item in rf.AsignedRecords[this])
        {
            System.Console.WriteLine(cont + " " + item.totalElements[0] + "-" + item.totalElements[1]);
            cont++;
        }

        int answer = int.Parse(Console.ReadLine());
        cont=0;
        foreach (var item in rf.AsignedRecords[this])
        {
           if(cont==answer)return item;
           cont++;    
        }
        return new Records(new List<int>{1,2});

    }
  
    //tiene una cant de fichas asigandas
    //a partir de las opciones de juego decide a donde y con que ficha jugar y la devuelve o decide pasar
    //aclarar q solo se puede pasar turno si no lleva ninguna ficha conectable con las opciones
}