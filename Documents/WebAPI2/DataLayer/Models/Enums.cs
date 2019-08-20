using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public enum Offence
    {
        None,
        Basic,
        Powerful,
        Unstoppable
    }
    public enum Defence
    {
        None,
        Basic,
        Powerful
    }

    public enum GameState
    {
        Lobby,
        NameSelection,
        RoleDistribution,
        GameStart,
        Discussion,
        Voting,
        Judgement,
        Defence,
        LastWord,
        Trial,
        Night,
        GameEnd
    }
    public enum Alignment
    {
        NotDecided = 0,
        Town,
        Mafia,
        Neutral
    }
    public enum GameMode
    {
        Classic,
        Any
    }

    public enum RoleName
    {
        //Town roles
        BodyGuard=0,
        Doctor,
        Investigator,
        Lookout,
        Mayor,
        Medium,
        Sheriff,
        Veteran,
        Vigilante,
        //Mafia roles
        Godfather,
        Mafioso,
        Framer,
        //Neutral roles
        SerialKiller,
        Jester

    }


}