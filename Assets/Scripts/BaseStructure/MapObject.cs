using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapObject: Object {
    override public string objectName => "Null";

    public virtual bool isTile { get; } = false;

    public TileBase tile {
        get {
            return isTile ? Resources.Load<TileBase>("Objects/" + objectName + "/tile") : null;
        }
    }

    public Vector2 startPosition;

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
            if (objectName == "Jumper")
                Debug.Log(1);
            if(gameObject != null) {
                gameObject.transform.position = value;
            } else {
                startPosition = value;
            }
        }
    }
    virtual public void onCollizion(MapObject obj, Collision2D collision) {}
    virtual public void onCollizion(MapObject obj, Collider2D collision) {}
    public override void updateObject() { }
    public override void startObject() { }
    public override void resetObject() { }
    public void setupOrder() {
        if(gameObject.GetComponent<SpriteRenderer>() != null)
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)order;
        else 
        gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)order;
    }
    public override void execute(Command command) { }

    public MapObject(float x, float y) {
        startPosition = new Vector2(x,y);
    }
    public MapObject() {
    }
}