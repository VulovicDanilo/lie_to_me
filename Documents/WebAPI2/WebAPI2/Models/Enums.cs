﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
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
        Town,
        Mafia,
        Neutral
    }
    public enum GameMode
    {
        Classic,
        Any
    }

}