namespace juego;
    //***funcion*** mantenerse alerta del estado del tablero
    //me dice las opciones que se pueden jugar
    //registra cuales fichas han sido jugadas
    
    //lista de fichas jugadas
public class GameInformation
{
    //fichas jugadas en el tablero
    //ver como poner esta propiedad pq no se quiere que sea modificable por cualq clase 
    public List<Records> totalRecords{get;set;}
    //actualizar las opciones de juego en cada jugada,paea saber que numeros debe contener la ficha  jugar.
    public  List<int> OptionsToPlay{get;set;}
    //constructor...
    public  GameInformation()
    {
        totalRecords=new List<Records>();
        OptionsToPlay= new List<int>();

    }
    

    
}