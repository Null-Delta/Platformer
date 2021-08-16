using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : SmartStalker
{
    public override string objectName => "Bull";
    
    Vector2Int runDirection = new Vector2Int();

    public override void startObject()
    {
        base.startObject();   

        hp = 80;
        stayDelay = 0.3f;
        immortalTimeForHit = 0.2f;
        foundRange = 9;
        damage = 5;
        attackTime = 0.9f;
        startOfDamageTime = 0.7f;
        endOfDamageTime = 0.6f;
        rangeOfAttack = 1f;
        typeOfWalk=1;
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
                foundWay();
            }
        }
    }

    public override void foundWay()
    {
        if (foundTarget)
        {
            float deltaX =Mathf.Abs(target.position.x - position.x);
            float deltaY =Mathf.Abs(target.position.y - position.y);
            if (deltaX + deltaY <= rangeOfAttack)
            {
                isAttack = true;
                startOfAttack();
                
            }
            else 
            {
                RaycastHit2D tmpRay = Physics2D.Raycast(position, (new Vector2(target.position.x, target.position.y) - position).normalized, foundRange, 9);

                if (tmpRay.collider !=null && tmpRay.collider.gameObject.layer == 3 && (deltaX == 0 || deltaY == 0))
                {
                    stayDelay = 0.0f;
                    startOfDamageTime = 0.9f;
                    endOfDamageTime = 0.8f;
                    

                    int tmpCount =(int)Mathf.Abs(deltaX+deltaY);
                    deltaX = (target.position.x - position.x);
                    deltaY = (target.position.y - position.y);
                    runDirection = new Vector2Int((int)deltaX/tmpCount, (int)deltaY/tmpCount);
                    for (int i = tmpCount; i != 0; i--)
                    {
                        addMovement(new movement(new Vector2Int((int)deltaX/tmpCount, (int)deltaY/tmpCount), true));
                    }
                }
                else
                {
                    stayDelay = 0.3f;
                    startOfDamageTime = 0.7f;
                    endOfDamageTime = 0.6f;

                    var tmpVector = aStar(position, target.mapLocation);
                    if (tmpVector.x == 0 && tmpVector.y == 0)
                        return;
                    addMovement(new movement(toInt(tmpVector -position), true));
                }
            }
        }
            
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
        if (runDirection.magnitude != 0)
        {
            lockAttackPosition = this.mapLocation + runDirection;
            runDirection = new Vector2Int(0,0);
            return;
        }
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
        if (movements.Count == 0 && runDirection.magnitude != 0)
        {
            isAttack = true;
            startOfAttack();
        }
        else if (movements.Count == 0)
            foundWay();
    }


    public Bull(int x, int y): base(x,y) {
        
    }
}
