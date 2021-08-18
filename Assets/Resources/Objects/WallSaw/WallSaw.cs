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
    float waitTime = 0.5f;

    Wall saveWall;
    
    List<WalkAndLive> saveObj  = new List<WalkAndLive>();

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

        newPosition = position+direction;
        timeOnMove = 1/speed;
        order = ObjectOrder.underWall;
        timeRunner = timeOnMove;
    }

    public override void updateObject()
    {
        if (waitTime <=0)
        {
            timeRunner -= Time.deltaTime;
            
            if (timeRunner <= 0)
            {
                saveObj = new List<WalkAndLive>();
                timeRunner += timeOnMove;
                var tmpList = map.getMapObjects<Wall>(Mathf.RoundToInt(position.x+directionOfWall.x),Mathf.RoundToInt(position.y+directionOfWall.y), x=>  x is Wall);
                if (tmpList == null)
                {
                    position = newPosition;
                    var tmpVector = direction;
                    direction =directionOfWall;
                    directionOfWall = -tmpVector;
                    newPosition = saveWall.position-directionOfWall;
                }
                else if (map.getMapObjects<Wall>(Mathf.RoundToInt(position.x+direction.x),Mathf.RoundToInt(position.y+direction.y), x=>  x is Wall) != null && map.getMapObjects<Wall>(Mathf.RoundToInt(position.x-directionOfWall.x),Mathf.RoundToInt(position.y-directionOfWall.y), x=>  x is Wall) != null)
                {
                    //position = newPosition;
                    tmpList = map.getMapObjects<Wall>(Mathf.RoundToInt(position.x-directionOfWall.x),Mathf.RoundToInt(position.y-directionOfWall.y), x=>  x is Wall);
                    direction = -direction;
                    directionOfWall = -directionOfWall;
                    newPosition = tmpList[0].position-directionOfWall+direction;
                }
                else if (map.getMapObjects<Wall>(Mathf.RoundToInt(position.x+direction.x),Mathf.RoundToInt(position.y+direction.y), x=>  x is Wall) != null)
                {
                    tmpList = map.getMapObjects<Wall>(Mathf.RoundToInt(position.x+direction.x),Mathf.RoundToInt(position.y+direction.y), x=>  x is Wall);
                    position = newPosition;
                    var tmpVector = direction;
                    direction = -directionOfWall;
                    directionOfWall = tmpVector;
                }
                if (tmpList != null)
                {
                    newPosition = tmpList[0].position-directionOfWall+direction;
                    saveWall =tmpList[0];
                }
                
            }
            
            
            position +=direction*speed*Time.deltaTime;
            
            var tmpDamageList = map.getMapObjects<WalkAndLive>(Mathf.RoundToInt(position.x),Mathf.RoundToInt(position.y), x=>  x is WalkAndLive);
            if (tmpDamageList != null)
                {
                    for (int i = 0; i != tmpDamageList.Count; i++)
                    {
                        if (!saveObj.Contains(tmpDamageList[i]))
                        {
                            tmpDamageList[i].getDamage(damage);
                            saveObj.Add(tmpDamageList[i]);
                        }
                    }
                }
        }
        else 
            waitTime -= Time.deltaTime;
    }

    public WallSaw(float x, float y, float _speed= 0, float _damag = 5, bool dir = false): base(x,y)
    {
        if (_speed == 0)
            speed = 5;
        else
            speed = _speed;
        damage = _damag;
        firstDirection = dir;
    }
}
