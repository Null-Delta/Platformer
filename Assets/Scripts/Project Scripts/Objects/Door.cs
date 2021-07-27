using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MapObject
{
    List<Key> keyToOpen = new List<Key>();
    public override string objectName => "Door";

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-3);
        isCollisiable = true;
    }

    public void addKey(Key k)
    {
        keyToOpen.Add(k);
        k.addDoor(this);
    }
    public void eraseKey(Key k)
    {
        keyToOpen.Remove(k);
        if (keyToOpen.Count == 0)
            map.destroyObject(this);
    }

    public Door(int x, int y) {
        position = new Vector2(x,y);
    }

}
