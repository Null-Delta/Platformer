using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject map;
    List<Object> objects = new List<Object>();
    //int i = 0; Нигде не используется

    //float time = 0f; Нигде не используется
    void Start()
    {
        map = new GameObject();
        //objects.Add(new Bullet(1.3f, 10.2f));
        //objects.Add(new Bullet(6.3f, 10.2f));
        //objects.Add(new Walker(10,10));

        Camera.main.transform.position = new Vector3(15,15, -10);

        SetRect<Wall>(0,0,30,30,0);
        //SetRect<Wall>(5,5,1,1,0);
        //SetRect<Wall>(5,5,20,20,0);
        //SetRect<Wall>(12,12,6,6,1);
        objects.Add(new Player(5,10));
        //objects.Add(new Player(2,10));
        //objects.Add(new Player(3,10));
        //objects.Add(new Player(4,10));
        objects.Add(new Walker(9,10));
        objects.Add(new Teleport(15,15, 15, 10));
        objects.Add(new Teleport(15,10, 15, 15));
        objects.Add(new OnPressObject(25,25));
        objects.Add(new Walker(3,15));
        objects.Add(new Turrel_bullet(3,3, 0.3f, 0.3f));
        objects.Add(new Live_wall(5,25, 0.8f, 0.8f));

        SetRect<Floor>(0,0,30,30,1);

        objects.RemoveAll(x1 => x1.objectName == "Floor" && objects.Find(x => x.objectName == "Wall" && (x is MapObject) && (x as MapObject).position == (x1 as MapObject).position) == null && Random.Range(0,4) == 0);

        objects.RemoveAll(x1 => x1.objectName == "Floor" && objects.Find(x => x.objectName == "Wall" && (x is MapObject) && (x as MapObject).position == (x1 as MapObject).position) != null);
        
        //SetRect<Floor>(10,10,1,4,0);

        
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
