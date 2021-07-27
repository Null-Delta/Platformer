using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live_wall : MapObject
{
    float sum_time;
    public float act_delay;
    public float animation_time;
    bool in_animation = false;
    bool is_ready = true;
    public override string objectName => "Live_wall";


    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1000;
    }



    public virtual bool readyCheck()
    {
        return true;
    }

    public virtual void actStart()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-2);
        
    }

    public virtual void actAnimation()
    {
        
    }

    public virtual void actFinish()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1000;
        map.setupObject(new Bullet(position.x, position.y));
    }




    public override void updateObject(float time) {
        
        sum_time+=time; //важно
        is_ready = readyCheck();
    
        if (sum_time >= act_delay && !in_animation && is_ready)
        {
            sum_time =act_delay;
            in_animation = true;
            
            actStart();
        
        }
        
        if (in_animation)
        {
            if(sum_time >= act_delay + animation_time) {
                sum_time = 0f;
                in_animation = false;
                actFinish();
            }
            actAnimation();
        }
    }


    public Live_wall(int x, int y, float _act_delay ,float _animation_time) {
        position = new Vector2(x,y);
        act_delay = _act_delay;
        animation_time = _animation_time;
    }
    

    
}