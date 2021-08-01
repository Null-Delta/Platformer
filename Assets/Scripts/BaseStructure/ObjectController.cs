using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Object obj;
    bool isUpdatableObject = true;
    // Start is called before the first frame update
    void Start()
    {
        obj.startObject();
        if(obj is Wall || obj is Floor) isUpdatableObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isUpdatableObject) {
            obj.updateObject(Time.deltaTime);
            if(obj is MapObject) {
                (obj as MapObject).setupOrder();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        (obj as MapObject).onCollizion(collision.gameObject.GetComponent<ObjectController>().obj as MapObject, collision);
    }
}
