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

    //////////////////////////////////////////
    //переменные, отвечающие за свойства
    public bool canFall = false;
    ///////////////////////////////////////////

    public void getDamage(float damage)
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
        movements.Enqueue(new movement(Vector2Int.RoundToInt(lastFloor - new Vector2(position.x,position.y )), false));
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {

    }



    public override void startObject()
    {
        base.startObject();   
        isCollisiable = true;
    }

    public override bool canMoveOn(Vector2Int point)
    {
        return true;
    }



    override public void onStartWalk()
    {
        if (savedStayDelay != -1)  // оглушение
        {
            stayDelay = savedStayDelay;
            savedStayDelay = -1;
        }
    }

    public override void updateObject()
    {
        if (nowImmortalTime > 0)
            nowImmortalTime -= Time.deltaTime;
        base.updateObject();
    }

    override public void onEndWalk() 
    {
        if (canFall)   // падение
        {
            if (map.getMapObjects<MapObject>((int)mapLocation.x, (int)mapLocation.y, x => x.objectName == "Floor" || x.objectName == "MovingFloor") == null)
            {
                onFall();
            }
            else if (map.getMapObjects<MapObject>((int)mapLocation.x, (int)mapLocation.y, x => x.objectName == "Floor") !=null)
            {
                lastFloor = position;
            }
        }
    }

    public WalkAndLive(int x, int y): base(x,y) {
        
    }

}
