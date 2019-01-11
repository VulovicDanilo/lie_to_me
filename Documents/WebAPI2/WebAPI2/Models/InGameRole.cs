namespace WebAPI2.Models
{
    public  class InGameRole
    {
        public int InGameRoleID { get; set; }
        public Offence Offence { get; set; }
        public Defence Defence { get; set; }
        public TownRole TownRole { get; set; }

    }
}