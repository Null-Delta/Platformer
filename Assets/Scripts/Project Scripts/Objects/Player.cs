using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: WalkAndLive
{
    
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;

    public bool isAnimFinish;
    public Queue<int> direction = new Queue<int>();

    public CheckPoint nowCheckPoint;
    

    override public void onStartWalk() {
        // linearMove.x = 0;
        // linearMove.y = 0;
        //var newDir = movements.Peek();
        if (savedStayDelay != -1)
        {
            Camera.main.GetComponent<PlayerControl>().enabled = true;   
        }
        base.onStartWalk();
        

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
        
        hp = 100;
        immortalTimeForHit = 0.5f;
        canFall = true;
    }

    public override bool canMoveOn(Vector2Int point)
    {
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
                return (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements.Peek().point));
            }
            if (map.getMapObjects<MapObject>(point.x, point.y, x => x is CheckPoint) != null)
            {
                return true;
            }
        }


        return map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
    }

    override public void onEndWalk() {
        base.onEndWalk();

        stepCount++;
    }

    override public void onDeath()
    {
        if (nowCheckPoint != null)
        {
            Camera.main.GetComponent<PlayerControl>().enabled = true;
            nowCheckPoint.spawnPlayer();
        }
    }


    public Player(int x, int y): base(x,y) {
        
    }
}
