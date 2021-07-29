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
        isCollisiable = true;
        order = ObjectOrder.wall;
    }

    public override void execute(Command command)
    {
        switch (command.name) {
            case "Open":
                order = ObjectOrder.underWall;
                isCollisiable = false;
                gameObject.GetComponent<Animator>().Play("DoorOpen");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            break;

            case "Close":
                order = ObjectOrder.wall;
                isCollisiable = true;
                gameObject.GetComponent<Animator>().Play("DoorClose");
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            break;
        }
    }

    public Door(int x, int y) {
        position = new Vector2(x,y);
    }

}
