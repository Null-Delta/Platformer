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

    public override void execute(Command command)
    {
        switch (command.name) {
            case "Open":
                isCollisiable = false;
                gameObject.GetComponent<Animator>().Play("DoorOpen");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-1);
            break;

            case "Close":
                isCollisiable = true;
                gameObject.GetComponent<Animator>().Play("DoorClose");
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-3);
            break;
        }
    }

    public Door(int x, int y) {
        position = new Vector2(x,y);
    }

}
