namespace juego;
    //***funcion*** mantenerse alerta del estado del tablero
    //me dice las opciones que se pueden jugar
    //registra cuales fichas han sido jugadas
    
    //lista de fichas jugadas
public class GameInformation
{
    //fichas jugadas en el tablero
    //ver como poner esta propiedad pq no se quiere que sea modificable por cualq clase 
    public List<Records> RecordsInGame{get;set;}
    //actualizar las opciones de juego en cada jugada,paea saber que numeros debe contener la ficha  jugar.
    public  List<int> OptionsToPlay{get;set;}
    public List<Records> RecordsInOrder{get;set;}
    public int numberOfOptions;
    //constructor...
    public  GameInformation(int numberOfOptions)
    {
        RecordsInGame=new List<Records>();
        OptionsToPlay= new List<int>();
        RecordsInOrder=MakingRecords(numberOfOptions);
        this.numberOfOptions = numberOfOptions;
    }
    //aqui e ponen las fichas en orden random,como barajear ,asi al tomar las fichas de esta lista la puedo tomar por ejemplo las
    //primeras nueve al primer jugador y asi.
    public List<Records> MakingRecords(int cant)
    {
        List<Records> records=new List<Records>();
        for(int i=0;i<cant;i++)
        {
            for (int j = i; j < cant; j++)
            {
                //if(i<=j)
                //{}
                    List<int> aux=new List<int>();
                    aux.Add(j);
                    aux.Add(i);

                    records.Add(new Records(aux));
                
            }
        }
        Random random=new Random();
        int n=records.Count;
        while(n>1)
        {
            n--;
            int i=random.Next(n+1);
            Records temp=records[i];
            records[i]=records[n];
            records[n]=temp;
        }
        return records;
    }
    

    
}