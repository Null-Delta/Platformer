using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Reflection;

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

    public UnityEngine.Experimental.Rendering.Universal.Light2D shapeLight;

    Vector3[] points;
    public override string objectName => "Teleport";
    public override void OnPressStart(WalkableObject walker)
    {
        points = new Vector3[5];
        points[0] = new Vector3(1f, 1f, 0);
        points[1] = new Vector3(-1f, 1f, 0);
        points[2] = new Vector3(-1f, -1f, 0);
        points[3] = new Vector3(1f, -1f, 0);
        points[4] = new Vector3(1.5f, 0f, 0);
        
        shapeLight = walker.gameObject.GetComponent<Light2D>();
        foreach(Vector3 i in shapeLight.shapePath)
            Debug.Log(i);
        SetShapePath(shapeLight, points);
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
        void SetFieldValue<T>(object obj, string name, T val) {
            var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(obj, val);
        }
        
        void SetShapePath(Light2D light, Vector3[] path) {
            SetFieldValue<Vector3[]>(light, "m_ShapePath",  path);
        }

    public override void OnPressEnd(WalkableObject walker)
    {
        isActivation = false;
        if(!isTeleportation)
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
            points[4] = new Vector3(1.5f + sumTime*2, 0f, 0);
            SetShapePath(shapeLight, points);
            sumTime += Time.deltaTime;
            if (sumTime > deleyBeforeTeleportation)
            {
                //TODO: Сделать ограничение на перемещение в WalkableObject 
                if(map.getMapObjects<MapObject>(brotherPosition.x, brotherPosition.y, x => x is WalkableObject) == null)
                {
                    map.removeMapObject(teleportationObj.position, teleportationObj);
                    map.insertMapObject(brotherPosition, teleportationObj);
                    teleportationObj.mapLocation = brotherPosition;
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
                points[4] = new Vector3(1.5f + deleyBeforeTeleportation + sumTime*2, 0f, 0);
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
        isCollisiable = false;
        order = ObjectOrder.underWall;
        gameObject.transform.position = position;
        disintegrationMaterial = Resources.Load<Material>("URP/DisintegrationMaterial");
    }

    public Teleport(int x, int y, int bx, int by, List<Command> OnTeleport) : base(x, y)
    {
        brotherPosition = new Vector2Int(bx, by);
        events = new Dictionary<string, List<Command>>();
        events["OnTeleport"] = OnTeleport;
    }
}
