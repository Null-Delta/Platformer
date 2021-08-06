using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrel_bullet : Live_wall
{
    public override string objectName => "Turrel_bullet";

    public override void startObject()
    {
        base.startObject();
        isCollisiable = true;
        gameObject.transform.position = position;
        order = ObjectOrder.wall;
    }



    override public bool readyCheck() 
    {
        bool there_walker= false;
        for (int step = 1; step != 6; step++)
        {
            if (map.getMapObjects<MapObject>((int)(position.x+step),(int)position.y, x=> x is WalkableObject) != null )
            {
                there_walker = true;
                break;
            }
            else if (map.getMapObjects<MapObject>((int)(position.x-step),(int)position.y,x=> x is WalkableObject ) != null)
            {
                there_walker = true;
                break;
            }
            else if (map.getMapObjects<MapObject>((int)position.x,(int)(position.y-step),x=> x is WalkableObject ) != null)
            {
                there_walker = true;
                break;
            } else if (map.getMapObjects<MapObject>((int)position.x,(int)(position.y-step),x=> x is WalkableObject) != null)
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
        map.setupObject(new Bullet(position.x+0.5f, position.y, 1,0, 5));
        map.setupObject(new Bullet(position.x, position.y+0.5f, 0,1, 5));
        map.setupObject(new Bullet(position.x-0.5f, position.y, -1,0, 5));
        map.setupObject(new Bullet(position.x, position.y-0.5f, 0,-1, 5));
    }

    public Turrel_bullet(int x, int y, float _act_delay, float _animation_time): base(x,y, _act_delay, _animation_time) {
        act_delay = _act_delay;
        animation_time = _animation_time;
    }
}