using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : UsualStalker
{
    public override string objectName => "Jumper";
    
    bool inStartJump = false;
    bool inEndJump = false;
    bool preJump = false;
    float flyingTime = 2.1f;
    int count=0;
    float saveY;    
    Vector2 savePosition;

    public override void startObject()
    {
        base.startObject();   

        hp = 70;
        stayDelay = 0.5f;
        foundRange = 10;
        damage = 15;
        attackTime = 3f;
        startOfDamageTime = 2.7f;
        endOfDamageTime = 1.5f;
        rangeOfAttack = 9;
        canFall = false;
        onFloor = false;
        order = ObjectOrder.wall;
    }

    public override void updateObject()
    {
        base.updateObject();
        if (attackRunner > 0) 
        {
            
            attackRunner -=Time.deltaTime;
            if (attackRunner <= startOfDamageTime && flyingTime <= attackRunner)
            {
                if (!preJump)
                {
                    map.removeMapObject(this.mapLocation, this);
                    inStartJump = true;
                    immortal = true;
                    preJump = true;
                    gameObject.GetComponent<Animator>().Play("jump",0,0);
                }

                this.gameObject.GetComponent<Collider2D>().enabled =false;
                //addMovement(new movements(new Vector2Int(0,1), false));
                position+=new Vector2Int(0,1);
                count++;
            }

            else if (inStartJump)
            {
                position = position+(new Vector2(target.mapLocation.x-this.mapLocation.x, target.mapLocation.y - saveY));
                savePosition = target.mapLocation;
                inStartJump = false;
            }

            if (attackRunner <= flyingTime && endOfDamageTime <= attackRunner) // нанесение урона
            {
                //addMovement(new movements(new Vector2Int(0,-1), false));
                if (count >0)
                {
                    position+=new Vector2Int(0,-1);
                    count--;
                }
                gameObject.GetComponent<Animator>().Play("Hit",0,0);
                inEndJump = true;
            }
            else if (inEndJump)
            {
                gameObject.GetComponentInChildren<ParticleSystem>().Play();

                addMovement(new movement(new Vector2Int((int)(savePosition.x-this.mapLocation.x),(int)(savePosition.y-this.mapLocation.y)), false));
                this.gameObject.GetComponent<Collider2D>().enabled =true;
                inEndJump = false;
                immortal = false;
                preJump = false;
                //addMovement(new movements(new Vector2Int(0,-1), false));
                var tmpList =map.getMapObjects<WalkAndLive>(new List<Vector2Int>
                    { 
                        this.mapLocation+Vector2Int.up,
                        this.mapLocation+Vector2Int.up+Vector2Int.right,
                        this.mapLocation+Vector2Int.up+Vector2Int.left,
                        this.mapLocation+Vector2Int.down,
                        this.mapLocation+Vector2Int.down+Vector2Int.right,
                        this.mapLocation+Vector2Int.down+Vector2Int.left,
                        this.mapLocation+Vector2Int.right,
                        this.mapLocation+Vector2Int.left,
                        this.mapLocation
                    }, x => x is WalkAndLive);
                if(tmpList != null)
                {
                    for (int i = 0; i != tmpList.Count; i++)
                        if (tmpList[i] != this)
                        {
                            tmpList[i].getDamage(damage);
                        }
                }
            }

            if (attackRunner <=0)  // конец атаки
            {
                isAttack = false;
                foundWay();
            }
        }
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
        saveY = mapLocation.y;
        count=0;
    }

    public override void dealDamage()
    {
        base.dealDamage();
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public Jumper(int x, int y): base(x,y) {
        
    }
}
