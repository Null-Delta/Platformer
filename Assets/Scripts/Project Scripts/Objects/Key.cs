using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : OnPressObject
{
    List<Door> doorToOpen = new List<Door>();
    public override string objectName => "Key";

    public void addDoor(Door d)
    {
        doorToOpen.Add(d);
    }

    public override void OnPress(Walker who)
    {
        var doorIterator =  doorToOpen.GetEnumerator();
        while (doorIterator.MoveNext())
        {
            doorIterator.Current.eraseKey(this);
        }
        map.destroyObject(this);
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-3);
    }

    public Key(int x, int y):base(x,y)
    {
        position = new Vector2(x,y);
    }
}
