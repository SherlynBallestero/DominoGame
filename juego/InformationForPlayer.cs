namespace juego
{
    public class InformationForPlayer
    {
        //almacenador de los pases de cada jugador, importante garantizar que sea un clonado del original para que el 
        //jugador no pueda copiarlo
        public Dictionary<Player, List<int>> turnPass;
        //almacenador de las jugadas de cada jugador.
        public Dictionary<Player, List<jugada>> turnPlayed;
        //listas de fichas que tiene el jugador con sus valores correspondientes
        public List<(Records rcd, int weight)> records;
        //indices de indices de las fichas que matchean con las opciones para jugar dada la lista anterior
        public List<(Records rcd,int weight)> matchedRec;
        //lista de opciones en las que jugar
        public List<(Records,int)> Options;

        public match match;
        public InformationForPlayer(Dictionary<Player, List<int>> turnPass, Dictionary<Player, List<jugada>> turnPlayed, List<(Records rcd, int weight)> records, List<(Records rcd,int weight)> matchedRec,List<(Records,int)> Options, match match)
        {
            this.turnPass = turnPass;
            this.turnPlayed = turnPlayed;
            this.records = records;
            this.matchedRec = matchedRec;
            this.Options=Options;
            this.match = match;
        } 
        

    }


}