namespace juego;
public abstract class Player
{
    public string id;
    public Player(string id)
    {
        this.id = id;
    }
    public abstract jugada GiveMeRecords(InformationForPlayer info, Referee rf);
}
public class ManualPlayer : Player
{
    public ManualPlayer(string id) : base(id) { }
    public override jugada GiveMeRecords(InformationForPlayer info, Referee rf)
    {
        int cont = 0;
        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var item in info.Options)
        {
            System.Console.WriteLine(item.Item2 + " option# " + cont++);
        }
        Console.ForegroundColor = ConsoleColor.Gray;

        cont = 0;
        Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("selecciona una ficha para jugar");
        Console.ForegroundColor = ConsoleColor.Gray;
        foreach (var item in info.matchedRec)
        {
            System.Console.WriteLine(cont + " " + item.rcd.element1 + "-" + item.rcd.element2);
            cont++;
        }
        string aux = Console.ReadLine();
        int answer = 0;
        while (!int.TryParse(aux, out answer))
        {
            System.Console.WriteLine("Debe elegir un número");
            aux = System.Console.ReadLine();
        }
        //que sea automatica la jugada cuando no hay que decidir
        if (info.Options.Count>=1 && info.Options != null)
        {

            Records auxRecord = new Records(new List<int> { info.Options[0].Item2, info.Options[1].Item2 });
            Records move = info.matchedRec[answer].rcd;
            int selectedOption = 0;
            if ((auxRecord.element1 == move.element1 && auxRecord.element2 == move.element2) || auxRecord.element1 == move.element2 && auxRecord.element2 == move.element1)
            {

                System.Console.WriteLine("Por donde quieres jugar?");
                aux = System.Console.ReadLine();
                while (!int.TryParse(aux, out selectedOption))
                {
                    System.Console.WriteLine("Debe elegir un número");
                    aux = System.Console.ReadLine();
                }
                return new jugada(selectedOption, info.matchedRec[answer].rcd);
            }
            else
            {

                Random r1 = new Random();
                selectedOption = r1.Next(0, 2);
                return new jugada(selectedOption, info.matchedRec[answer].rcd);
            }
        }
        else
        {
            int selectedOption = 0;
            Random r1 = new Random();
            selectedOption = r1.Next(0, 2);
            return new jugada(selectedOption, info.matchedRec[answer].rcd);
        }


    }
}
public class RandomPlayer : Player
{
    public RandomPlayer(string id) : base(id) { }
    public override jugada GiveMeRecords(InformationForPlayer info, Referee rf)
    {
        Random r1 = new Random();
        int answer = r1.Next(0, info.matchedRec.Count);
        int selectedOption = r1.Next(0, 2);
        return new jugada(selectedOption, info.matchedRec[answer].rcd);
    }
}
public class GreedyPlayer : Player
{
    public GreedyPlayer(string id) : base(id) { }
    public override jugada GiveMeRecords(InformationForPlayer info, Referee rf)
    {
        Rearrange(rf, info.matchedRec);
        Random r = new Random();
        int option = r.Next(0, 2);
        return new jugada(option, info.matchedRec[0].rcd);
    }
    private static void Rearrange(Referee referee, List<(Records rcd, int weight)> records)
    {
        for (int i = 0; i < records.Count - 1; i++)
            for (int j = i + 1; j < records.Count; j++)
            {
                if (RulesCheck(records[i].weight, records[j].weight, referee) > 0)
                {
                    Swap(records[i], records[j]);
                }
            }
    }

    private static void Swap((Records rcd, int wgt) left, (Records rcd, int wgt) right)
    {
        Records rd = new Records(new List<int>() { 0, 0 });
        int temp = 0;
        rd = left.rcd;
        temp = left.wgt;
        left.rcd = right.rcd;
        left.wgt = right.wgt;
        right.rcd = rd;
        right.wgt = temp;
    }
    private static int RulesCheck(int left, int right, Referee referee)
    {
        if (referee.Winner is clasicWinner) { return (left < right) ? 1 : 0; }
        else if (referee.Winner is Winner) { return (left > right) ? 1 : 0; }
        return 0;
    }
    /*protected static int FindOption(Records record,List<(Records,int)> Options, InformationForPlayer info)
    {
        for(int i=0;i<Options.Count;i++)
        {
            if(info.match(record, i, Options[0].Item1, Options[0].Item2)){return 0;}
            else if(info.match(record, i, Options[1].Item1, Options[1].Item2)){return 1;}
        }

        //no debe llegar aquí

        return 0;
    }*/

    protected static bool IsIn(Records rec, List<(Records rcd, int weight)> matchedRec)
    {
        for (int i = 0; i < matchedRec.Count; i++)
        {
            if (rec.element1 == matchedRec[i].rcd.element1 && rec.element2 == matchedRec[i].rcd.element2)
            {
                return true;
            }
        }
        return false;
    }
}
public class DataPlayer : GreedyPlayer
{
    public DataPlayer(string id) : base(id) { }
    public override jugada GiveMeRecords(InformationForPlayer info, Referee referee)
    {
        bool[] mark = MarkData(info.records);
        Random r = new Random();
        int option = 0;
        for (int i = 0; i < mark.Length; i++)
        {
            if (IsIn(info.records[i].rcd, info.matchedRec) && mark[i])
            {
                option = r.Next(0, 2);
                return new jugada(option, info.records[i].rcd);
            }
            //{return new jugada(FindOption(info.records[i].rcd,info.Options,info),info.records[i].rcd);}
        }
        for (int i = 0; i < mark.Length; i++)
        {
            if (IsIn(info.records[i].rcd, info.matchedRec) && !mark[i])
            {
                option = r.Next(0, 2);
                return new jugada(option, info.records[i].rcd);
            }

            //    {return new jugada(FindOption(info.records[i].rcd,info.Options,info),info.records[i].rcd);}
        }
        //Caso base, se implementa de tal manera que no llega aquí
        return new jugada(0, info.matchedRec[0].rcd);

    }
    private static bool[] MarkData(List<(Records rcd, int weight)> records)
    {
        int max = -1;
        bool[] sol = new bool[records.Count];
        for (int i = 0; i < records.Count; i++)
        {
            if (records[i].rcd.element1 > max) { max = records[i].rcd.element1; }
            else if (records[i].rcd.element2 > max) { max = records[i].rcd.element2; }
        }
        for (int i = 0; i < sol.Length; i++)
        {
            sol[i] = (max == records[i].rcd.element1 || max == records[i].rcd.element2);
        }
        return sol;
    }
}