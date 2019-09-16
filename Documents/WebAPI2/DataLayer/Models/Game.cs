using DataLayer.Models.Roles;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Column(name: "StartTime")]
        public DateTime? StartTime { get; set; }
        [Column(name: "EndTime")]
        public DateTime? EndTime { get; set; }
        public Alignment? Winner { get; set; }
        public virtual List<Player> Players { get; set; }
        [ForeignKey("Owner")]
        public virtual int? Owner_Id { get; set; }
        public Player Owner { get; set; }
        public GameMode GameMode { get; set; }


        [NotMapped]
        public readonly int MAX_PLAYERS = 10;
        [NotMapped]
        public GameState GameState { get; set; }
        [NotMapped]
        public List<Player> Winners { get; set; }
        [NotMapped]
        public IModel Channel { get; set; }


        public Game()
        {
            Players = new List<Player>();
            Owner = null;
        }
        public Game(Player owner)
            : base()
        {
            Owner = owner;
        }


        public static RoleStrategy GetTownRoleFromNumber(int idx)
        {
            switch(idx)
            {
                case 0:
                    return new BodyguardStrategy();
                case 1:
                    return new DoctorStrategy();
                case 2:
                    return new InvestigatorStrategy();
                case 3:
                    return new LookoutStrategy();
                case 4:
                    return new MayorStrategy();
                case 5:
                    return new MediumStrategy();
                case 6:
                    return new SheriffStrategy();
                case 7:
                    return new VeteranStrategy();
                case 8:
                    return new VigilanteStrategy();
                default:
                    throw new Exception("index out of range");
                
            }
        }

        public static RoleStrategy GetMafiaRoleFromNumber(int idx)
        {
            switch (idx)
            {
                case 0:
                    return new FramerStrategy();
                case 1:
                    return new MafiosoStrategy();
                case 2:
                    return new GodfatherStrategy();
                default:
                    throw new Exception("Index out of range");
            }
        }

        public static RoleStrategy GetNeutralRoleFromNumber(int idx)
        {
            switch(idx)
            {
                case 0:
                    return new JesterStrategy();
                case 1:
                    return new SerialKillerStrategy();
                default:
                    throw new Exception("Index out of range");
            }
        }

        public static IList<RoleStrategy> GetTownPool(int poolSize)
        {
            List<RoleStrategy> pool = new List<RoleStrategy>(poolSize);
            var rnd = new Random();
            for(int i = 0; i < poolSize; i++)
            {
                int idx = rnd.Next(0, poolSize);
                pool.Add(Game.GetTownRoleFromNumber(idx));
            }
            return pool;
        }
        public static IList<RoleStrategy> GetMafiaPool(int poolSize)
        {
            List<RoleStrategy> pool = new List<RoleStrategy>(poolSize);
            var rnd = new Random();
            for (int i = 0; i < poolSize; i++)
            {
                int idx = rnd.Next(0, poolSize);
                pool.Add(Game.GetMafiaRoleFromNumber(idx));
            }
            return pool;
        }
        public static IList<RoleStrategy> GetNeutralPool(int poolSize)
        {
            List<RoleStrategy> pool = new List<RoleStrategy>(poolSize);
            var rnd = new Random();
            for (int i = 0; i < poolSize; i++)
            {
                int idx = rnd.Next(0, poolSize);
                pool.Add(Game.GetNeutralRoleFromNumber(idx));
            }
            return pool;
        }
        public static IEnumerable<RoleStrategy> GetStrategyPool(int numberOfPlayers)
        {

            int townNumber = numberOfPlayers / 2;
            int mafiaNumber = numberOfPlayers / 3;
            int neutralNumber = numberOfPlayers - mafiaNumber - townNumber;

            List<RoleStrategy> pool = new List<RoleStrategy>();
            pool.AddRange(GetTownPool(townNumber));
            pool.AddRange(GetMafiaPool(mafiaNumber));
            pool.AddRange(GetNeutralPool(neutralNumber));
            
            return pool;
        }

    }
}