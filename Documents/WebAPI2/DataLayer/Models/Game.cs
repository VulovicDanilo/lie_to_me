using DataLayer.Extensions;
using DataLayer.Models.Roles;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;

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
        public readonly int MAX_PLAYERS = 3;
        [NotMapped]
        public GameState GameState { get; set; }
        [NotMapped]
        public List<Player> Winners { get; set; }
        [NotMapped]
        public IModel Channel { get; set; }
        [NotMapped]
        public Timer Timer { get; set; }
        [NotMapped]
        public Dictionary<GameState, int> Durations { get; set; } = new Dictionary<GameState, int>();
        [NotMapped]
        public int Duration
        {
            get
            {
                if (Durations.ContainsKey(this.GameState))
                {
                    return Durations[this.GameState];
                }
                else
                {
                    return -1;
                }
            }
        }
        [NotMapped]
        public int Day { get; set; }
        [NotMapped]
        public Player Accused { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Owner = null;
            Day = 0;
            //Durations.Add(GameState.NameSelection, 15);
            //Durations.Add(GameState.RoleDistribution, 5);
            //Durations.Add(GameState.Discussion, 60);
            //Durations.Add(GameState.Voting, 30);
            //Durations.Add(GameState.Defence, 20);
            //Durations.Add(GameState.Judgement, 15);
            //Durations.Add(GameState.LastWord, 10);
            //Durations.Add(GameState.Night, 30);

            Durations.Add(GameState.NameSelection, 3);
            Durations.Add(GameState.RoleDistribution, 3);
            Durations.Add(GameState.Discussion, 3);
            Durations.Add(GameState.Voting, 3);
            Durations.Add(GameState.Defence, 3);
            Durations.Add(GameState.Judgement, 3);
            Durations.Add(GameState.LastWord, 3);
            Durations.Add(GameState.Night, 3);

            Timer = new Timer();
        }
        public Game(Player owner)
            : base()
        {
            Owner = owner;
        }


        public static RoleStrategy GetTownRoleFromNumber(int idx)
        {
            switch (idx)
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
                default:
                    throw new Exception("Index out of range");
            }
        }

        public static RoleStrategy GetNeutralRoleFromNumber(int idx)
        {
            switch (idx)
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
            for (int i = 0; i < poolSize; i++)
            {
                int idx = rnd.Next(0, poolSize);
                pool.Add(Game.GetTownRoleFromNumber(idx));
            }
            return pool;
        }

        private bool FakeNameExists(string fakeName)
        {
            return Players.Exists(x => x.FakeName == fakeName);
        }

        public void AssignNamesAndRoles()
        {
            var names = GetNamePool();
            Queue<string> queue = new Queue<string>(names);

            Queue<RoleStrategy> strategies = new Queue<RoleStrategy>(GetStrategyPool(this.MAX_PLAYERS));

            int i = 0;
            foreach (var player in Players)
            {
                while (player.FakeName == "" || player.FakeName is null)
                {
                    var name = queue.Dequeue();
                    if (!FakeNameExists(name))
                    {
                        player.FakeName = name;
                    }
                }
                player.SetRole(strategies.Dequeue());
                player.Number = i++;
            }
        }

        public void StartNameSelection()
        {

        }

        public static IList<RoleStrategy> GetMafiaPool(int poolSize)
        {
            List<RoleStrategy> pool = new List<RoleStrategy>(poolSize)
            {
                new GodfatherStrategy()
            };

            var rnd = new Random();
            for (int i = 0; i < poolSize - 1; i++)
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
            pool.Shuffle();
            return pool;
        }

        private static readonly string[] first_names = { "John", "Johnny", "Mr.", "Louis", "Sergeant", "Dr.", "Person", "Jane", "Anne", "Maria" };
        private static readonly string[] last_names = { "Doe", "Jimbo", "Mustafa", "Harambe", "Chance", "Chase", "Why", "Here", "Six", "Ok", "Bravo" };
        public static IEnumerable<string> GetNamePool()
        {
            List<string> first = new List<string>();
            List<string> last = new List<string>();
            first.AddRange(first_names);
            last.AddRange(last_names);
            first.Shuffle();
            last.Shuffle();
            List<string> results = new List<string>();
            for (int i = 0; i < first.Count; i++)
            {
                var result = first[i] + " " + last[i];
                results.Add(result);
            }
            return results;
        }


        private List<ElapsedEventHandler> events = new List<ElapsedEventHandler>();
        public void addElapsedEvent(ElapsedEventHandler ev)
        {
            this.events.Add(ev);
            this.Timer.Elapsed += ev;
        }

        public void removeElapsedEvent(ElapsedEventHandler ev)
        {
            this.Timer.Elapsed -= ev;
        }
        public void resetTimerEvents()
        {
            foreach(ElapsedEventHandler ev in events)
            {
                removeElapsedEvent(ev);
            }
            events.RemoveAll(x => true);
        }
    }
}