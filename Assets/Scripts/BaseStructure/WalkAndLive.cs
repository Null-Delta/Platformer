using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndLive : WalkableObject, Health
{
    public override string objectName => "WalkAndLive";


    public float hp { get;set;}
    public float immortalTimeForHit {get;set;}
    public float immortalTime {get;set;}

    public float stanTime = 0;
    public Vector2 lastFloor;
    bool actFall = false;

    //////////////////////////////////////////
    //переменные, отвечающие за свойства
    public bool canFall = false;
    ///////////////////////////////////////////

    public void getDamage(float damage)
    {
        if (immortalTime <=0)
        {

            hp -=damage;
            if (hp <= 0)
            {
                hp = 0;
                onDeath();
                map.destroyObject(this);
            }
            else
                onGetDamage(damage);
            immortalTime = immortalTimeForHit;
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
        getDamage(5);
        stanTime = 0.7f;      //    fallingTime
        movements.Clear();
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
        if (canFall)   // падение
        {
            if (map.getMapObjects<MapObject>((int)(mapLocation + movements.Peek().point).x, (int)(mapLocation + movements.Peek().point).y, x => x.objectName == "Floor" ||
             x.objectName == "MovingFloor" || ( x.objectName == "BreakableFloor" && (x as BreakableFloor).isReal) ) == null)
            {
                actFall = true;
            }
        }
    }

    public override void updateObject()
    {
        if (immortalTime > 0)        //время бессмертия
            immortalTime -= Time.deltaTime;

        if (stanTime > 0)          // механика оглушения
        {
            stanTime-= Time.deltaTime;
            stopTime = true;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0); // to delet
            if ( stanTime <= 0)
            {
                stopTime = false;
                movements.Clear();
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255); // to delet
                addMovement(new movement(Vector2Int.RoundToInt(lastFloor - new Vector2(position.x,position.y )), false));
            }
        }

        base.updateObject();
    }

    override public void onEndWalk() 
    {
        if (actFall)
        {
            onFall();
            actFall = false;
        }
        if (map.getMapObjects<MapObject>((int)mapLocation.x, (int)mapLocation.y, x => x.objectName == "Floor") !=null)
        {
            lastFloor = position;
        }
    }

    public WalkAndLive(int x, int y): base(x,y) {
        
    }

}
