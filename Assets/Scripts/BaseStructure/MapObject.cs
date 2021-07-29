using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject: Object {
    override public string objectName => "Null";

    public ObjectOrder order;
    public bool isCollisiable;
    public Vector2 position;
    virtual public void onCollizion(MapObject obj, Collision2D collision) {}
    public override void updateObject(float time) { }
    public override void startObject() { }
    public override void resetObject() { }
    public void setupOrder() {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - (int)order);
    }
    public override void execute(Command command) { }
}