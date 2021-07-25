using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: Walker
{
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;
    public Queue<int> direction = new Queue<int>();
    override public void onWalkStart() {
        linearMove.dx = 0;
        linearMove.dy = 0;

        switch (direction.Peek()) {
            case 0:
                gameObject.GetComponent<Animator>().Play("MoveUp",0,0);
                linearMove.dy = 1;
            break;
            case 1:
                gameObject.GetComponent<Animator>().Play("MoveRight",0,0);
                linearMove.dx = 1;
            break;
            case 2:
                gameObject.GetComponent<Animator>().Play("MoveDown",0,0);
                linearMove.dy = -1;
            break;
            case 3:
                gameObject.GetComponent<Animator>().Play("MoveLeft",0,0);
                linearMove.dx = -1;
            break;
        }
            
        if(map.getMapObjects<StaticMapObject>((int)position.x + (int)linearMove.dx, (int)position.y + (int)linearMove.dy, x => x.objectName == "Floor") == null || map.getMapObjects<StaticMapObject>((int)(position.x + linearMove.dx),
            (int)(position.y + linearMove.dy), x => x.isDecoration == false) != null ||
            map.checkWalkerPoint(new Vector2(linearMove.dx +position.x,linearMove.dy+position.y))) 
        {
            if(linearMove.dx != 0) linearMove.dx = 0;
            if(linearMove.dy != 0) linearMove.dy = 0;
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
        linearMove.dx = 1;
        linearMove.dy = 0;
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
