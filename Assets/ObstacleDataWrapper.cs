using System;
[Serializable]
public class ObstacleDataWrapper
{
    public int angle;
    public int obj;

    public ObstacleDataWrapper()
    {

    }

    public ObstacleDataWrapper(int angle, int obj)
    {
        this.angle = angle;
        this.obj = obj;
    }
}
