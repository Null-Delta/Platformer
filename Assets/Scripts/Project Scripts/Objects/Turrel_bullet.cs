using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrel_bullet : Live_wall
{
    public override string objectName => "Turrel_bullet";
    public override void startObject()
    {
        base.startObject();

        isDecoration = true;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder  = -(int)(position.y-2)+1;
    }



    override public bool readyCheck() 
    {
        bool there_walker= false;
        for (int step = 1; step != 6; step++)
        {
            if (map.checkWalkerPoint(new Vector2(position.x+step,position.y)))
            {
                there_walker = true;
                break;
            }
            else if (map.checkWalkerPoint(new Vector2(position.x,position.y+step)))
            {
                there_walker = true;
                break;
            }
            else if (map.checkWalkerPoint(new Vector2(position.x-step,position.y)))
            {
                there_walker = true;
                break;
            } else if (map.checkWalkerPoint(new Vector2(position.x,position.y-step)))
            {
                there_walker = true;
                break;
            }
        }
        return there_walker;
    }

    override public void actStart() 
    {
        
    }

    override public void actAnimation() 
    {

    }

    override public void actFinish()
    {
        map.spawn_object(new Bullet(position.x, position.y, 1,0));
        map.spawn_object(new Bullet(position.x, position.y, 0,1));
        map.spawn_object(new Bullet(position.x, position.y, -1,0));
        map.spawn_object(new Bullet(position.x, position.y, 0,-1));
    }




    public override bool isCollizion(MapObject obj)
    {
        return true;
    }

    public Turrel_bullet(int x, int y, float _act_delay, float _animation_time): base(x,y, _act_delay, _animation_time) {
        position = new Vector2(x,y);
        act_delay = _act_delay;
        animation_time = _animation_time;
    }
}