using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerChel : SmartStalker
{
    public override string objectName => "LazerChel";
    
    int typeAttack = 0;
    int countOfLazer= 0;
    Vector2Int directionOfAttack;
    string nameOfDirection;
    float longerTime;
    List<Vector2Int> lazerPoint;
    bool isFire = false;

    List<GameObject> g ;//to delet

    public override void startObject()
    {
        base.startObject();   

        hp = 20;
        stayDelay = 0.4f;
        foundRange = 9;
        damage = 10;
        attackTime = 1f;
        startOfDamageTime = 0.8f;
        endOfDamageTime = 0.1f;
        rangeOfAttack = 7;
        typeOfWalk=1;
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
        longerTime = 0.8f;
        countOfLazer = 0;
        lazerPoint = new List<Vector2Int>();
        g = new List<GameObject>();
        
        if (typeAttack ==0)
        {
            var tmpVector = target.position - this.position;
            if (tmpVector.x == 0)
                if (tmpVector.y > 0)
                {
                    directionOfAttack = Vector2Int.up;
                    nameOfDirection = "ulazer";
                }
                else
                {
                    directionOfAttack = Vector2Int.down;
                    nameOfDirection = "dlazer";
                }
            else
                if (tmpVector.x > 0)
                {
                    directionOfAttack = Vector2Int.right;
                    nameOfDirection = "rlazer";
                }
                else
                {
                    directionOfAttack = Vector2Int.left;
                    nameOfDirection = "llazer";
                }
        }

    }

    public override void updateObject()
    {
        base.updateObject();
        if (attackRunner > 0) 
        {
            attackRunner -=Time.deltaTime;
            if (attackRunner <= startOfDamageTime && attackRunner >= endOfDamageTime) // нанесение урона
            {
                dealDamage();
            }
            if (attackRunner <=0)  // конец атаки
            {
                isAttack = false;
                isFire = false;
                for (int i = 0; i != g.Count; i++)
                {
                    map.delet(g[i]); // to delet
                }
                foundWay();
            }
        }
    }



    public override void foundWay()
    {
        if (foundTarget)
        {
            float tmpFloat = Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y);
            if (tmpFloat > rangeOfAttack || (tmpFloat <= rangeOfAttack && tmpFloat >= rangeOfAttack-4.5f && Mathf.Abs(target.position.x - position.x)!=0 &&  Mathf.Abs(target.position.y - position.y) !=0) )
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                    return;
                addMovement(new movement(toInt(tmpVector -position), true));
            }
            else if (tmpFloat <= rangeOfAttack && tmpFloat >= rangeOfAttack-4.5f && (Mathf.Abs(target.position.x - position.x)==0 ||  Mathf.Abs(target.position.y - position.y) ==0))
            {
                typeAttack = 0;
                isAttack = true;
                startOfAttack();
            }
            else if ( tmpFloat <= rangeOfAttack-4.5f)
            {
                typeAttack = 1;
                isAttack = true;
                startOfAttack();
            }
        }
    }

    public override void dealDamage()
    {
        if (typeAttack == 0)
        {
            if (!isFire)
            {
                gameObject.GetComponentInChildren<Animator>().Play(nameOfDirection);
                isFire = true;
            }
                
            if (countOfLazer< rangeOfAttack && attackRunner <= longerTime)
            {
                longerTime -=0.02f;
                countOfLazer++;
            }
            for( int ii = 1; ii != countOfLazer+1; ii++)
            {
                List<WalkAndLive> tmpList =map.getMapObjects<WalkAndLive>((int)(mapLocation.x+directionOfAttack.x*ii), (int)(mapLocation.y+directionOfAttack.y*ii), x => x is WalkAndLive);
                if(tmpList != null)
                {
                    for (int i = 0; i != tmpList.Count; i++)
                        if (damagedList.Contains(tmpList[i]) == false)
                        {
                            tmpList[i].getDamage(damage, 0);
                            damagedList.Add(tmpList[i]);
                        }
                }
            }
        }
        else if (typeAttack == 1)
        {
            if (!isFire)
            {
                gameObject.GetComponentInChildren<Animator>().Play("clazer");
                longerTime = 0;
                isFire = true;
            }
            List<WalkAndLive> tmpList =map.getMapObjects<WalkAndLive>(new List<Vector2Int>
                    { 
                        this.mapLocation+Vector2Int.up,
                        this.mapLocation+Vector2Int.up+Vector2Int.right,
                        this.mapLocation+Vector2Int.up+Vector2Int.left,
                        this.mapLocation+Vector2Int.down,
                        this.mapLocation+Vector2Int.down+Vector2Int.right,
                        this.mapLocation+Vector2Int.down+Vector2Int.left,
                        this.mapLocation+Vector2Int.right,
                        this.mapLocation+Vector2Int.left,
                    }, x => x is WalkAndLive);
            if(tmpList != null)
                {
                    for (int i = 0; i != tmpList.Count; i++)
                        if (damagedList.Contains(tmpList[i]) == false)
                        {
                            tmpList[i].getDamage(damage, 0);
                            damagedList.Add(tmpList[i]);
                        }
                }
        }
        
        
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public LazerChel(int x, int y): base(x,y) {
        
    }
}
