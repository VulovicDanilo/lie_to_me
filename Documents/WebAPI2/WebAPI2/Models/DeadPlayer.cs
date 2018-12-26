namespace WebAPI2.Models
{
    public class DeadPlayer
    {
        public int DeadPlayerID { get; set; }
        public Player Player { get; set; }
        public string DeathNote { get; set; }
        public bool Available { get; set; }
    }
}