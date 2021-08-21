using UnityEngine;
public class Grass : ConnectedObject
{
    public override string objectName => "Grass";

    public override bool isTile => true;
    // public override void startObject()
    // {
    //     base.startObject();
    //     gameObject.transform.position = position;
    //     setupStyle((int)position.x, (int)position.y);

    // }

    public override void execute(Command command)
    {

    }

    public Grass(int x, int y): base(x,y) { 
        order = ObjectOrder.onFloor;
        isCollisiable = false;
        position = new Vector2(x,y);
    }

    public Grass():base(0,0) {
        order = ObjectOrder.onFloor;
        isCollisiable = false;
        position = new Vector2(0,0);
    }
}
