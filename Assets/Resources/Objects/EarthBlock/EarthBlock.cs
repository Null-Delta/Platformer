using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBlock : WalkAndLive
{
    public override string objectName => "EarthBlock";
    float timeToDestroy;

    public override void startObject()
    {
        base.startObject();
        order = ObjectOrder.wall;
        mapLocation = new Vector2Int((int)position.x, (int)position.y);
        
        hp = 20;
        immortalTimeForHit = 0.0f;
        canFall = false;
    }
    public override void updateObject()
    {
        base.updateObject();
        if (timeToDestroy != -1f)
        {
            timeToDestroy -=Time.deltaTime;
            if (timeToDestroy <= 0 )
            {
                map.destroyObject(this);
            }
        }
    }

    //override public void onDeath()
    //{
    //}

    public EarthBlock(int x, int y, float _timeToDestroy = -1f): base(x,y) {
        timeToDestroy = _timeToDestroy;
    }
    public EarthBlock(): base(0,0) {
        timeToDestroy = -1f;
    }
}
