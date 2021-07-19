using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovableMapObject
{
    public override string objectName => "Bullet";
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
        move = new LinearMove(-1,0,5);
        radius = 0.25f;
    }

    public override void updateObject(float time) {
        position += new Vector2(linearMove.dx * linearMove.speed * time,linearMove.dy * linearMove.speed * time);
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 0);
    }

    public override bool isCollizion(MapObject obj)
    {
        return true;
    }

    public override void onCollizion(MapObject obj, Vector2 orientation)
    {
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
        } else if (obj is Walker) {
            map.deleteObject(this);
        }
    }
    public Bullet(float x, float y) {
        position = new Vector2(x,y);
        move = new LinearMove(Random.Range(-1f,1f),Random.Range(-1f,1f), 5);
        radius = 0.25f;
    }
}
