using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        List<Object> objects = new List<Object>();

        for(int i = 0; i < 10; i++) {
            for(int j = 0; j < 10; j++) {
                if(i == 0 || i == 9 || j == 0 || j == 9) {
                    objects.Add(new Wall(i,j));
                }

                objects.Add(new Floor(i,j));
            }
        }
        objects.Add(new Bullet(5.3f,5.8f));
        objects.Add(new Bullet(6.3f,5.8f));

        GameObject map = new GameObject();
        map.AddComponent<Map>();
        map.GetComponent<Map>().setupObjects(objects);
        //m.setupObjects(objects);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
