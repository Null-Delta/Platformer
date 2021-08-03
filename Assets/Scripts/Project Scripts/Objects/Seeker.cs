using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : WalkAndLive
{
    public override string objectName => "Seeker";

    public GameObject target;

    // найден ли игрок
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
    }
    public override void updateObject()
    {
        if (target == null)
        {
            target = Camera.main.GetComponent<PlayerControl>().CurrentPlayer.gameObject;
            foundTarget = false;
        }
        else if (!foundTarget)
        {
            
            RaycastHit2D lookRay = Physics2D.Raycast(position, (new Vector2(target.transform.position.x, target.transform.position.y) - position).normalized, foundRange, 9);
            Debug.DrawRay(position, (new Vector2(target.transform.position.x, target.transform.position.y) - position));
            if (lookRay.collider !=null && lookRay.collider.gameObject.layer == 3)
            {
                
                // видит игрока
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

    public Seeker(int x, int y): base(x,y) {
        
    }
}
