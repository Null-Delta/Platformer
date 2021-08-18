using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : UsualStalker
{
    public override string objectName => "Warrior";
    
    

    public override void startObject()
    {
        base.startObject();   

        hp = 50;
        stayDelay = 0.1f;
        foundRange = 7;
        damage = 15;
        attackTime = 0.7f;
        startOfDamageTime = 0.2f;
        endOfDamageTime = 0.0f;
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
        foundWay();
    }

    public Warrior(int x, int y): base(x,y) {
        
    }
}
