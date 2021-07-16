using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    virtual public MoveMode mode {
        get {
            return MoveMode.none;
        }
    }
}

public enum MoveMode {
    linear, none
}

public class LinearMove: Move {
    public float dx,dy,speed;

    public override MoveMode mode => MoveMode.linear;

    public LinearMove(float x, float y, float spd) {
        dx = x;
        dy = y;
        speed = spd;
    }
}

public class MovableMapObject: MapObject {
    public float radius;
    public Move move;
}