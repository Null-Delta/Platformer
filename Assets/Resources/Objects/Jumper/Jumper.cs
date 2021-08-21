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
        canFall = true;
        //onFloor = false;
        order = ObjectOrder.wall +1;
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
                    gameObject.GetComponentInChildren<Animator>().Play("Up",0,0);
                    map.removeMapObject(this.mapLocation, this);
                    if (targetWalker != null)
                        clearTarget();
                    inStartJump = true;
                    immortal = true;

                    preJump = true;
                }
                this.gameObject.GetComponentInChildren<Collider2D>().enabled =false;
                //addMovement(new movements(new Vector2Int(0,1), false));
                //position+=new Vector2Int(0,1);
                //count++;
                

            }
            else if (inStartJump)
            {
                addMovement(new movement(target.mapLocation - mapLocation, false));
                

                // var tmpList = map.getMapObjects<MovingFloor>((int)target.mapLocation.x, (int)target.mapLocation.y, x => x is MovingFloor);

                // if (tmpList!=null)
                // {
                //     position = tmpList[0].position + new Vector2(0, position.y-saveY);
                //     //tryFindTarget(target.mapLocation);
                // }
                // else
                // {
                //     position = new Vector2(Mathf.RoundToInt(position.x),Mathf.RoundToInt(position.y));
                // }
                //position = position+(new Vector2(target.mapLocation.x-this.mapLocation.x, target.mapLocation.y - saveY));
                //addMovement(new movements(target.position.x-this.position.x, target.position.y - saveY , false));
                //savePosition = target.mapLocation;
                inStartJump = false;
                
            }

            if (attackRunner <= flyingTime && endOfDamageTime <= attackRunner) // нанесение урона
            {
                inEndJump = true;
            }
            
            else if (inEndJump)
            {
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                //Debug.Log(savePosition-mapLocation);
                //position = savePosition;
                //addMovement(new movement(new Vector2Int((int)(savePosition.x-this.mapLocation.x),(int)(savePosition.y-this.mapLocation.y)), false));
                this.gameObject.GetComponentInChildren<Collider2D>().enabled =true;

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
                if (map.getMapObjects<MapObject>((int)mapLocation.x, (int)mapLocation.y, x => x.objectName == "Floor" ||
                x.objectName == "MovingFloor" || ( x.objectName == "BreakableFloor" && (x as BreakableFloor).isReal) ) == null)
                    onFall();
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
