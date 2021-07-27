using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : OnPressObject
{
    Vector2 brotherPosition;
    Teleport brother;
    public override string objectName => "Teleport";
    public override void OnPress(Walker who)
    {
        who.position = brotherPosition;

        map.deleteWalkerPoint(who.taked_points[0], who);
        who.taked_points = new List<Vector2>{brotherPosition};
        map.setWalkerPoint(brotherPosition, who);
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-1);

        brother = map.getMapObjects<Teleport>((int)brotherPosition.x, (int)brotherPosition.y, x=> x is Teleport)[0];
    }

    public Teleport(int x, int y, int bx,int by): base(x,y) 
    {
        brotherPosition = new Vector2(bx,by);

        position = new Vector2(x,y);
    }
}
