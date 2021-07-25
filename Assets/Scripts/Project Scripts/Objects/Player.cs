using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: Walker
{
    
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;
    public Queue<int> direction = new Queue<int>();
    override public void onWalkStart() {
        linearMove.x = 0;
        linearMove.y = 0;

        switch (direction.Peek()) {
            case 0:
                gameObject.GetComponent<Animator>().Play("MoveUp",0,0);
                linearMove.y = 1;
            break;
            case 1:
                gameObject.GetComponent<Animator>().Play("MoveRight",0,0);
                linearMove.x = 1;
            break;
            case 2:
                gameObject.GetComponent<Animator>().Play("MoveDown",0,0);
                linearMove.y = -1;
            break;
            case 3:
                gameObject.GetComponent<Animator>().Play("MoveLeft",0,0);
                linearMove.x = -1;
            break;
        }
            
        if(map.getMapObjects<StaticMapObject>((int)position.x + (int)linearMove.x, (int)position.y + (int)linearMove.y, x => x.objectName == "Floor") == null || map.getMapObjects<StaticMapObject>((int)(position.x + linearMove.x),
            (int)(position.y + linearMove.y), x => x.isDecoration == false || x is Walker) != null) 
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
            gameObject.GetComponent<Animator>().Play("MoveStop",0,0);
            return false;
        }
            
        return true;
    }

    public override void startObject()
    {
        base.startObject();
        Camera.main.GetComponent<PlayerControl>().CurrentPlayer = this;
        linearMove.x = 1;
        linearMove.y = 0;
        move_delay = 0.0f;
        animation_time = 0.2f;
    }
    override public void onWalkFinish() {
        direction.Dequeue();
    }

    public override bool isCollizion(MapObject obj)
    {
        return true;
    }

    public Player(float x, float y): base(x,y) {

    }
}
