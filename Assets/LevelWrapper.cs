using System;
using System.Collections.Generic;

[Serializable]
public class LevelWrapper
{
    public List<ObstacleDataWrapper> level;

    public LevelWrapper(List<ObstacleDataWrapper> level)
    {
        this.level = level;
    }
}
