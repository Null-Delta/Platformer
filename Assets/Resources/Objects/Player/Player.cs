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
        base.onStartWalk();
        gameObject.GetComponent<Animator>().Play("PlayerLeft" + ((stepCount % 2 == 0) ? "LeftLeg" : "RightLeg"),0,0);
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
                return ((map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject)[0] as PushableObject).tryPush(movements[0].point));
            }
            if (map.getMapObjects<MapObject>(point.x, point.y, x => x is CheckPoint) != null)
            {
                return true;
            }
        }


        return map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable) == null;
    }

    override public void onEndWalk() {
        base.onEndWalk();

        stepCount++;
    }

    override public void onDeath()
    {
        if (nowCheckPoint != null)
        {
            Camera.main.GetComponent<PlayerControl>().ControlActive = false;
            Camera.main.GetComponent<PlayerControl>().CurrentPlayer = null;
            nowCheckPoint.spawnPlayer();
        }
    }


    public Player(int x, int y): base(x,y) {
        
    }
}
