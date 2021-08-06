using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndLive : WalkableObject, Health
{
    public override string objectName => "WalkAndLive";

    public float hp { get;set;}
    public float immortalTimeForHit {get;set;}
    public float immortalTime {get;set;}

    public float stunTime = 0;
    public Vector2 lastFloor;
    bool actFall = false;
    bool isFalling = false;

    GameObject lookHp;

    //////////////////////////////////////////
    //переменные, отвечающие за свойства
    public bool canFall = false;
    ///////////////////////////////////////////

    public void getDamage(float damage)
    {
        if (!actFall)
            gameObject.GetComponent<Animator>().Play("getDamage", 1, 0);
        if (immortalTime <=0)
        {
            //Debug.Log(damage);
            hp -=damage;

            lookHp.SetActive(true);
            lookHp.transform.localScale = new Vector3(hp/50f,lookHp.transform.localScale.y, lookHp.transform.localScale.z );
            
            if (hp <= 0)
            {
                hp = 0;
                onDeath();
                map.destroyObject(this);
            }
            else
            {
                onGetDamage(damage);
            }
                
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
        stunTime = 0.7f;      //    fallingTime
        movements.Clear();
        isFalling = true;
    }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        
    }



    public override void startObject()
    {
        base.startObject();   
        isCollisiable = true;

        lookHp = map.createHpLine(this);
        lookHp.transform.SetParent(this.gameObject.transform);
        lookHp.SetActive(false);
        
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
        if (immortalTime > 0) //время бессмертия
        {
            immortalTime -= Time.deltaTime;
            if (immortalTime <=0)
            {
                lookHp.SetActive(false);
            }
        }        
            
        if (stunTime > 0)          // механика оглушения
        {
            stunTime-= Time.deltaTime;
            stopTime = true;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.1f,0.1f,0.1f, 0.5f); // to delet
            if ( stunTime <= 0)
            {
                stopTime = false;
                movements.Clear();
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 1); // to delet
                if (isFalling)
                {
                    isFalling = false;
                    addMovement(new movement(Vector2Int.RoundToInt(lastFloor - new Vector2(position.x,position.y )), false));
                }
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
