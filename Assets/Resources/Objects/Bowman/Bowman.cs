using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : UsualStalker
{
    public override string objectName => "Bowman";
    
    bool notFire = true;
    Vector2 floatLockAttackPosition;

    public override void startObject()
    {
        base.startObject();   

        hp = 30;
        stayDelay = 0.4f;
        immortalTimeForHit = 0.2f;
        foundRange = 9;
        damage = 10;
        attackTime = 0.7f;
        startOfDamageTime = 0.5f;
        //endOfDamageTime = 0.0f;
        rangeOfAttack = 10;
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
        floatLockAttackPosition = target.position - position;
        floatLockAttackPosition.Normalize();
    }

    public override void updateObject()
    {
        base.updateObject();
        if (attackRunner > 0) 
        {
            attackRunner -=Time.deltaTime;
            if (attackRunner <= startOfDamageTime && notFire) // нанесение урона
            {
                dealDamage();
            }
            if (attackRunner <=0)  // конец атаки
            {
                isAttack = false;
                notFire = true;
                foundWay();
            }
        }
    }

    public override void dealDamage()
    {
        base.dealDamage();
        notFire = false;
        Bullet tmpBullet = new Bullet((floatLockAttackPosition.x*1.5f + position.x), (floatLockAttackPosition.y*1.5f + position.y), floatLockAttackPosition.x, floatLockAttackPosition.y, 8, damage);
        map.setupObject(tmpBullet);
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public Bowman(int x, int y): base(x,y) {
        
    }
}
