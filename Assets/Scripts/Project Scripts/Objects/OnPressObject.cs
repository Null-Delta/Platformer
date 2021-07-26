using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPressObject : StaticMapObject
{
    float sum_time;

    public override string objectName => "OnPressObject";


    public virtual void OnPress(Walker who)
    {
        map.spawn_object(new OnPressObject((int)position.x+1, (int)position.y+1));
        map.spawn_object(new OnPressObject((int)position.x-1, (int)position.y-1));
        map.spawn_object(new OnPressObject((int)position.x-1, (int)position.y+1));
        map.spawn_object(new OnPressObject((int)position.x+1, (int)position.y-1));
        map.deleteObject(this);
    }


    public override void startObject()
    {
        base.startObject();
        isDecoration = true;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-1);
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
