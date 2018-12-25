namespace WebAPI2.Models
{
    public  class InGameRole:IEntity
    {
        public int InGameRoleID { get; set; }

        public int ID
        {
            get
            {
                return InGameRoleID;
            }
        }
    }
}