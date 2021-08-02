using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : ConnectedObject
{
    public override string objectName => "WireEnable";
    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-1);
        setupStyle((int)position.x, (int)position.y);
        order = ObjectOrder.onFloor;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.12f, 0.12f, 0.13f, 1f);
    }


    public override void execute(Command command)
    {
        switch(command.name) {
            case "Enable":
                gameObject.GetComponent<SpriteRenderer>().color = new Color(166f / 255f, 158f / 255f, 154f / 255f, 1f);
            break;
            case "Disable":
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.12f, 0.12f, 0.13f, 1f);
            break;
        }
    }
    public Wire(int x, int y): base(x,y) { }
}
