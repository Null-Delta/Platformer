using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : OnPressObject
{
    Vector2 brotherPosition;
    float sum_time;
    bool isTeleportation = false;
    bool[] b = new bool[]{true, true};
    float teleportatinTime = 1;
    Walker teleportationObj;
    public override string objectName => "Teleport";
    public override void OnPress(Walker who)
    {

        isTeleportation = true;
        teleportationObj = who;

        if(who is Player) {
            // GameObject prefab = Resources.Load<GameObject>("Prefabs/TeleportAnimation");
            // GameObject prefab2 = Resources.Load<GameObject>("Prefabs/TeleportAnimationIn");
            // map.setupGameObject(prefab, new Vector3(position.x,position.y,0));
            // map.setupGameObject(prefab2, new Vector3(brotherPosition.x,brotherPosition.y,0));
            //(who as Player).gameObject.GetComponent<Animator>().Play("OnTeleport", 0, 0);
            map.executeGroup(events["OnTeleport"]);
        }

        //Instantiate(prefab, new Vector3(position.x,position.y,0), Quaternion.identity);
    }

    public override void updateObject(float time) 
    {
        //
        if(isTeleportation)
        {
            sum_time += time;
            if(sum_time < teleportatinTime)
            {
                float t = (teleportatinTime - sum_time)/teleportatinTime;
                teleportationObj.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("Progress", t);
            }
            else
            {
                if(b[0])
                {
                    teleportationObj.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("Progress", 0);
                    b[0] = false;
                }
                if(sum_time < teleportatinTime + 0.25f)
                {
                    if(b[1])
                    {
                        map.moveMapObject(brotherPosition, teleportationObj);
                        b[1] = false;
                    }
                }
                else
                {
                    if(sum_time < teleportatinTime*2 + 0.25f)
                    {
                        float t2 = -((teleportatinTime+ 0.25f) - sum_time)/teleportatinTime;
                        teleportationObj.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("Progress", t2);
                    }
                    else
                    {
                        teleportationObj.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("Progress", 1);
                        b[0] = true; b[1] = true;
                        isTeleportation = false;
                        sum_time = 0;
                    }
                }   
            }
        }

    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        order = ObjectOrder.underWall;
        gameObject.transform.position = position;
    }

    public Teleport(int x, int y, int bx,int by, List<Command> OnTeleport): base(x,y) 
    {
        brotherPosition = new Vector2(bx,by);
        events = new Dictionary<string, List<Command>>();
        events["OnTeleport"] = OnTeleport;
        position = new Vector2(x,y);
    }
}
