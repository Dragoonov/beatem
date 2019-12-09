using System;

[Serializable]
public class Competitor
{
    public string name;
    public bool ready;

    public Competitor()
    {

    }

    public Competitor(string name, bool ready)
    {
        this.name = name;
        this.ready = ready;
    }
}
