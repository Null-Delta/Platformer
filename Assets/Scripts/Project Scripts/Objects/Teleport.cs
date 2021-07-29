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
        map.moveMapObject(brotherPosition, who);

        if(who is Player) {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TeleportAnimation");
            GameObject prefab2 = Resources.Load<GameObject>("Prefabs/TeleportAnimationIn");
            map.setupGameObject(prefab, new Vector3(position.x,position.y,0));
            map.setupGameObject(prefab2, new Vector3(brotherPosition.x,brotherPosition.y,0));
            (who as Player).gameObject.GetComponent<Animator>().Play("OnTeleport", 0, 0);
        }

        //Instantiate(prefab, new Vector3(position.x,position.y,0), Quaternion.identity);
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        order = ObjectOrder.underWall;
        gameObject.transform.position = position;

        brother = map.getMapObjects<Teleport>((int)brotherPosition.x, (int)brotherPosition.y, x=> x is Teleport)[0];
    }

    public Teleport(int x, int y, int bx,int by): base(x,y) 
    {
        brotherPosition = new Vector2(bx,by);

        position = new Vector2(x,y);
    }
}
