using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualStalker : PornoSKonyami
{
    public override string objectName => "UsualStalker";

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.5f;
        order = ObjectOrder.wall;

        hp = 100;
        immortalTimeForHit = 0.5f;
        foundRange = 5;
        canFall = true;
    }

    override public void onStartWalk()
    {
        base.onStartWalk();
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
    }

    public override void updateObject()
    {
        base.updateObject();
    }

    public override void firstLook()
    {
        //Debug.Log("Ээ!");
        addMovement(new movement(new Vector2Int(-1,0), true));
    }

    public UsualStalker(int x, int y): base(x,y) {
        
    }
}
