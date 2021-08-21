using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndLive : WalkableObject, IHealth
{
    public override string objectName => "WalkAndLive";

    public float hp { get;set;}

    public float stunTime = 0;
    public float stunOnDamage=0.2f;
    public bool immortal = false;

    public float deathTime = 0.11f;
    public Vector2 lastFloor;
    bool actFall = false;
    bool isFalling = false;
    

    
    public bool isDeath = false;

    GameObject lookHp;

    //////////////////////////////////////////
    //переменные, отвечающие за свойства
    public bool canFall = false;
    ///////////////////////////////////////////

    public void getDamage(float damage=0, float timeToStan=-1, int typeOfDamage=0)
    {
        if (!immortal)
        {
            if (stunTime <=0)
                if (timeToStan==-1)
                    stunTime = stunOnDamage;
                else
                {
                    stunTime = timeToStan;
                }
            if (!actFall)
            {
                gameObject.GetComponentInChildren<Animator>().Play("getDamage", 1, 0);
            }
            
            if (hp - damage <= 0)
            {
                hp = 0;
                startDeath();
                isDeath = true;
            }
            else
            {
                hp -=damage;
            }
            onGetDamage(damage);

            lookHp.transform.GetChild(0).localScale = new Vector3(hp/100f, lookHp.transform.GetChild(0).localScale.y, lookHp.transform.GetChild(0).localScale.z);
            lookHp.transform.GetChild(2).GetComponentInChildren<UnityEngine.UI.Text>().text = "" + hp;     
        }
    }

    public virtual void startDeath()
    {

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
        lookHp.transform.SetParent(this.gameObject.transform.GetChild(0));   
    }

    public override bool canMoveOn(Vector2Int point)
    {
        return true;
    }

    override public void onStartWalk()
    {
        if (canFall)   // падение
        {
            if (map.getMapObjects<MapObject>((int)(mapLocation + movements[0].point).x, (int)(mapLocation + movements[0].point).y, x => x.objectName == "Floor" ||
            x.objectName == "MovingFloor" || ( x.objectName == "BreakableFloor" && (x as BreakableFloor).isReal) ) == null)
            {
                actFall = true;
            }
        }
    }

    public override void updateObject()
    {
        if(isDeath)
        {
            deathTime-= Time.deltaTime;
            if(deathTime < 0)
            {
                onDeath();
                map.destroyObject(this);
                deathTime = 0.11f;
                isDeath = false;
            }
            
        }
            
        if (stunTime > 0)          // механика оглушения
        {
            stunTime-= Time.deltaTime;
            isIgnoreMoves = true;
            for (int i = 1; i < movements.Count;i++)
            {
                movements.Remove(movements[i]);
            }
            this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.1f,0.1f,0.1f, 0.5f); // to delet
            if (stunTime <= 0)
            {
                isIgnoreMoves = false;
                this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1, 1); // to delet
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
