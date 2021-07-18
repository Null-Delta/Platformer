using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MovableMapObject
{

    float sum_time;
    float move_delay;

    public override string objectName => "Walker";
    LinearMove linearMove {
        get {
            return move as LinearMove;
        }
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y);
        move = new LinearMove(1,0,1);
        radius = 1f;
        move_delay = 0.5f;
    }

    public override StateEvent stateCheck(float time) { 
        

        if(sum_time + time >= move_delay && sum_time < move_delay)
        {
            return new StateEvent(time - (sum_time + time - move_delay), this);
        }
         return null;
        }

    public override void updateObject(float time) {
        
        sum_time+=time; //важно

        if (sum_time >= move_delay)
        {
            
            //linearMove.dx = Random.Range(-1,2);
            //linearMove.dy = Random.Range(-1,2);

            position += new Vector2((int)(linearMove.dx * linearMove.speed),(int)(linearMove.dy * linearMove.speed));
            gameObject.transform.position = position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 1);
            sum_time = 0f;
        }
    }

    public override bool isCollizion(MapObject obj)
    {
        return true;
    }

    public override void onCollizion(MapObject obj, Vector2 orientation)
    {//todo
    
        if(obj is Wall) {
            
            if(orientation.x != 0) {
                linearMove.dx = -linearMove.dx;
            }

            if(orientation.y != 0) {
                linearMove.dy = -linearMove.dy;
            }
        } else if (obj is Bullet) {
            var vs = (position - obj.position).normalized;
            linearMove.dx = vs.x;
            linearMove.dy = vs.y;
        }
    }
    public Walker(float x, float y) {
        position = new Vector2(x,y);
    }
}
