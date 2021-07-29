using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPressObject : MapObject
{
    float sum_time;

    public override string objectName => "OnPressObject";


    public virtual void OffPress(Walker who)
    {
        
    }
    public virtual void OnPress(Walker who)
    {
        map.setupObject(new OnPressObject((int)position.x+1, (int)position.y+1));
        map.setupObject(new OnPressObject((int)position.x-1, (int)position.y-1));
        map.setupObject(new OnPressObject((int)position.x-1, (int)position.y+1));
        map.setupObject(new OnPressObject((int)position.x+1, (int)position.y-1));
        map.destroyObject(this);
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y);
    }


    public override void updateObject(float time) 
    {
        sum_time+=time; //важно
    }


    public OnPressObject(int x, int y) 
    {
        position = new Vector2(x,y);
    }
    

}
