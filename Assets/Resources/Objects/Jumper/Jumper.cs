using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : UsualStalker
{
    public override string objectName => "Jumper";
    
    bool inStartJump = false;
    bool inEndJump = false;
    bool preJump = false;
    float flyingTime;

    public override void startObject()
    {
        base.startObject();   

        hp = 70;
        stayDelay = 0.5f;
        foundRange = 10;
        damage = 15;
        attackTime = 3f;
        startOfDamageTime = 2.5f;
        flyingTime = 2f;
        endOfDamageTime = 1.5f;
        rangeOfAttack = 9;
        canFall = true;
        order = ObjectOrder.wall;
    }

    public override void updateObject()
    {
        base.updateObject();
        if (attackRunner > 0) 
        {
            
            attackRunner -=Time.deltaTime;
            if (!preJump && attackRunner <= startOfDamageTime)
            {
                gameObject.GetComponentInChildren<Animator>().Play("Up",0,0);
                map.removeMapObject(this.mapLocation, this);
                if (targetWalker != null)
                    clearTarget();
                this.gameObject.GetComponentInChildren<Collider2D>().enabled =false;
                inStartJump = true;
                immortal = true;
                preJump = true;
            }     
            else if (inStartJump && attackRunner <= flyingTime) // поиск места приземления
            {
                addMovement(new movement(target.mapLocation - mapLocation, false));
                inStartJump = false;
                inEndJump = true;
            }  
            else if (inEndJump && attackRunner <= endOfDamageTime)
            {
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                //Debug.Log(savePosition-mapLocation);
                //position = savePosition;
                //addMovement(new movement(new Vector2Int((int)(savePosition.x-this.mapLocation.x),(int)(savePosition.y-this.mapLocation.y)), false));
                this.gameObject.GetComponentInChildren<Collider2D>().enabled =true;
                inEndJump = false;
                immortal = false;

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
                        if (!(tmpList[i] is Jumper))
                        {
                            tmpList[i].getDamage(damage);
                        }
                }
                if (map.getMapObjects<MapObject>((int)mapLocation.x, (int)mapLocation.y, x => x.objectName == "Floor" ||
                x.objectName == "MovingFloor" || ( x.objectName == "BreakableFloor" && (x as BreakableFloor).isReal) ) == null)
                    onFall();
                else
                    lastFloor = mapLocation;
            }

            if (attackRunner <=0)  // конец атаки
            {
                preJump = false;
                isAttack = false;
                foundWay();
            }
        }
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
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
