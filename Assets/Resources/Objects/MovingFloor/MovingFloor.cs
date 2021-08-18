using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : WalkableObject
{

    public WalkableObject movingObject = null;
    public override string objectName => "MovingFloor";

    public Queue<movement> preset;

    public override void startObject()
    {
        //base.startObject();

        isCollisiable = false;
        order = ObjectOrder.floor;
        movements = new List<movement>();

        var enumer = preset.GetEnumerator();
        while(enumer.MoveNext()) {
            movements.Add(enumer.Current);
        }
    }

    public override void onEndWalk(){
        if(movements.Count == 0) {
            var enumer = preset.GetEnumerator();
            while(enumer.MoveNext()) {
                movements.Add(enumer.Current);
            }
        }
    }

    override public bool tryFindTarget() {
        //localTarget = null;
        return false;
    }
    
    // override public void setLocationOnMap() {
    //     Vector2Int move = movements.Peek().point;

    //     map.removeMapObject(mapLocation, this);

    //     //var enumer = movingObject
    //     if(movingObject != null) {
    //         map.removeMapObject(mapLocation, movingObject);
    //     }

    //     mapLocation = move + mapLocation;
    //     map.insertMapObject(mapLocation, this);

    //     if(movingObject != null) {
    //         map.insertMapObject(mapLocation, movingObject);
    //         movingObject.mapLocation = mapLocation;
    //     }
    // }

    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        
    }

    public MovingFloor(int x, int y):base(x,y) { }

    public MovingFloor(int x, int y, List<Vector2Int> ways, float speed = 0.5f):base(x,y) {
        stayDelay = speed;

        preset = new Queue<movement>();
        var enumer = ways.GetEnumerator();
        while(enumer.MoveNext()) {
            movement m;
            m.isAnimate = true;
            m.point = enumer.Current;
            preset.Enqueue(m);
        }
    }
    
}
