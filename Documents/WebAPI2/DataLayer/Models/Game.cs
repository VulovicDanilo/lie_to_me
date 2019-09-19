using DataLayer.DTOs;
using DataLayer.Extensions;
using DataLayer.Models.Roles;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public readonly int MAX_PLAYERS = 10;
        [NotMapped]
        public GameState GameState { get; set; }
        [NotMapped]
        public List<Player> Winners { get; set; } = new List<Player>();
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
        public int Day { get; set; } = 1;
        [NotMapped]
        public Player Accused { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Owner = null;
            Day = 0;
            Durations.Add(GameState.NameSelection, 3);
            Durations.Add(GameState.RoleDistribution, 5);
            Durations.Add(GameState.Discussion, 3);
            Durations.Add(GameState.Voting, 30);
            Durations.Add(GameState.Defence, 20);
            Durations.Add(GameState.Judgement, 15);
            Durations.Add(GameState.LastWord, 10);
            Durations.Add(GameState.Night, 30);

            //Durations.Add(GameState.NameSelection, 3);
            //Durations.Add(GameState.RoleDistribution, 3);
            //Durations.Add(GameState.Discussion, 3);
            //Durations.Add(GameState.Voting, 20);
            //Durations.Add(GameState.Defence, 3);
            //Durations.Add(GameState.Judgement, 3);
            //Durations.Add(GameState.LastWord, 3);
            //Durations.Add(GameState.Night, 3);

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

        public static RoleName[] GetRandomRoles(RoleName notThis)
        {
            Random rnd = new Random();
            var role1 = (RoleName)rnd.Next();
            while (role1 != notThis)
            {
                role1 = (RoleName)rnd.Next();
            }
            var role2 = (RoleName)rnd.Next();
            while (role2 != notThis && role2 != role1)
            {
                role2 = (RoleName)rnd.Next();
            }
            return new RoleName[] { role1, role2 };
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
        public void ResetTimerEvents()
        {
            foreach (ElapsedEventHandler ev in events)
            {
                removeElapsedEvent(ev);
            }
            events.RemoveAll(x => true);
        }


        public void SetNextState()
        {
            if (GameState == GameState.Discussion)
            {
                ResolveDiscussion();
            }
            else if (GameState == GameState.Voting)
            {
                ResolveVoting();
            }
            else if (GameState == GameState.Defence)
            {
                ResolveDefence();
            }
            else if (GameState == GameState.Judgement)
            {
                ResolveJudgement();
            }
            else if (GameState == GameState.LastWord)
            {
                ResolveLastWord();
            }
            else if (GameState == GameState.Night)
            {
                ResolveNight();
            }
        }

        public void ResolveDiscussion()
        {
            GameState = GameState.Voting;
        }
        private Dictionary<int,int> votes = new Dictionary<int, int>();
        public void ResolveVoting()
        {
            if (votes.Count == 0)
            {
                GameState = GameState.Night;
            }
            else
            {
                var most = votes.GroupBy(i => i.Value)
                    .OrderByDescending(grp => grp.Count())
                    .Select(grp => grp.Key).First();
                var mostCount = votes.Count(x => x.Value == most);
                var aliveCount = Players.Count(x => x.Alive);
                if (mostCount >= aliveCount / 2)
                {
                    Accused = Players.Find(x => x.Number == most);
                    GameState = GameState.Defence;
                }
                else
                {
                    GameState = GameState.Night;
                }
            }
            votes.Clear();
        }
        public void ResolveDefence()
        {
            GameState = GameState.Judgement;
        }

        private Dictionary<int, JudgementVote> judgementVotes = new Dictionary<int, JudgementVote>();
        public void ResolveJudgement()
        {
            int judgeFor = 0;
            int judgeAgainst = 0;
            foreach (var vote in judgementVotes)
            {
                if (vote.Value == JudgementVote.Guilty)
                {
                    judgeFor++;
                }
                else if (vote.Value == JudgementVote.Innocent)
                {
                    judgeAgainst++;
                }
            }
            if (judgeFor > judgeAgainst)
            {
                Accused.Alive = false;
                GameState = GameState.LastWord;
            }
            else
            {
                GameState = GameState.Night;
            }
            Accused = null;
            judgementVotes.Clear();
        }
        public void ResolveLastWord()
        {
            GameState = GameState.Night;
        }

        private Dictionary<int, ExecuteActionModel> actions = new Dictionary<int, ExecuteActionModel>();
        public List<int> attacked = new List<int>();
        public void ResolveNight()
        {
            GameState = GameState.Discussion;
            // clear visitors
            foreach (var player in Players)
            {
                player.Reset();
            }
            var executioners = new List<Player>();
            foreach (var action in actions)
            {
                var who = Players.Find(x => x.Number == action.Value.Who);
                var to = Players.Find(x => x.Number == action.Value.To);
                executioners.Add(who);
                if (who != to)
                {
                    to.AddVisitor(who);
                }
            }

            executioners.Sort((x, y) => x.Role.Priority.CompareTo(y.Role.Priority));

            foreach (var executioner in executioners)
            {
                var action = actions[executioner.Number];
                if (!executioner.Done)
                {
                    executioner.Role.ExecuteAction(this, action);
                }
            }
            Alignment won = ResolveWinner();
            if(won != Alignment.NotDecided)
            {
                GameState = GameState.GameEnd;
            } else
            {
                GameState = GameState.Discussion;
            }
            Day++;
        }

        private Alignment ResolveWinner()
        {
            var town = Players.Where(x => x.Role.Alignment == Alignment.Town && x.Alive).Count();
            var mafia = Players.Where(x => x.Role.Alignment == Alignment.Mafia && x.Alive).Count();
            var serialKillers = Players.Where(x => x.Role.RoleName == RoleName.SerialKiller && x.Alive);


            if (town == 0 && mafia == 0)
            {
                foreach (var serial in serialKillers)
                {
                    Winners.Add(serial);
                    serial.Winner = true;
                }
                return Alignment.Draw;
            }
            else if (town == 0)
            {
                Winners.AddRange(Players.Where(x => x.Role.Alignment == Alignment.Mafia).ToList());
                return Alignment.Mafia;
            }
            else if (mafia == 0)
            {
                Winners.AddRange(Players.Where(x => x.Role.Alignment == Alignment.Town).ToList());
                return Alignment.Town;
            }

            return Alignment.NotDecided;
        }

        private void ResolveGameEnd()
        {
            Winners.ForEach(x => x.Winner = true);
            GameState = GameState.GameEnd;
        }

        public void AddVote(int voter,int voted)
        {
            if (GameState == GameState.Voting)
            {
                if (votes.ContainsKey(voter))
                {
                    votes[voter] = voted;
                }
                else
                {
                    votes.Add(voter, voted);    
                }
            }
        }
        public void AddJudgementVote(int voterNumber, JudgementVote vote)
        {
            if (GameState == GameState.Judgement)
            {
                if (judgementVotes.ContainsKey(voterNumber))
                {
                    judgementVotes[voterNumber] = vote;
                }
                else
                {
                    judgementVotes.Add(voterNumber, vote);
                }
            }
        }
        public void AddAction(ExecuteActionModel action)
        {
            if (GameState == GameState.Night)
            {
                actions.Add(action.Who, action);
            }
        }
        public void RemoveAction(int number)
        {
            if (GameState == GameState.Night)
            {
                actions.Remove(number);
            }
        }
    }
}