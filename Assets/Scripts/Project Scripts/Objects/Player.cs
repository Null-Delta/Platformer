using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: Walker
{
    
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;
    public bool isAnimFinish;
    public Queue<int> direction = new Queue<int>();
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

        if(map.getMapObjects<MapObject>((int)position.x + (int)linearMove.x, (int)position.y + (int)linearMove.y, x => x.objectName == "Floor") == null || map.getMapObjects<MapObject>((int)(position.x + linearMove.x),
            (int)(position.y + linearMove.y), x => x.isCollisiable || x is Walker) != null) 
        {
            if(linearMove.x != 0) linearMove.x = 0;
            if(linearMove.y != 0) linearMove.y = 0;
        }
    }

    public void addDirection(int dir)
    {
        if(direction.Count < 2)
            direction.Enqueue(dir);
    }

    override public bool readyCheck()
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
    public override void startObject()
    {
        base.startObject();   
        linearMove.x = 0;
        linearMove.y = 0;
        move_delay = 0.0f;
        animation_time = 0.2f;
        Camera.main.GetComponent<PlayerControl>().CurrentPlayer = this;
        Camera.main.GetComponent<CamControl>().targetObj = this.gameObject;
        //Camera.main.GetComponent<CamControl>(
    }
    override public void onWalkFinish() {
        direction.Dequeue();
        stepCount++;
    }

    public Player(float x, float y): base(x,y) {
        
    }
}
