using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MovableMapObject
{

    float sum_time;

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
        move = new LinearMove(Random.Range(-1f,1f),Random.Range(-1f,1f),1);
        radius = 1f;
    }

    public override StateEvent stateCheck(float time) { 
        if(sum_time + time >= 1)
        {
            return new StateEvent(sum_time + time -1, this);
        }
         return null;
        }

    public override void updateObject(float time) {
        sum_time+=time;

        if (sum_time >= 1)
        {
            linearMove.dx = Random.Range(-1f,1f);
            linearMove.dy = Random.Range(-1f,1f);
            position += new Vector2((int)(linearMove.dx * linearMove.speed * 1),(int)(linearMove.dy * linearMove.speed * 1));
            gameObject.transform.position = position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 1);
            sum_time -=1;
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
