using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWizard : SmartStalker
{
    public override string objectName => "EarthWizard";
    
    int notFire = 0;
    float cooldownTime = 0;
    float timeToBum = 0.4f;

    public override void startObject()
    {
        base.startObject();   

        hp = 100;
        stayDelay = 0.5f;
        immortalTimeForHit = 0.2f;
        foundRange = 10;
        damage = 10;
        attackTime = 4.0f;
        startOfDamageTime = 2f;
        endOfDamageTime = 0.1f;
        rangeOfAttack = 8;
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
    }

    public override void updateObject()
    {
        base.updateObject();
        if (cooldownTime >0)
            cooldownTime -=Time.deltaTime;
        if (attackRunner > 0) 
        {
            attackRunner -=Time.deltaTime;
            if (foundTarget && Mathf.Abs(target.position.x - position.x) > rangeOfAttack && notFire == 0)
            {
                attackRunner = 0f;
                notFire = 2;
            }

            if (attackRunner <= startOfDamageTime && attackRunner >= endOfDamageTime && notFire == 0) // нанесение урона
            {
                dealDamage();
            }
            if (attackRunner <= startOfDamageTime/2 && attackRunner >= endOfDamageTime && notFire == 1) // нанесение урона
            {
                dealDamage();
            }

            if (attackRunner <=0)  // конец атаки
            {
                if (notFire == 2)
                    cooldownTime = 4;
                isAttack = false;
                notFire = 0;
                foundWay();
            }
        }
    }

    public override void dealDamage()
    {
        if (notFire == 0)
        {
            lockAttackPosition = target.mapLocation;
            notFire = 1;
            var tmpList =map.getMapObjects<WalkAndLive>((int)lockAttackPosition.x+1, (int)lockAttackPosition.y, x => x is WalkAndLive);
            if(tmpList == null)
            {
                EarthBlock b1 = new EarthBlock((int)lockAttackPosition.x+1, (int)lockAttackPosition.y, startOfDamageTime / 2f + timeToBum +0.1f);
                map.setupObject(b1);
            }
            tmpList =map.getMapObjects<WalkAndLive>((int)lockAttackPosition.x-1, (int)lockAttackPosition.y, x => x is WalkAndLive);
            if(tmpList == null)
            {
                EarthBlock b2 = new EarthBlock((int)lockAttackPosition.x-1, (int)lockAttackPosition.y, startOfDamageTime / 2f +timeToBum+0.1f);
                map.setupObject(b2);
            }

            tmpList =map.getMapObjects<WalkAndLive>((int)lockAttackPosition.x, (int)lockAttackPosition.y+1, x => x is WalkAndLive);
            if(tmpList == null)
            {
                EarthBlock b3 = new EarthBlock((int)lockAttackPosition.x, (int)lockAttackPosition.y+1, startOfDamageTime / 2f +timeToBum+0.1f);
                map.setupObject(b3);
            }
            tmpList =map.getMapObjects<WalkAndLive>((int)lockAttackPosition.x, (int)lockAttackPosition.y-1, x => x is WalkAndLive);
            if(tmpList == null)
            {
                EarthBlock b4 = new EarthBlock((int)lockAttackPosition.x, (int)lockAttackPosition.y-1, startOfDamageTime / 2f +timeToBum+0.1f);
                map.setupObject(b4);
            }
            
            
        }
        else if (notFire == 1)
        {
            notFire = 2;
            Mina m = new Mina(lockAttackPosition.x, lockAttackPosition.y, timeToBum, damage);  
            map.setupObject(m);
        }
        
    }

    public override void foundWay()
    {
        if (foundTarget)
            if (Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y) > rangeOfAttack)
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                    return;
                addMovement(new movement(toInt(tmpVector -position), true));
            }
            else if ( Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y) < rangeOfAttack/2f)
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                    return;
                addMovement(new movement(toInt(-tmpVector +position), true));
            }
            else if (cooldownTime <=0)
            {
                isAttack = true;
                startOfAttack();
            }
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public EarthWizard(int x, int y): base(x,y) {
        
    }
}
