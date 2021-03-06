﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Defence,
        Judgement,
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
        Neutral,
        Draw
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

    public enum MessageQueueChannel
    {
        ChatMessageDead = 0,
        ChatMessageAlive = 1,
        ContextBroadcast = 99,
        LobbyInfo = 100,
        PrivateChannelOffset = 1000
    }

    public enum JudgementVote
    {
        Guilty,
        Innocent,
        Abstained
    }


}