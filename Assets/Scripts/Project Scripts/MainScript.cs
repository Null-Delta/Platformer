using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    List<Object> objects = new List<Object>();
    void Start()
    {

       

        for(int i = 0; i < 10; i++) {
            for(int j = 0; j < 10; j++) {
                if(i == 0 || i == 9 || j == 0 || j == 9) {
                    objects.Add(new Wall(i,j));
                }

                objects.Add(new Floor(i,j));
            }
        }
        objects.Add(new Bullet(5.3f, 5.8f));
        objects.Add(new Bullet(6.3f, 5.8f));

        SetRect<Wall>(13,13,5,5,0);
        SetRect<Floor>(13,13,5,5,1);
        
        GameObject map = new GameObject();
        map.AddComponent<Map>();
        map.GetComponent<Map>().setupObjects(objects);
        //m.setupObjects(objects);
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
