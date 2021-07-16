using UnityEngine;
public class Event {
    public float time;

    public Event(float t) {
        time = t;
    }
}

public class StateEvent: Event {
    public Object obj;

    public StateEvent(float t, Object o) : base(t) {
        obj = o;
    }
}

public class CollizionEvent: Event {
    public MapObject obj1, obj2;
    public Vector2 orientation;

    public CollizionEvent(float t, MapObject o1, MapObject o2, Vector2 orent): base(t) {
        obj1 = o1;
        obj2 = o2;
        orientation = orent;
    }
}