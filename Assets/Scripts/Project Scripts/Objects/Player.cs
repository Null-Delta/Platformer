using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: Walker
{
    
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;
    public bool isAnimFinish;
    public Queue<int> direction = new Queue<int>();

    public bool isFalling = false;
    public float fallingTime = 0.6f;
    public Vector2 lastFloor;
    bool isOnFloor = false;

    override public void onWalkStart() {
        linearMove.x = 0;
        linearMove.y = 0;
        var newDir = direction.Peek();
        switch (direction.Peek()) {
            case 0:
                gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
                linearMove.y = 1;
            break;
            case 1:
                gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
                linearMove.x = 1;
            break;
            case 2:
                gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
                linearMove.y = -1;
            break;
            case 3:
                gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
                linearMove.x = -1;
            break;
        }

        isAnimFinish = false;
        dir = newDir;

        Vector2 tmpNewPosition = position+linearMove;
        if( map.getMapObjects<MapObject>(Mathf.RoundToInt(tmpNewPosition.x),Mathf.RoundToInt(tmpNewPosition.y), x => x.isCollisiable) != null) 
        {

            if(map.getMapObjects<MapObject>((int)tmpNewPosition.x, (int)tmpNewPosition.y, x => x.objectName == "Box") != null) {
                int direction = -1;
                if(linearMove.x == -1) direction = 0;
                if(linearMove.y == 1) direction = 1;
                if(linearMove.x == 1) direction = 2;
                if(linearMove.y == -1) direction = 3;

                map.getMapObjects<Box>((int)tmpNewPosition.x, (int)tmpNewPosition.y, x => x.objectName == "Box")[0].setDirection(direction);

                if(!map.getMapObjects<Box>((int)tmpNewPosition.x, (int)tmpNewPosition.y, x => x.objectName == "Box")[0].willMove()) {
                     if(linearMove.x != 0) linearMove.x = 0;
                    if(linearMove.y != 0) linearMove.y = 0;
                }
            } else {
                if(linearMove.x != 0) linearMove.x = 0;
                if(linearMove.y != 0) linearMove.y = 0;
            }
        }

        if (isOnFloor)
            isOnFloor = false;
        var tmpList = map.getMapObjects<MovingFloor>((int)tmpNewPosition.x,(int)tmpNewPosition.y, x => x is MovingFloor);
        if(tmpList != null && tmpList[0].onMe ==null)
        {
            tmpList[0].addWalkerOn(tmpNewPosition ,this);
            isOnFloor = true;
        }

    }

    public void addDirection(int dir)
    {
        if(direction.Count < 2)
            direction.Enqueue(dir);
    }

    override public bool readyCheck()
    {
        if (!isFalling)
        {
            if(direction.Count == 0)
            {
                if(dir != -1) {
                    dir = -1;
                    //gameObject.GetComponent<Animator>().Play("MoveStop",0,0);
                    isAnimFinish = true;
                }
                return false;
            }
                
            return true;
        }
        else if (sum_time <= fallingTime)
        {
            Debug.Log("Падаю");
            return false;
        }
        else
        {
            isFalling = false;
            getDamage(15);
            map.moveMapObject(lastFloor, this);
            return false;
        }
    }
    public override void startObject()
    {
        base.startObject();   
        linearMove.x = 0;
        linearMove.y = 0;
        move_delay = 0.0f;
        animation_time = 0.2f;
        Camera.main.GetComponent<PlayerControl>().CurrentPlayer = this;
        Camera.main.GetComponent<CamControl>().targetObj = this.gameObject;
        lastFloor = position;
    }
    override public void onWalkFinish() {
        direction.Dequeue();
        stepCount++;

        if (map.getMapObjects<MapObject>((int)position.x, (int)position.y, x => x.objectName == "Floor") != null && !isOnFloor)
        {
            lastFloor = position;
        }
        else if (!isOnFloor)
        {
            // начало падения
            isFalling = true;
            sum_time = 0;

        }

    }

    public Player(float x, float y): base(x,y) {
        
    }
}
