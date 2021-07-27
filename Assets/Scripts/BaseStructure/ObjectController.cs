using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Object obj;
    // Start is called before the first frame update
    void Start()
    {
        obj.startObject();
    }

    // Update is called once per frame
    void Update()
    {
        obj.updateObject(Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        (obj as MapObject).onCollizion(collision.gameObject.GetComponent<ObjectController>().obj as MapObject, collision);
    }
}
