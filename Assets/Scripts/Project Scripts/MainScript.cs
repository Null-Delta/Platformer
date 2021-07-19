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
        // objects.Add(new Bullet(5.3f, 5.8f));
        objects.Add(new Bullet(1.3f, 15f));
        objects.Add(new Bullet(11.3f, 15f));
        objects.Add(new Walker(5f, 15f));
        Camera.main.transform.position = new Vector3(15,15, -10);
        SetRect<Wall>(0,0,30,30,0);
        //SetRect<Wall>(5,5,20,20,0);
        //SetRect<Wall>(12,12,6,6,1);
        SetRect<Puddle>(2,2,2,2,1);
         for (int i = 1; i < 29; i++)
            for (int j = 1; j < 29; j++)
                if(Random.Range(0f,1f) < 0.20f) objects.Add(new Puddle(i,j));
        objects.RemoveAll(x=>(x as MapObject).position.y == 5 && (x as MapObject).position.x >= 10 && (x as MapObject).position.x < 20 && (x is Wall));
        SetRect<Floor>(0,0,30,30,1);
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
