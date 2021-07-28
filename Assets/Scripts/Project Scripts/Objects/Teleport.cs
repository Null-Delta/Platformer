using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : OnPressObject
{
    Vector2 brotherPosition;
    public override string objectName => "Teleport";
    public override void OnPress(Walker who)
    {
        who.position = brotherPosition;

        map.removeMapObject(who.taked_points[0], who);
        who.taked_points = new List<Vector2>{brotherPosition};
        map.insertMapObject(brotherPosition, who);

        if(who is Player) {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TeleportAnimation");
            GameObject prefab2 = Resources.Load<GameObject>("Prefabs/TeleportAnimationIn");
            map.setupGameObject(prefab, new Vector3(position.x,position.y,0));
            map.setupGameObject(prefab2, new Vector3(brotherPosition.x,brotherPosition.y,0));
            (who as Player).gameObject.GetComponent<Animator>().Play("OnTeleport", 0, 0);
            map.executeGroup(events["OnTeleport"]);
        }
        //Instantiate(prefab, new Vector3(position.x,position.y,0), Quaternion.identity);
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 1);
    }

    public Teleport(int x, int y, int bx,int by, List<Command> OnTeleport): base(x,y) 
    {
        brotherPosition = new Vector2(bx,by);
        events = new Dictionary<string, List<Command>>();
        events["OnTeleport"] = OnTeleport;
        position = new Vector2(x,y);
    }
}
