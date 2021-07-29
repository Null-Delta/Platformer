using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveFloor : Walker
{
    public override string objectName => "LiveFloor";

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)+3;

        
        isCollisiable = false;
    }
    public override bool readyCheck()
    {
        return true;
    }
    public override void onWalkStart()
    {
        
    }
    public override void onWalkAnimation(float time)
    {

    }
    public override void onWalkFinish()
    {
        
    }
    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        
    }

    public LiveFloor(float x, float y):base(x,y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
        linearMove = new Vector2();

        linearMove.x = 0;
        linearMove.y = 0;
        move_delay = Random.Range(0.1f,0.3f);
        animation_time = Random.Range(0.1f,0.3f);
    }
}
