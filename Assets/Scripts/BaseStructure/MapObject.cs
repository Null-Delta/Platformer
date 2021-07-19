using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject: Object {
    override public string objectName => "Null";
    public Vector2 position;
    virtual public void onCollizion(MapObject obj, Vector2 orientation) {}
    virtual public bool isCollizion(MapObject obj) {return false;}
    public override void updateObject(float time) { }
    public override StateEvent stateCheck(float time) { return null; }
    public override void startObject() { }
    public override void resetObject() { }
    public override void execute(Command command) { }
}