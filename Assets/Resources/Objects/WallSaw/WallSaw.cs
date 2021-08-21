using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSaw : MapObject
{
    public override string objectName => "WallSaw";
    public Vector2 direction;
    public Vector2 directionOfWall;

    public List<Vector2> movePoints = new List<Vector2>();
    public List<Vector2> damagePoints = new List<Vector2>();
    Vector2 speedVector;
    float speed;
    float damage = 5;
    int step = -1;

    float timeOnMove=0;
    float timeRunner=0;
    float waitTime=1f;
    bool isReady = false;

    Wall saveWall;
    
    List<WalkAndLive> saveObj  = new List<WalkAndLive>();

    public override void startObject()
    {
        timeOnMove = 1/speed;
        order = ObjectOrder.underWall;
        timeRunner = timeOnMove;
    }

    int nextStep(int i)
    {
        return (i+1) % movePoints.Count;
    }

    void foundPath()
    {
        movePoints.Add(position);
        damagePoints.Add(directionOfWall);
        Vector2 tmpPosition = position;

        while (true)
        {
            tmpPosition+=direction;
            
            if (map.getMapObjects<Wall>(Mathf.RoundToInt(tmpPosition.x+directionOfWall.x*0.5f),Mathf.RoundToInt(tmpPosition.y+directionOfWall.y*0.5f), x=>  x is Wall) == null)
            {
                tmpPosition -=direction*0.5f;
                movePoints.Add(tmpPosition);
                damagePoints.Add(directionOfWall-direction);

                var tmpVector = direction;
                direction =directionOfWall;
                directionOfWall = -tmpVector;

                tmpPosition +=direction*0.5f;
            }
            else if (map.getMapObjects<Wall>(Mathf.RoundToInt(tmpPosition.x-directionOfWall.x*0.5f),Mathf.RoundToInt(tmpPosition.y-directionOfWall.y*0.5f), x=>  x is Wall) != null)
            {
                tmpPosition -=direction*0.5f;
                movePoints.Add(tmpPosition);
                damagePoints.Add(directionOfWall+direction);

                var tmpVector = direction;
                direction = -directionOfWall;
                directionOfWall = tmpVector;
                
                tmpPosition +=direction*0.5f;
            }

            if (tmpPosition == movePoints[0])
                break;
            movePoints.Add(tmpPosition);
            damagePoints.Add(directionOfWall);
        }
    }

    public override void updateObject()
    {
        if (isReady && waitTime <=0)
        {
            if (timeRunner >= timeOnMove)
            {
                
                step = nextStep(step);
                timeRunner -=timeOnMove;
                timeOnMove = ((movePoints[nextStep(step)]-movePoints[step]).magnitude)/speed;
                saveObj = new List<WalkAndLive>();
            }

            timeRunner += Time.deltaTime;
            position = movePoints[step] + (movePoints[nextStep(step)]-movePoints[step])*(timeRunner/timeOnMove);

            var tmpDamageList = map.getMapObjects<WalkAndLive>(Mathf.RoundToInt(position.x-damagePoints[step].x*0.5f),Mathf.RoundToInt(position.y-damagePoints[step].y*0.5f), x=>  x is WalkAndLive);
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
        {
            waitTime -=Time.deltaTime;
            if (isReady == false)
            {
                isReady = true;
                foundPath();
            }
        }
    }

    public WallSaw(float x, float y, Vector2 d, Vector2 dw, float _speed, float _damag)
    {
        speed = _speed;
        damage = _damag;
        direction = d;
        directionOfWall = dw;
        startPosition= new Vector2(x,y);
        startPosition +=directionOfWall*0.5f;
        position = startPosition;
    }
}
