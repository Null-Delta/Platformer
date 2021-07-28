using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : OnPressObject
{
    public override string objectName => "Key";
    public override void OnPress(Walker who)
    {
        if(who is Player) {
            map.executeGroup(events["onSelect"]);
            map.destroyObject(this);
        }
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-3);
    }

    public Key(int x, int y, List<Command> onSelect):base(x,y)
    {
        position = new Vector2(x,y);
        events = new Dictionary<string, List<Command>>();
        events["onSelect"] = onSelect;
    }
}
