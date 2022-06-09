
namespace juego;
//public  delegate int Factorial(int x);
public class Referee
{
    public int numberOfOptions;
    //constructor....
    public Referee(int numberOfOptions)
    {
        this.numberOfOptions=numberOfOptions;
        this.AsignedRecords=new Dictionary<Player, List<Records>>();
       // this.cantCaras=cantCaras;
    }
    //formula de combinacion con repeticion para saber las cantidad de fichas total que se pueden jugar
    // public int TotalRecords{get{return Factorial(numberOfOPtions+cantCaras-1)/Factorial(cantCaras)*Factorial(numberOfOPtions-1); }}
     
    //almacenar las fichas repartidas a cada jugador por torneo 
    public Dictionary<Player,List<Records>> AsignedRecords{get;set;}
    
     //public int  Factorial(int a){int b=0;for(int i=1;i<=a;i++) {b =b*i;}return b;}
    //trabajar este metodo como delegado o lambda o lo que sea
    

    ///<summary>
    /// Esta funcion reparte fichas a cierto jugador,luego registra al jugador si no ha sido registrado 
    ///y registra las fichas agregadas a su mano   
    ///</summary>
     public void DistributeRecord(Player player, int cant)
     {
         //cont sirve para asegurar crear el numero de fichas indicado.
        int cont=0;
        List<Records> aux=new List<Records>();

        //creando ficha... 
        while(cont<cant+1) 
        {
            List<int> ElementsAux=new List<int>();
            //haciendo random los numeros para cada cara de la ficha...
            Random random=new Random();
            bool correct=true;
            while(correct)
            {
                ElementsAux.Add(random.Next(0,numberOfOptions));
                ElementsAux.Add(random.Next(0,numberOfOptions));
                if(!aux.Contains(new Records(ElementsAux)))correct=false;
            }
            //ent cuenta si se vuelve a crear ficha
            //si no se ha asignado a ningun jugador o se encuentra en las asignadas a este antes 
            Records auxRecords=new Records(ElementsAux);
            
            int count=0;
           if(AsignedRecords.Count!=0)
           {
                foreach (var item in AsignedRecords.Keys)
                {
                 if(auxRecords.Equals(AsignedRecords[item]))continue;
                 else{count++;}
                }
             //si count es igual al numero de elementos en este dictionary significa q la ficha no se ha asignado
                 if(count!=AsignedRecords.Count)
                {
                continue;
                }
           }
            //si la ficha no ha sido dada a algun jugador ent asignala
            aux.Add(auxRecords);
            cont++;       
        }
        AsignedRecords.Add(player,aux);
        System.Console.WriteLine(AsignedRecords.Count); 
     }
    ///<summary>
    ///Dado el jugador actual me dice a quien le toca
    ///y asociar el orden en que jugara cada uno. 
    ///</summary>
    public Player NextTurn(Player actualPlayer)
    {
        bool aux=false;
        foreach (var item in AsignedRecords.Keys)
        {
            if(aux)return item;
            if(item==actualPlayer)
            {
                aux=true;
            }
        }
       
        Player auxplayer=actualPlayer;
            foreach (var item in AsignedRecords.Keys)
            {
                auxplayer= item; break;
            }
        return auxplayer;
        
    }
       
    ///<summary>
    ///Esta funcion retorna si una jugada es valida de acuedo con el estado del tablero y la jugada, la jugada
    /// viene dada por una ficha y una de las opciones de juego, es decir, esta en el caso en que solo se puede
    ///jugar fichas que contengan 3 o 4, q son las opciones actualmente viables en el partido, si la opcion 
    ///seleccionada por el jugador, digase por ej 3 no se puede conectar con la ficha que juega ent la jugada 
    ///no es valida. 
    ///</summary>
    public bool validPlay(Records records)
    {

        throw new NotImplementedException();
    }
    ///<summary>
    ///Nos dice true o false en dependencia de si el juego llego a su final, un juego llega a su final 
    ///si alguno de los jugadores se quedan sin ficha o si ya no se puede jugar mas, o sea todos los jugadores
    ///han pasado turno dado que no tienen fichas que encaje con las opciones disponibles en el juego.
    ///</summary>
    public bool EndGame(GameInformation gm)
    {
        throw new NotImplementedException();
        //se vera si todos los jugadores se pasaron(o sea de entre las opciones de juego ninguna ficha de ningun jugador 
        //satisface estas opciones) o si hay algn jugador que no tenga fichas.

        foreach (var item in AsignedRecords.Keys)
        {
            if(AsignedRecords[item].Count==0)return true;
            //asigned record en item me da las lista con fichas que tiene cada jugador
            foreach (var item2 in AsignedRecords[item])
            {
                //paseando por la lista de fichas que tiene asignado el jugador correspondiente con item
                //viendo si alguna ficha de esta lista tiene no conectada alguna opcion que machee con optiontoplay
                if(gm.OptionsToPlay.Contains(item2.totalElements[0]))return true;
                if(gm.OptionsToPlay.Contains(item2.totalElements[1]))return true;
           
            }
        }
        return false;

    }
    ///<summary>
   /// Se dira cual jugador es el ganador,de acuerdo con el criterio de quien menso pesa tenga.
   ///</summary>
    public Player Win()
    {
        double min=(double)int.MaxValue;
        double aux=0;
        Player playerW=new Player();
        foreach (var item in AsignedRecords.Keys)
        {
            if(AsignedRecords[item].Count!=0)
            {
                foreach (var item2 in AsignedRecords[item])
                {
                    aux+=item2.weight();
                }
                if(aux<min){min=aux;
                playerW=item;}
            }else
            {
                return item;
            }
        }
        return playerW;
    }


}
