using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MapObject
{
    public override string objectName => "Bullet";
    public Vector2 direction;
    float speed;
    float damage = 5;
    public override void startObject()
    {
        base.startObject();
        gameObject.GetComponent<Rigidbody2D>().velocity = direction;
        order = ObjectOrder.wall;
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        if(obj is Wall || obj is Door) {
            var contact = collision.contacts[0];

            if(contact.normal.x > 0) {
                direction.x = Mathf.Abs(direction.x);
            } else if(contact.normal.x < 0) {
                direction.x = -Mathf.Abs(direction.x);
            }

            if(contact.normal.y > 0) {
                direction.y = Mathf.Abs(direction.y);
            } else if(contact.normal.y < 0) {
                direction.y = -Mathf.Abs(direction.y);
            }
            
            gameObject.GetComponent<Rigidbody2D>().velocity = direction;

        }else if(obj is WalkAndLive) {
            (obj as WalkAndLive).getDamage(damage);
            map.destroyObject(this);
        }
         else if(obj is WalkableObject) {
            map.destroyObject(this);
        } else if(obj is Bullet) {
            var contact = collision.contacts[0];
            direction = contact.normal * speed;
            gameObject.GetComponent<Rigidbody2D>().velocity = direction;
        }
    }
    public Bullet(float x, float y): base(x,y) {
        speed = 5;
        direction = new Vector2(Random.Range(-1f,1f) * speed, Random.Range(-1f,1f) * speed);
    }

    public Bullet(float x, float y, float xd, float yd, float _speed= 0, float _damag = 5): base(x,y) {
        if (_speed == 0)
            speed = 5;
        else
            speed = _speed;
        direction = new Vector2(xd * speed,yd * speed);
        damage = _damag;
    }
}
