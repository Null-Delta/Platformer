using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PressableObject
{
    public override string objectName => "Key";
    public override void OnPressStart(WalkableObject walker)
    {
        //Debug.Log("PRESS");
        if(walker is Player) {
            map.executeGroup(events["onSelect"]);
            map.destroyObject(this);
        }
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        order = ObjectOrder.underWall;
    }

    public Key(int x, int y, List<Command> onSelect):base(x,y)
    {
        position = new Vector2(x,y);
        events = new Dictionary<string, List<Command>>();
        events["onSelect"] = onSelect;
    }
}
