namespace juego;
//***funcion*** mantenerse alerta del estado del tablero
//me dice las opciones que se pueden jugar
//registra cuales fichas han sido jugadas

//lista de fichas jugadas
public class GameInformation
{
    //fichas jugadas en el tablero
    //ver como poner esta propiedad pq no se quiere que sea modificable por cualq clase 
    public List<Records> RecordsInGame { get; set; }
    //actualizar las opciones de juego en cada jugada,paea saber que numeros debe contener la ficha  jugar.
   // public Dictionary<Records,int>Options{get;set;}
    public List<(Records records,int option)> OptionsToPlay { get; set; }
    //lista de fichas barajeadas.
    public List<Records> RecordsInOrder { get; set; }
    public int numberOfOptions;
    //delegado que se encargara de calcular peso de las fichas
    public weight weight;
    //almacenador de los pases de cada jugador, importante garantizar que sea un clonado del original para que el 
    //jugador no pueda copiarlo
    public Dictionary<Player,List<int>> turnPass;
    //almacenador de las jugadas de cada jugador.
    public Dictionary<Player,List<jugada>> turnPlayed;
    //constructor...
    public GameInformation(int numberOfOptions, weight weight)
    {
        this.weight=weight;
        RecordsInGame = new List<Records>();
        OptionsToPlay = new List<(Records records,int optionFree)>();
        RecordsInOrder = MakingRecords(numberOfOptions);
        this.numberOfOptions = numberOfOptions;
        Dictionary<Player,List<jugada>> turnPlayed=new Dictionary<Player, List<jugada>>();
        Dictionary<Player,List<int>> turnPass=new Dictionary<Player, List<int>>();
        //this.weight=weight();
    }
    //aqui e ponen las fichas en orden random,como barajear ,asi al tomar las fichas de esta lista la puedo tomar por ejemplo las
    //primeras nueve al primer jugador y asi.
    public List<Records> MakingRecords(int cant)
    {
        List<Records> records = new List<Records>();
        for (int i = 0; i < cant; i++)
        {
            for (int j = i; j < cant; j++)
            {
                records.Add(new Records(i, j));
            }
        }
        Random random = new Random();
        int n = records.Count;
        while (n > 1)
        {
            n--;
            int i = random.Next(n + 1);
            Records temp = records[i];
            records[i] = records[n];
            records[n] = temp;
        }
        return records;
    }

    //esta funcion se encarga de llevar el total de puntos que hay entre las fichas de los jugadores
    public int  shuffledPoints( Referee referee)
    {
        int aux=0;
        foreach (var item in referee.AsignedRecords.Keys)
        {
            foreach (var item2 in referee.AsignedRecords[item])
            {
                aux+=weight(item2);
            }
        }
        return aux;
    }
    public int PointsInGame()
    {
        int result=0;
        foreach (var item in RecordsInGame)
        {
            result+=weight(item);
        }
        return result;
    }

}