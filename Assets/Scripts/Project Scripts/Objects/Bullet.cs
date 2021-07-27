using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MapObject
{
    public override string objectName => "Bullet";
    public Vector2 direction;

    float speed = 5f;
    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y);
        direction = new Vector2(1f * speed, 1f * speed);
        gameObject.GetComponent<Rigidbody2D>().velocity = direction;
    }

    public override void updateObject(float time) {
        position = gameObject.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 1);
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        if(obj is Wall) {
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

        } else if(obj is Walker) {
            map.destroyObject(this);
        } else if(obj is Bullet) {
            var contact = collision.contacts[0];
            direction = contact.normal * speed;
            gameObject.GetComponent<Rigidbody2D>().velocity = direction;
        }
    }
    public Bullet(float x, float y) {
        position = new Vector2(x,y);
    }

    public Bullet(float x, float y, float xd, float yd) {
        position = new Vector2(x,y);
    }
}
