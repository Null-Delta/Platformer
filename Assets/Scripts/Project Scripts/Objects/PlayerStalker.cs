using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStalker : Walker
{
    public override string objectName => "PlayerStalker";
    Player target;

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)+3;

        linearMove.x = 0;
        linearMove.y = 0;

        move_delay = Random.Range(0.1f,0.3f);
        animation_time = Random.Range(0.1f,0.3f);
        isCollisiable = true;
    }
    public override bool readyCheck()
    {
        return true;
    }
    public override void onWalkStart()
    {
        var rangeTarget = target.position - this.position;
        if (Mathf.Abs(rangeTarget.x) + Mathf.Abs(rangeTarget.y) <= 2)
        {
            linearMove.x = 0;
            linearMove.y = 0;
            map.setupObject(new Bullet(position.x +1f*rangeTarget.x, position.y+1f*rangeTarget.y, rangeTarget.x,rangeTarget.y, 5));
        }
        else if (Mathf.Abs(rangeTarget.x) <= Mathf.Abs(rangeTarget.y))
            if (rangeTarget.y < 0)
            {
                linearMove.x = 0;
                linearMove.y = -1;
            }
            else
            {
                linearMove.x = 0;
                linearMove.y = 1;
            }
        else
        {
            if (rangeTarget.x < 0)
            {
                linearMove.x = -1;
                linearMove.y = 0;
            }
            else
            {
                linearMove.x = 1;
                linearMove.y = 0;
            }
        }
    }
    public override void onWalkAnimation()
    {

    }
    public override void onWalkFinish()
    {
        
    }
    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        if(obj is Bullet)
        {
            this.getDamage(10);
        }
    }

    public PlayerStalker(float x, float y, Player play):base(x,y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
        linearMove = new Vector2();
        target = play;
    }
}
