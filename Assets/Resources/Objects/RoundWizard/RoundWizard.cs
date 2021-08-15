using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundWizard : SmartStalker
{
    public override string objectName => "RoundWizard";
    
    bool notFire = true;
    Vector2 floatLockAttackPosition;

    public override void startObject()
    {
        base.startObject();   

        hp = 80;
        stayDelay = 0.4f;
        immortalTimeForHit = 0.2f;
        foundRange = 9;
        damage = 10;
        attackTime = 0.15f;
        startOfDamageTime = 0.14f;
        //endOfDamageTime = 0.0f;
        rangeOfAttack = 9;
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
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
                
                foundWay();
            }
        }
    }

    public override void foundWay()
    {
        if (foundTarget)
        {
            float tmpFloat = Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y);
            if ((tmpFloat >= rangeOfAttack || !notFire) && tmpFloat > rangeOfAttack/3f)
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                    return;
                addMovement(new movement(toInt(tmpVector -position), true));
                notFire = true;
            }
            else if ( tmpFloat <= rangeOfAttack/3f)
            {
                notFire = true;
                isAttack = true;
                startOfAttack();
            }
            else
            {
                isAttack = true;
                startOfAttack();
            }
        }
    }

    public override void dealDamage()
    {
        base.dealDamage();
        notFire = false;
        RoundBullet tmpBullet = new RoundBullet(this, 5, damage);
        map.setupObject(tmpBullet);
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public RoundWizard(int x, int y): base(x,y) {
        
    }
}
