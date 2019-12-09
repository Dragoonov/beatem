using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TournamentDataWrapper
{
    public Competitor creator;
    public Competitor guest;
    public LevelWrapper levelWrapper;

    public TournamentDataWrapper(Competitor creator, Competitor guest, LevelWrapper level)
    {
        this.creator = creator;
        this.guest = guest;
        this.levelWrapper = level;
    }

    public TournamentDataWrapper()
    {

    }
}
