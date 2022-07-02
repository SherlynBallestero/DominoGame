namespace juego;
public class Player
{
    public  string id;
    public Player(string id)
    {
       this.id=id;
    }
    public jugada GiveMeRecords(InformationForPlayer ifp)
    {
        int cont = 0;
        System.Console.WriteLine(" jugador  "+ this.id);
        Console.ForegroundColor=ConsoleColor.Cyan;
        //foreach (var item in gm.OptionsToPlay)
        foreach (var item in ifp.OptionsToPlay)
        {
            System.Console.WriteLine(item.option + " option# "+ cont++);
        }
        Console.ForegroundColor=ConsoleColor.Gray;

        cont=0;
        System.Console.WriteLine("select a record to play");
        //foreach (var item in rf.AsignedRecords[this])
        foreach (var item in ifp.records)
        {
            System.Console.WriteLine(cont + " " + item.rcd.element1 + "-" + item.rcd.element2);
            cont++;
        }

        int answer = int.Parse(Console.ReadLine());
        System.Console.WriteLine( "where do you want to play?"  );
        int selectedOption = int.Parse(Console.ReadLine());
        cont=0;
        //foreach (var item in rf.AsignedRecords[this])
        foreach (var item in ifp.records)
        {
           if(cont==answer){
            return new jugada( selectedOption, item.rcd);}
            cont++;    

        }
        //devo;ver por default ya q no debe llegar aqui, ver como arreglar esto
        
        return new jugada (0, ifp.records[0].rcd);

    }
  
    //tiene una cant de fichas asigandas
    //a partir de las opciones de juego decide a donde y con que ficha jugar y la devuelve o decide pasar
    //aclarar q solo se puede pasar turno si no lleva ninguna ficha conectable con las opciones
}