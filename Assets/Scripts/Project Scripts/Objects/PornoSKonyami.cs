using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PornoSKonyami : WalkAndLive
{
    public override string objectName => "PornoSKonyami";

    public Player target;

    // найден ли игрок
    public bool foundTarget;
    //
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
            target = Camera.main.GetComponent<PlayerControl>().CurrentPlayer;
            foundTarget = false;
        }
        else if (!foundTarget)
        {
            RaycastHit2D lookRay = Physics2D.Raycast(position, (target.position - position).normalized, foundRange, 9);
            if (lookRay.collider !=null && lookRay.collider.gameObject.layer == 3)
            {
                //Debug.DrawRay(position, (target.position - position));
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

    public PornoSKonyami(int x, int y): base(x,y) {
        
    }
}
