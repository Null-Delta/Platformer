using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : WalkAndLive
{
    public override string objectName => "Seeker";

    public WalkableObject target;
    public WalkableObject saveTarget;

    public bool isAttack = false;
    public float rangeOfAttack = 1;
    public float attackTime = 0.5f;
    public float startOfDamageTime = 0.2f;
    public float endOfDamageTime = 0.0f;
    public float attackRunner;
    public float damage = 5;
    public Vector2Int lockAttackPosition;
    public List<WalkAndLive> damagedList;

    // найден ли объект
    public bool foundTarget;
    // дальность для обнаружения
    public int foundRange;
    float deathCooldown = 0;

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.0f;
        order = ObjectOrder.wall;
        
        hp = 100;
        immortalTimeForHit = 0.5f;
        foundRange = 5;
        isCollisiable = true;
    }
    public override void updateObject()
    {
        if (target != null && (target as WalkAndLive).isDeath)
        {
            target = null;
            foundTarget = false;
            deathCooldown = 3.3f;
        }
        if (deathCooldown >0)
            deathCooldown-= Time.deltaTime;
        if (deathCooldown <= 0 && target == null)
        {
            getTarget(null);
        }
        if (foundTarget && saveTarget != target)
        {
            saveTarget = target;
            foundTarget = false;
        }
        if (movements.Count == 0 && foundTarget && !isAttack)
        {
            foundTarget = false;
        }

        if (!foundTarget && target != null)
        {
            
            RaycastHit2D lookRay = Physics2D.Raycast(position, (new Vector2(target.position.x, target.position.y) - position).normalized, foundRange, 9);
            //Debug.DrawRay(position, (new Vector2(target.position.x, target.position.y) - position));
            if (lookRay.collider !=null && lookRay.collider.gameObject.layer == 3)
            {
                
                // увидел цель
                foundTarget = true;
                firstLook();
            }
        }

        ////
        base.updateObject();
        ////
    }

    // метод, вызывающийся при первой встрече
    public virtual void firstLook()
    {
        
    }

    public virtual void foundWay()
    {

    }


    public virtual void getTarget(MapObject m)
    {
        saveTarget =Camera.main.GetComponent<PlayerControl>().CurrentPlayer;
        target = Camera.main.GetComponent<PlayerControl>().CurrentPlayer;
    }





    public virtual void startOfAttack()
    {
        attackRunner = attackTime;
        damagedList = new List<WalkAndLive>();
    }

    public virtual void dealDamage()
    {
        var tmpList =map.getMapObjects<WalkAndLive>((int)lockAttackPosition.x, (int)lockAttackPosition.y, x => x is WalkAndLive);
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



    public Seeker(int x, int y): base(x,y) {
        
    }
}
