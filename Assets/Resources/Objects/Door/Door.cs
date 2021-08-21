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
        isCollisiable = true;
        order = ObjectOrder.wall;
    }

    public override void execute(Command command)
    {
        switch (command.name) {
            case "Open":
                order = ObjectOrder.underWall;
                isCollisiable = false;
                gameObject.GetComponentInChildren<Animator>().Play("DoorOpen");
                gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            break;

            case "Close":
                order = ObjectOrder.wall;
                isCollisiable = true;
                gameObject.GetComponentInChildren<Animator>().Play("DoorClose");
                gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
            break;
        }
    }

    public Door(int x, int y): base(x,y) { }

}
