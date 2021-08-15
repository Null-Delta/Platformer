using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : UsualStalker
{
    public override string objectName => "Runner";
    
    int stepCount = 0;

    public override void startObject()
    {
        base.startObject();   

        hp = 100;
        stayDelay = 1.2f;
        immortalTimeForHit = 0.2f;
        foundRange = 8;
        damage = 1;
        attackTime = 1f;
        startOfDamageTime = 0.5f;
        endOfDamageTime = 0.4f;
    }

    public override void updateObject()
    {
        base.updateObject();
        if (attackRunner > 0) 
        {
            attackRunner -=Time.deltaTime;
            if (attackRunner <= startOfDamageTime && endOfDamageTime <= attackRunner) // нанесение урона
            {
                dealDamage();
            }
            if (attackRunner <=0)  // конец атаки
            {
                isAttack = false;
                attackTime = 1f;
                startOfDamageTime = 0.5f;
                endOfDamageTime = 0.4f;
                damage = 2;
                foundWay();
            }
        }
    }

    public override void foundWay()
    {
        if (foundTarget)
            if (Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y) > rangeOfAttack)
            {
                if (stepCount <30)
                {
                    stepCount++;
                    this.gameObject.transform.localScale += new Vector3(0.01f, 0.01f, 0);//to delet
                }
                if (Mathf.Abs(target.position.x - position.x) >= Mathf.Abs(target.position.y - position.y))
                {
                    if (target.position.x - position.x > 0)
                        if (map.getMapObjects<MapObject>((int)position.x+1, (int)position.y, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(1,0), true));
                        else
                        {
                            if (target.position.y - position.y > 0)
                                addMovement(new movement(new Vector2Int(0,1), true));
                            else if (target.position.y - position.y < 0)
                                addMovement(new movement(new Vector2Int(0,-1), true));
                            else
                            {
                                this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
                                stepCount = 0;
                                stayDelay = 1.2f;
                            }
                        }
                    else
                        if (map.getMapObjects<MapObject>((int)position.x-1, (int)position.y, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(-1,0), true));
                        else
                        {
                            if (target.position.y - position.y > 0)
                                addMovement(new movement(new Vector2Int(0,1), true));
                            else if (target.position.y - position.y < 0)
                                addMovement(new movement(new Vector2Int(0,-1), true));
                            else
                            {
                                this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
                                stepCount = 0;
                                stayDelay = 1.2f;
                            }
                        }
                }
                else
                {
                    if (target.position.y - position.y > 0)
                        if (map.getMapObjects<MapObject>((int)position.x, (int)position.y+1, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(0,1), true));
                        else
                        {
                            if (target.position.x - position.x > 0)
                                addMovement(new movement(new Vector2Int(1,0), true));
                            else if (target.position.x - position.x < 0)
                                addMovement(new movement(new Vector2Int(-1,0), true));
                            else
                            {
                                this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
                                stepCount = 0;
                                stayDelay = 1.2f;
                            }
                        }
                    else
                        if (map.getMapObjects<MapObject>((int)position.x, (int)position.y-1, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(0,-1), true));
                        else
                        {
                            if (target.position.x - position.x > 0)
                                addMovement(new movement(new Vector2Int(1,0), true));
                            else if (target.position.x - position.x < 0)
                                addMovement(new movement(new Vector2Int(-1,0), true));
                            else
                            {
                                this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
                                stepCount = 0;
                                stayDelay = 1.2f;
                            }
                        }
                }
            }
            else
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
                stepCount = 0;
                stayDelay = 1.2f;
                isAttack = true;
                startOfAttack();
            }
    }

    public override bool canMoveOn(Vector2Int point)
    {
        bool tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
                tmpReturn = (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements[0].point));
                if (!tmpReturn)
                {
                    this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
                    stepCount = 0;
                    stayDelay = 1.2f;
                }
                return tmpReturn;
            }
        }

        tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if (!tmpReturn)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);//to delet
            stepCount = 0;
            stayDelay = 1.2f;
        }
        return tmpReturn;
    }



                
                
    public override void startOfAttack()
    {
        base.startOfAttack();
        if (Mathf.Abs(target.position.x - position.x) >= Mathf.Abs(target.position.y - position.y))
        {
            if (target.position.x - position.x > 0)
                lockAttackPosition = this.mapLocation + Vector2Int.right;
            else
                lockAttackPosition = this.mapLocation + Vector2Int.left;
        }
        else
        {
            if (target.position.y - position.y > 0)
                lockAttackPosition = this.mapLocation + Vector2Int.up;
            else
                lockAttackPosition = this.mapLocation + Vector2Int.down;
        }
    }

    public override void dealDamage()
    {
        base.dealDamage();
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        {
            if (stepCount != 30)
                stayDelay = 1.2f/ (stepCount);
            else
                stayDelay = 0f;
            if (stepCount < 11)
            {
                attackTime = 1f / stepCount;
                startOfDamageTime = 0.5f / stepCount;
                endOfDamageTime = 0.4f / stepCount;
            }
            damage = stepCount;
        }
        foundWay();
    }

    public Runner(int x, int y): base(x,y) {
        
    }
}
