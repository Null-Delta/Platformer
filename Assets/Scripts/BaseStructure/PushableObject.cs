using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject: WalkableObject {

    public bool ignorePushes = false;

    public bool tryPush(Vector2Int to) {
        if(!ignorePushes && canMoveOn(mapLocation + to)) {
            addMovement(new movement(to, true));
            return true;
        }
        return false;
    }

    public PushableObject(int x, int y): base(x,y) {
        
    }
}