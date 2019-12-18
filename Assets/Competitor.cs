using System;

[Serializable]
public class Competitor
{
    public string name;
    public bool ready;
    public int obstaclesTravelled;

    public Competitor()
    {

    }

    public Competitor(string name, bool ready, int travelledObjects)
    {
        this.name = name;
        this.ready = ready;
        this.obstaclesTravelled = travelledObjects;
    }
}
