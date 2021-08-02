using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject: Object {
    override public string objectName => "Null";

    Vector2 startPosition;

    public void setStartPosition() {
        position = startPosition;
    }

    public ObjectOrder order;
    public bool isCollisiable;

    public Vector2 position  {
        get {
            if(gameObject != null) {
                return gameObject.transform.position;
            } else {
                return startPosition;
            }
        }
        set {
            if(gameObject != null) {
                gameObject.transform.position = value;
            } else {
                startPosition = value;
            }
        }
    }

    virtual public void onCollizion(MapObject obj, Collision2D collision) {}
    public override void updateObject() { }
    public override void startObject() { }
    public override void resetObject() { }
    public void setupOrder() {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - (int)order);
    }
    public override void execute(Command command) { }

    public MapObject(float x, float y) {
        startPosition = new Vector2(x,y);
    }
}