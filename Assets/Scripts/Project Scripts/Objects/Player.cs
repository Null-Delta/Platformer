using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: WalkableObject
{
    
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;

    public bool isAnimFinish;
    public Queue<int> direction = new Queue<int>();

    

    override public void onStartWalk() {
        // linearMove.x = 0;
        // linearMove.y = 0;
        var newDir = movements.Peek();
        gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);

        // switch (newDir.point) {
        //     case Vector2Int.down:
        //     break;
        //     case 1:
        //         gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
        //     break;
        //     case 2:
        //         gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
        //     break;
        //     case 3:
        //         gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
        //     break;
        // }

        // isAnimFinish = false;
        // dir = newDir;

        //Vector2 tmpNewPosition = mapLocation;

        

        // if (isOnFloor)
        //     isOnFloor = false;

        // var tmpList = map.getMapObjects<MovingFloor>((int)tmpNewPosition.x,(int)tmpNewPosition.y, x => x is MovingFloor);
        // if(tmpList != null)                                   // обработка возможности вхождения на движущийся пол.
        // {
        //     if (tmpList[0].onMe ==null)
        //     {
        //         tmpList[0].addWalkerOn(tmpNewPosition ,this);
        //         isOnFloor = true;
        //     }
        //     else if (tmpList[0].onMe.isCollisiable)
        //     {
        //         if(linearMove.x != 0) linearMove.x = 0;
        //         if(linearMove.y != 0) linearMove.y = 0;
        //     }
        // }

    }

    public void addDirection(int dir)
    {
        if(direction.Count < 2)
            direction.Enqueue(dir);
    }

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.0f;
        Camera.main.GetComponent<PlayerControl>().CurrentPlayer = this;
        Camera.main.GetComponent<CamControl>().targetObj = this.gameObject;
        order = ObjectOrder.wall;
    }

    public override bool canMoveOn(Vector2Int point)
    {
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x.objectName == "Box") != null) {
                int direction = -1;
                if(movements.Peek().point.x == -1) direction = 0;
                if(movements.Peek().point.y == 1) direction = 1;
                if(movements.Peek().point.x == 1) direction = 2;
                if(movements.Peek().point.y == -1) direction = 3;
                map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].setDirection(direction);
                return map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].willMove();
            }
        }


        return map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable == true) == null;
    }

    override public void onEndWalk() {
        stepCount++;
    }

    public Player(int x, int y): base(x,y) {
        
    }
}
