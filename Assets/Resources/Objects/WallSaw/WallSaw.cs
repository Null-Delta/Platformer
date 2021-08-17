using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSaw : MapObject
{
    public override string objectName => "WallSaw";
    public Vector2 direction;
    public Vector2 directionOfWall;
    public Vector2 newPosition;
    float speed;
    float damage = 5;
    bool firstDirection;

    float timeOnMove;
    float timeRunner;
    

    public override void startObject()
    {
        isCollisiable = true;
        List<Wall> tmpList = map.getMapObjects<Wall>(new List<Vector2>{position+ Vector2.up, position+ Vector2.down,position+ Vector2.left, position+ Vector2.right }, x => x is Wall);
        if (tmpList == null)
            Debug.Log("Где стенка, эээ!");
        else
            directionOfWall =tmpList[0].position;

        if (directionOfWall.x - this.position.x == 0)
            if (firstDirection)
                direction = Vector2Int.right;
            else
                direction = Vector2Int.left;
        else
            if (firstDirection)
                direction = Vector2Int.up;
            else
                direction = Vector2Int.left;
        
        directionOfWall -=this.position;
        directionOfWall.Normalize();

        gameObject.GetComponent<Rigidbody2D>().velocity = direction*speed;
        newPosition = position+direction;
        timeOnMove = 1/speed;
        order = ObjectOrder.underWall;
        timeRunner = timeOnMove;
    }

    public override void updateObject()
    {
        timeRunner -= Time.deltaTime;
        if (timeRunner <= 0)
        {
            timeRunner += timeOnMove;
            var tmpList = map.getMapObjects<Wall>(Mathf.RoundToInt(position.x+directionOfWall.x),Mathf.RoundToInt(position.y+directionOfWall.y), x=>  x is Wall);
            if (tmpList == null)
            {
                position = newPosition;
                var tmpVector = direction;
                direction =directionOfWall;
                directionOfWall = -tmpVector;
                gameObject.GetComponent<Rigidbody2D>().velocity = direction*speed;
            }
            else
            {
                newPosition = tmpList[0].position-directionOfWall+direction;
            }
            
        }
        
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        Debug.Log(1);
        if(obj is WalkAndLive) {
            (obj as WalkAndLive).getDamage(damage);
        }
    }

    public WallSaw(float x, float y, float _speed= 0, float _damag = 5, bool dir = false): base(x,y){
        if (_speed == 0)
            speed = 5;
        else
            speed = _speed;
        damage = _damag;
        firstDirection = dir;
    }
}
