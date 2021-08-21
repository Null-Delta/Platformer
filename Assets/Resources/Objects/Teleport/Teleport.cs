using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleport : PressableObject
{
    Vector2Int brotherPosition;
    float sumTime;
    bool isTeleportation = false;
    bool isActivation = false;
    bool isReceptionObject = false;
    bool[] b = new bool[] { true, true };
    float teleportatinTime = 1f;
    float deleyBetweenTeleports = 0.25f;
    float deleyBeforeTeleportation = 0.15f;
    WalkableObject teleportationObj;
    Material disintegrationMaterial;
    Material teleportationMaterial;
    Material originalMaterial;

    public override string objectName => "Teleport";
    public override void OnPressStart(WalkableObject walker)
    {
        if (!isReceptionObject)
        {
            isActivation = true;
            teleportationObj = walker;
            originalMaterial = teleportationObj.gameObject.GetComponent<SpriteRenderer>().material;
            map.executeGroup(events["OnTeleport"]);
        }
        else
        {
            isReceptionObject = false;
        }
    }

    public override void OnPressEnd(WalkableObject walker)
    {
        isActivation = false;
        if (!isTeleportation)
        {
            sumTime = 0;
        }
    }

    public void OnReceptionObject()
    {
        isReceptionObject = true;
    }

    public override void updateObject()
    {
        if (isActivation)
        {
            sumTime += Time.deltaTime;
            if (sumTime > deleyBeforeTeleportation)
            {
                //TODO: Сделать ограничение на перемещение в WalkableObject 
                if (map.getMapObjects<MapObject>(brotherPosition.x, brotherPosition.y, x => x is WalkableObject) == null)
                {
                    //teleportationObj.addMovement(new movement(teleportationObj.mapLocation - brotherPosition, false));

                    teleportationObj.isIgnoreMoves = true;

                    teleportationObj.gameObject.GetComponent<SpriteRenderer>().material = disintegrationMaterial;
                    teleportationMaterial = teleportationObj.gameObject.GetComponent<SpriteRenderer>().material;
                    //Camera.main.GetComponent<PlayerControl>().ControlActive = false;
                    isTeleportation = true;
                    isActivation = false;
                    sumTime = 0;
                }
                else
                {
                    //Ну типа анимация шо ты еблан куды телепортируешь, не видишь занято?
                    sumTime = 0;
                }
            }
        }

        if (isTeleportation)
        {
            sumTime += Time.deltaTime;
            if (sumTime < teleportatinTime)
            {
                float t = (teleportatinTime - sumTime) / teleportatinTime;
                teleportationMaterial.SetFloat("Progress", t);
            }
            else
            {
                if (b[0])
                {
                    teleportationMaterial.SetFloat("Progress", 0);
                    b[0] = false;
                }
                if (sumTime < teleportatinTime + deleyBetweenTeleports)
                {
                    if (b[1])
                    {
                        if (map.getMapObjects<MapObject>(brotherPosition.x, brotherPosition.y, x => x is Teleport) != null)
                        {
                            map.getMapObjects<MapObject>(brotherPosition.x, brotherPosition.y, x => x is Teleport).ForEach(x =>
                            {
                                (x as Teleport).OnReceptionObject();
                            });
                        }
                        teleportationObj.addMovement(new movement(brotherPosition - teleportationObj.mapLocation, false));
                        b[1] = false;
                    }
                }
                else
                {
                    if (sumTime < teleportatinTime * 2 + deleyBetweenTeleports)
                    {
                        float t2 = -((teleportatinTime + deleyBetweenTeleports) - sumTime) / teleportatinTime;
                        teleportationMaterial.SetFloat("Progress", t2);
                    }
                    else
                    {

                        teleportationObj.isIgnoreMoves = false;
                        //Camera.main.GetComponent<PlayerControl>().ControlActive = true;
                        teleportationMaterial.SetFloat("Progress", 1);
                        teleportationObj.gameObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                        b[0] = true; b[1] = true;
                        isTeleportation = false;
                        sumTime = 0;
                        //map.removeMapObject(teleportationObj.position, teleportationObj);
                    }
                }
            }
        }
    }

    public override void startObject()
    {
        base.startObject();
        order = ObjectOrder.underWall;
        gameObject.transform.position = position;
    }

    public Teleport(int x, int y, int bx, int by, List<Command> OnTeleport) : base(x, y)
    {
        brotherPosition = new Vector2Int(bx, by);
        events = new Dictionary<string, List<Command>>();
        events["OnTeleport"] = OnTeleport;

        isCollisiable = false;
        order = ObjectOrder.underWall;

        disintegrationMaterial = Resources.Load<Material>("URP/DisintegrationMaterial");
    }
}
