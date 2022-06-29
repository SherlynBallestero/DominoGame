﻿namespace juego;
public class Program
{
    public static void Main(string[] argd)
    {
        WaysToPLay1();
    }
    public static void WaysToPLay1()
    {
        //haciendo instancias de las clases a emplear
        Player A = new Player("01A");
        Player B = new Player("02B");
        Player C = new Player("03C");
        Player D = new Player("04D");
        int numberOfOptions = 3;
        GameInformation gi = new GameInformation(numberOfOptions,new weight(delegate (Records records) { return records.element1 + records.element2; }));
        Referee referee = new Referee(numberOfOptions, new clasicEnd(), new clasicWinner(), new validator(), new shuffler());
        int index = 0;
        referee.shuffler.Shuffle(A, gi, ref index, referee);
        referee.shuffler.Shuffle(B, gi, ref index, referee);
        int max=0;
        while (!referee.finalized.EndGame(gi, referee,ref max))
        {
            //caminando por cada jugador siempre que el juego no haya acabado
            foreach (Player item in referee.AsignedRecords.Keys)
            {
                if (referee.finalized.EndGame(gi, referee,ref max)) break;
                //verificar que el jugador no se pase
                if (referee.HavesARecord(gi, item))
                {

                    jugada jugadaAux = item.GiveMeRecords(gi, referee);
                    while (!referee.validator.ValidPlay(jugadaAux, gi, new match(delegate(int option,int record){return option==record;})))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("wrong!!!!!");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        jugadaAux = item.GiveMeRecords(gi, referee);
                    }
                    referee.Play(jugadaAux, gi);
                    referee.AsignedRecords[item].Remove(jugadaAux.record);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine("paseeee para " + item.id);
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
            }
        }
        referee.Winner.Win(referee,gi);
    }
}

