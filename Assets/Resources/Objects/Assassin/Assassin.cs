using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : SmartStalker
{
    public override string objectName => "Assassin";

    Vector2Int attackDirection;

    public override void startObject()
    {
        base.startObject();   

        hp = 30;
        stayDelay = 0.07f;
        foundRange = 12;
        damage = 15;
        attackTime = 0.3f;
        startOfDamageTime = 0.2f;
        endOfDamageTime = 0.15f;
        rangeOfAttack = 1f;
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
                if (attackDirection.x != 0)
                    addMovement(new movement(-attackDirection.x, 0, true));
                else
                    addMovement(new movement(0,-attackDirection.y, true));
                dealDamage();
            }
        }
    }



    public override void startOfAttack()
    {
        base.startOfAttack();
        if (Mathf.Abs(target.position.x - position.x) >= Mathf.Abs(target.position.y - position.y))
        {
            if (target.position.x - position.x > 0)
            {
                lockAttackPosition = this.mapLocation + Vector2Int.right;
                attackDirection =Vector2Int.right;
            }
            else
            {
                lockAttackPosition = this.mapLocation + Vector2Int.left;
                attackDirection =Vector2Int.left;
            }
        }
        else
        {
            if (target.position.y - position.y > 0)
            {
                lockAttackPosition = this.mapLocation + Vector2Int.up;
                attackDirection =Vector2Int.up;
            }
            else
            {
                lockAttackPosition = this.mapLocation + Vector2Int.down;
                attackDirection =Vector2Int.down;
            }
        }
    }

    public override void dealDamage()
    {
        List<WalkAndLive> tmpList;
        if (attackDirection.x != 0)
            tmpList =map.getMapObjects<WalkAndLive>(new List<Vector2Int>{lockAttackPosition, lockAttackPosition+Vector2Int.down,lockAttackPosition+Vector2Int.up }, x => x is WalkAndLive);
        else
            tmpList =map.getMapObjects<WalkAndLive>(new List<Vector2Int>{lockAttackPosition, lockAttackPosition+Vector2Int.right,lockAttackPosition+Vector2Int.left }, x => x is WalkAndLive);
        if(tmpList != null)
        {
            for (int i = 0; i != tmpList.Count; i++)
                if (damagedList.Contains(tmpList[i]) == false)
                {
                    tmpList[i].getDamage(damage);
                    damagedList.Add(tmpList[i]);
                }
        }
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        if (movements.Count == 0)
            foundWay();
    }


    public Assassin(int x, int y): base(x,y) {
        
    }
}
