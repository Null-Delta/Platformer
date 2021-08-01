using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndLive : WalkableObject, Health
{
    public override string objectName => "WalkAndLive";


    public float hp { get;set;}
    public float immortalTime {get;set;}
    public float nowImmortalTime {get;set;}

    public float savedStayDelay;
    public Vector2 lastFloor;

    public virtual void getDamage(float damage)
    {
        if (nowImmortalTime <=0)
        {

            hp -=damage;
            if (hp <= 0)
            {
                onDeath();
                map.destroyObject(this);
            }
            else
                onGetDamage(damage);
            nowImmortalTime = immortalTime;
        }
    }

    public virtual void onDeath()
    {

    }
    public virtual void onGetDamage(float damage)
    {

    }


    public virtual void onFall()
    {
        getDamage(15);
        savedStayDelay = stayDelay;
        stayDelay = 0.7f;      //    fallingTime
        movements.Clear();
        Debug.Log(lastFloor );

        Debug.Log(lastFloor - new Vector2(gameObject.transform.position.x,gameObject.transform.position.y ));
        movements.Enqueue(new movement(Vector2Int.RoundToInt(lastFloor - new Vector2(gameObject.transform.position.x,gameObject.transform.position.y )), false));
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {

    }



    public override void addedUpdateObject(float time)
    {
        nowImmortalTime -=time;
    }

    override public void onStartWalk()
    {
        if (savedStayDelay != -1)
        {
            stayDelay = savedStayDelay;
            savedStayDelay = -1;
        }
    }

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.0f;
        isCollisiable = true;
        order = ObjectOrder.wall;

        hp = 100;
        immortalTime = 1;
    }

    public override bool canMoveOn(Vector2Int point)
    {
        return true;
    }

    override public void onEndWalk() 
    {
        if (map.getMapObjects<MapObject>((int)position.x, (int)position.y, x => x.objectName == "Floor" || x.objectName == "MovingFloor") == null)
        {
            onFall();
        }
        else
        {
            lastFloor = position;
        }
    }

    public WalkAndLive(int x, int y): base(x,y) {
        
    }

}
