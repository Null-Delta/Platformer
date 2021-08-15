using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBullet : MapObject
{
    public override string objectName => "RoundBullet";
    public Vector2 direction;
    float speed;
    float damage = 5;
    MapObject owner = null;
    float radius;
    public override void startObject()
    {
        direction = position - owner.position;
        direction.Normalize();
        float tmpFloat = direction.x;
        direction.x = direction.y;
        direction.y = -tmpFloat;

        gameObject.GetComponent<Rigidbody2D>().velocity = direction*speed;
        order = ObjectOrder.wall;

        this.gameObject.transform.SetParent(owner.gameObject.transform);
    }
    public override void updateObject()
    {
        direction = position - owner.position;
        direction.Normalize();
        float tmpFloat = direction.x;
        direction.x = direction.y;
        direction.y = -tmpFloat;

        gameObject.GetComponent<Rigidbody2D>().velocity = direction*speed;
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        if(obj is Wall || obj is Door) {
            map.destroyObject(this);
        }
        else if(obj == owner) {
           map.destroyObject(this);
        }
        else if(obj is WalkAndLive) {
            (obj as WalkAndLive).getDamage(damage);
            map.destroyObject(this);
        }
         else if(obj is WalkableObject) {
            map.destroyObject(this);
        } else if(obj is Bullet) {
            map.destroyObject(this);
        }
    }

    public RoundBullet(MapObject _owner = null, float _speed= 0, float _damag = 5, float rad =0){
        if (_speed == 0)
            speed = 5;
        else
            speed = _speed;
        damage = _damag;
        owner = _owner;
        if (rad != 0)
            radius = rad;
        else
            radius = Random.Range(1f,2f);

        float tmpFloat = Random.Range(-radius,radius);
        int tmpSign = Random.Range(0,2)*2-1;
        this.startPosition.x = owner.position.x + tmpFloat;
        this.startPosition.y = owner.position.y + tmpSign*Mathf.Sqrt(radius*radius - tmpFloat*tmpFloat);
    }
}
