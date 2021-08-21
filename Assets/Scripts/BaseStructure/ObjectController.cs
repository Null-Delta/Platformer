using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Object obj;
    
    // статичные объекты
    static List<string> unUpdateObjects =  new List<string>{"Wall", "Floor", "Grass"};

    void Start()
    {
        obj.startObject();
        foreach (var item in unUpdateObjects)
        {
            if(obj.objectName == item) 
            {
                this.enabled = false;
                break;
            }
        }
        
        
        if(obj is MapObject) {
            (obj as MapObject).setStartPosition();
            (obj as MapObject).setupOrder();
        }
    }

    void Update()
    {
        obj.updateObject();
        if(obj is MapObject) {
            (obj as MapObject).setupOrder();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name != "Tilemap - lvl 0")
            (obj as MapObject).onCollizion(collision.gameObject.GetComponent<ObjectController>().obj as MapObject, collision);
        else 
            (obj as MapObject).onCollizion(null, collision);
        // В случае удара об тайлмап объект конструкция collision.gameObject возвращает сам уровень Тайлмапа из-за
        // этого невозможно получить ObjectController. Такой случай пока единственный, так что сейчас при ударе со стенкой
        // возвращается null
    }

    void OnTriggerEnter2D(Collider2D collision) {
        (obj as MapObject).onCollizion(collision.gameObject.GetComponent<ObjectController>().obj as MapObject, collision);
    }
   
}
