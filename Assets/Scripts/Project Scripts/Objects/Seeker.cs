using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : WalkAndLive
{
    public override string objectName => "Seeker";

    public WalkableObject target;
    public WalkableObject saveTarget;

    // найден ли объект
    public bool foundTarget;
    // дальность для обнаружения
    public int foundRange;

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
        if (foundTarget && saveTarget != target)
        {
            saveTarget = target;
            foundTarget = false;
        }
        if (!foundTarget && target != null)
        {
            
            RaycastHit2D lookRay = Physics2D.Raycast(position, (new Vector2(target.position.x, target.position.y) - position).normalized, foundRange, 9);
            Debug.DrawRay(position, (new Vector2(target.position.x, target.position.y) - position));
            if (lookRay.collider !=null && lookRay.collider.gameObject.layer == 3)
            {
                
                // увидел цель
                foundTarget = true;
                firstLook();
            }
        }
        base.updateObject();
    }

    // метод, вызывающийся при первой встрече
    public virtual void firstLook()
    {
        
    }

    public virtual void getTarget(WalkableObject m)
    {
        target = m;
    }

    public Seeker(int x, int y): base(x,y) {
        
    }
}
