using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndLive : WalkableObject, Health
{
    public override string objectName => "WalkAndLive";


    public float hp { get;set;}
    public float immortalTimeForHit {get;set;}
    public float immortalTime {get;set;}

    public float savedStayDelay;
    public Vector2 lastFloor;

    public Vector3 savedSize;//to delet

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
        if (this is Player)
            Camera.main.GetComponent<PlayerControl>().enabled = false; 
        savedSize = this.gameObject.transform.localScale;
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
        savedSize = this.gameObject.transform.localScale;  // to delet
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

            this.gameObject.transform.localScale = savedSize; // to delet
        }
    }

    public override void updateObject()
    {
        if (immortalTime > 0)
            immortalTime -= Time.deltaTime;

        if (Camera.main.GetComponent<PlayerControl>().enabled == false)
        {
            this.gameObject.transform.localScale -= new Vector3(0.01f,0.01f,0); // to delet
        }

        base.updateObject();
    }

    override public void onEndWalk() 
    {
        if (canFall)   // падение
        {
            if (map.getMapObjects<MapObject>((int)mapLocation.x, (int)mapLocation.y, x => x.objectName == "Floor" ||
             x.objectName == "MovingFloor" || ( x.objectName == "BreakableFloor" && (x as BreakableFloor).isReal) ) == null)
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
