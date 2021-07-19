using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject map;
    List<Object> objects = new List<Object>();
    int i = 0;

    float time = 0f;
    void Start()
    {
        map = new GameObject();
        //objects.Add(new Bullet(1.3f, 10.2f));
        //objects.Add(new Bullet(6.3f, 10.2f));
        objects.Add(new Walker(10,10));

        Camera.main.transform.position = new Vector3(15,15, -10);

        SetRect<Wall>(0,0,30,30,0);
        //SetRect<Wall>(5,5,20,20,0);
        //SetRect<Wall>(12,12,6,6,1);
        //objects.Add(new Floor(10,10));
        
        SetRect<Floor>(0,0,30,30,1);

        objects.RemoveAll(x1 => x1.objectName == "Floor" && objects.Find(x => x.objectName == "Wall" && (x is MapObject) && (x as MapObject).position == (x1 as MapObject).position) == null && Random.Range(0,2) == 0);

        objects.RemoveAll(x1 => x1.objectName == "Floor" && objects.Find(x => x.objectName == "Wall" && (x is MapObject) && (x as MapObject).position == (x1 as MapObject).position) != null);
        map.AddComponent<Map>();
        map.GetComponent<Map>().setupObjects(objects);
    }

    void SetRect<T>(int x, int y, int widht, int height, int hollow) where T : StaticMapObject, new()
    {
        for(int i = x; i < x + widht; i++)
            for(int j = y; j < y + height; j++)
            {
                if(hollow == 0)
                    if (!(i == x || j == y || i == x + widht -1 || j == y + height -1)) continue;

                T obj = new T();
                obj.position = new Vector2(i,j);
                objects.Add(obj);
            }
    }
}
