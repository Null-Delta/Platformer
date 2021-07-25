using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    const int width = 256, height = 256;
    List<Object> objects = new List<Object>();
    List<MovableMapObject> movableObjects = new List<MovableMapObject>();
    List<Group> groups = new List<Group>();
    List<StaticMapObject>[,] mapMatrix = new List<StaticMapObject>[width,height];
    int error_catcher; //err
    Walker[,] mapWalkers = new Walker[width,height];
    List<Object> to_spawn_objects = new List<Object>();

    // public void RefreshAround(int x, int y)
    // {
    //     float r = 1.5f;

    //     //Обработка всех объектов вокруг.
    //     for (float i = 0; i < 2 * 3.14f; i+= 2*3.14f/8f)
    //     {
    //         int dx = x + (int)(Mathf.Cos(i)*r), dy = y + (int)(Mathf.Sin(i)*r);

    //         if (getMapObjects<ConnectedObject>(dx,dy) != null)
    //         {
    //             foreach(ConnectedObject obj in getMapObjects<ConnectedObject>(dx,dy))
    //             {
    //                 int weight = (int)(obj.gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 17);
    //                 int height = (int)(obj.gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 17);
    //                 obj.setupStyle(dx, dy, weight, height);
    //             }
    //         }
    //     }
    // }

    public void spawn_object(Object obj)
    {
        to_spawn_objects.Add(obj);
    }

    public bool checkWalkerPoint(Vector2 point)
    {
        if((int)point.x < 0 || (int)point.y < 0 || (int)point.x >= width || (int)point.y >= height) return false;
        return mapWalkers[(int)point.x, (int)point.y] !=null;
    }
    public void setWalkerPoint(Vector2 point, Walker obj)
    {
        mapWalkers[(int)point.x, (int)point.y] = obj;
    }
    public void deleteWalkerPoint(Vector2 point)
    {
        mapWalkers[(int)point.x, (int)point.y] = null;
    }

    public List<T> getMapObjects<T>(int x, int y, Predicate<T> predicate = default(Predicate<T>)) where T: StaticMapObject {
        if(x < 0 || y < 0 || x >= width || y >= height) return null; 
        
        List<T> list = new List<T>();

        var iter = mapMatrix[x,y].GetEnumerator();
        while(iter.MoveNext()) {
            if(iter.Current != null && iter.Current is T && predicate(iter.Current as T)) {
                list.Add(iter.Current as T);
            }
        }

        if(list.Count == 0) return null;

        return list;
    }

    public void addObject(Object obj) {
        objects.Add(obj);
        
        if(obj is MapObject) {
            if(obj is StaticMapObject) {
                mapMatrix[(int)(obj as StaticMapObject).position.x,(int)(obj as StaticMapObject).position.y].RemoveAll(x => x.objectName == obj.objectName);
                mapMatrix[(int)(obj as StaticMapObject).position.x,(int)(obj as StaticMapObject).position.y].Add(obj as StaticMapObject);
            }
            if(obj is Walker) {

                mapWalkers[(int)(obj as Walker).position.x,(int)(obj as Walker).position.y] = (obj as Walker);
            }
        }

        if(obj is MovableMapObject) {
            movableObjects.Add(obj as MovableMapObject);
        }
    }

    public void deleteObject(Object obj) {
        objects.Remove(obj);

        if(obj is StaticMapObject) {
            mapMatrix[(int)(obj as StaticMapObject).position.x,(int)(obj as StaticMapObject).position.y].Remove(obj as StaticMapObject);
        }
        else if(obj is Walker) {
            var taked_points_iterator = (obj as Walker).taked_points.GetEnumerator();
            while(taked_points_iterator.MoveNext())
                mapWalkers[(int)taked_points_iterator.Current.x, (int)taked_points_iterator.Current.y] = null;
        }

        if(obj is MovableMapObject) {
            movableObjects.Remove(obj as MovableMapObject);
        }
        Destroy(obj.gameObject);
    }

    public void executeGroup(List<Action> actions) {
        var actionIterator = actions.GetEnumerator();

        while(actionIterator.MoveNext()) {
            var group = groups.Find(x => x.groupID == actionIterator.Current.groupID);
            if(group == null) return;

            var objectsIterator = group.objects.GetEnumerator();
            while(objectsIterator.MoveNext()) {
                var commandIterator = actionIterator.Current.commands.GetEnumerator();
                while(commandIterator.MoveNext()) {
                    objectsIterator.Current.execute(commandIterator.Current);
                }
            }
        }

    }
    void Start()
    {
        var objectsIterator = objects.GetEnumerator();
        while(objectsIterator.MoveNext()) {
            objectsIterator.Current.startObject();
        }
    }
    public void setupObjects(List<Object> objs) {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                mapMatrix[x,y] = new List<StaticMapObject>();
            }
        }

        var objectsIterator = objs.GetEnumerator();

        while(objectsIterator.MoveNext()) {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + objectsIterator.Current.objectName);

            objectsIterator.Current.gameObject = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
            objectsIterator.Current.map = this;
            addObject(objectsIterator.Current);
        }
    }

    public void setupObject(Object obj) {

        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + obj.objectName);

        obj.gameObject = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
        obj.map = this;
        addObject(obj);
    
    }

    CollizionEvent collizionTime(MovableMapObject m1, MovableMapObject m2) {
        if(m1 == m2 || !m1.isCollizion(m2) || !m2.isCollizion(m1)) {
            return null;
        }

        if (m1.move.mode == MoveMode.linear && m2.move.mode == MoveMode.linear) {
            var line1 = m1.move as LinearMove;
            var line2 = m2.move as LinearMove;
            float A = line1.dx * line1.speed - line2.dx * line2.speed;
            float B = m1.position.x - m2.position.x;
            float C = line1.dy * line1.speed - line2.dy * line2.speed;
            float D = m1.position.y - m2.position.y;
            float E =  Mathf.Pow((m1.radius + m2.radius), 2);

            float a = Mathf.Pow(A,2) + Mathf.Pow(C,2);
            float b = (2 * A * B + 2 * C * D);
            float c = Mathf.Pow(B, 2) + Mathf.Pow(D, 2) - E;

            float d = b * b - 4 * a * c;
            
            if(d < 0 || a == 0) {
                return null;
            } else {
                float x1 = (-b + Mathf.Sqrt(d)) / (2 * a);
                float x2 = (-b - Mathf.Sqrt(d)) / (2 * a);
                float firstTime = Mathf.Min(x1, x2);

                return firstTime >= 0 ? 
                    new CollizionEvent(
                        firstTime, 
                        m1 as MapObject, 
                        m2 as MapObject, 
                        new Vector2(0,0)) : null;
            }
        }
        return null;
    }
    
    CollizionEvent mapCollizionTime(MovableMapObject m, float time) {
        if(time == 0) return null;

        var times = new List<(float, int, int, int)>();
        var move = (m.move as LinearMove);
        
        int startX = move.dx > 0 ? (int)Mathf.Ceil(m.position.x) : (int)Mathf.Floor(m.position.x);
        int endX = move.dx > 0 ? (int)Mathf.Ceil(m.position.x + time * move.dx * move.speed) : (int)Mathf.Floor(m.position.x + time * move.dx * move.speed);
        
        int startY = move.dy > 0 ? (int)Mathf.Ceil(m.position.y) : (int)Mathf.Floor(m.position.y);
        int endY = move.dy > 0 ? (int)Mathf.Ceil(m.position.y + time * move.dy * move.speed) : (int)Mathf.Floor(m.position.y + time * move.dy * move.speed);
        
        int minx = Mathf.Min(startX, endX);
        int maxx = Mathf.Max(startX, endX);

        int miny = Mathf.Min(startY, endY);
        int maxy = Mathf.Max(startY, endY);

        if(move.dx != 0) {
            for(int x = minx; x <= maxx; x++) {
                float t = ((float)x - m.position.x) / (move.dx * move.speed);
                if(move.dy != 0 && Math.Floor(Math.Round((double)(m.position.y + t * move.dy * move.speed), 4)) == Math.Round((double)(m.position.y + t * move.dy * move.speed), 4)) {
                    times.Add((t, -1, -1, -3));
                } else {
                    times.Add((t, -1, -1, -1));
                }
            }
        }

        if(move.dy != 0) {  
            for(int y = miny; y <= maxy; y++) {
                float t = ((float)y - m.position.y) / (move.dy * move.speed);
                if(move.dx != 0 && Math.Floor(Math.Round((double)(m.position.x + t * move.dx * move.speed), 4)) == Math.Round((double)(m.position.x + t * move.dx * move.speed), 4)) {
                    times.Add((t, -1, -1, -3));
                } else {
                    times.Add((t, -1, -1, -2));
                }
            }
        }

        times.Sort();

        for(int i = 0; i < times.Count; i++) {
            float xPos = m.position.x + times[i].Item1 * move.dx * move.speed;
            float yPos = m.position.y + times[i].Item1 * move.dy * move.speed;
            int xOffset = move.dx > 0 ? 1 : 0;
            int yOffset = move.dy > 0 ? 1 : 0;

            if(times[i].Item4 == -1) {
                if(yPos == (int)yPos) {
                    if(move.dy != 0) {
                        times[i] = (times[i].Item1, (int)xPos + xOffset,  (int)yPos + yOffset, -1);
                    } else {
                        if(getMapObjects<StaticMapObject>((int)xPos + xOffset,  (int)yPos + yOffset, x => x.isDecoration == false) != null && getMapObjects<StaticMapObject>((int)xPos + xOffset,  (int)yPos + yOffset + 1, x => x.isDecoration == false) != null) {
                            times[i] = (times[i].Item1, (int)xPos + xOffset,  (int)yPos + yOffset, -1);
                        } else {
                            times[i] = (times[i].Item1, -3, -3, -1);
                        }
                    }
                } else {
                    times[i] = (times[i].Item1, (int)xPos + xOffset, (int)yPos + 1, -1);
                }
            } else if(times[i].Item4 == -2) {
                if(xPos == (int)xPos) {
                    if(move.dx != 0) {
                        times[i] = (times[i].Item1, (int)xPos + xOffset,  (int)yPos + yOffset, -2);
                    } else {
                        if(getMapObjects<StaticMapObject>((int)xPos + xOffset,  (int)yPos + yOffset, x => x.isDecoration == false) != null && getMapObjects<StaticMapObject>((int)xPos + xOffset + 1,  (int)yPos + yOffset, x => x.isDecoration == false) != null) {
                            times[i] = (times[i].Item1, (int)xPos + xOffset,  (int)yPos + yOffset, -2);
                        } else {
                            times[i] = (times[i].Item1, -3, -3, -2);
                        }
                    }
                } else {
                    times[i] = (times[i].Item1, (int)xPos + 1, (int)yPos + yOffset, -2);
                }
            } else if(times[i].Item4 == -3) {
                times[i] = (times[i].Item1, (int)xPos + xOffset, (int)yPos + yOffset, -3);
            }

            if(getMapObjects<StaticMapObject>(times[i].Item2,times[i].Item3, x => x.isDecoration == false) != null && times[i].Item1 >= 0) {
                
                Vector2 orientation = new Vector2(0,0);

                if(times[i].Item4 == -1) {
                    orientation = move.dx > 0 ? orientation += new Vector2(-1,0) : orientation += new Vector2(1,0);
                } else if(times[i].Item4 == -2){
                    orientation = move.dy > 0 ? orientation += new Vector2(0,-1) : orientation += new Vector2(0,1);
                } else {
                    if(move.dx > 0) {
                        orientation = move.dy > 0 ? orientation += new Vector2(-1,-1) : orientation += new Vector2(-1,1);
                    } else {
                        orientation = move.dy > 0 ? orientation += new Vector2(1,-1) : orientation += new Vector2(1,1);
                    }
                }

                return new CollizionEvent(
                    times[i].Item1,
                    m as MapObject,
                    getMapObjects<StaticMapObject>(times[i].Item2,times[i].Item3, x => x.isDecoration == false)[0],
                    orientation);
            }
        }

        return null;
    }

    void Update()
    {
        error_catcher = 0;//err
        var delta = Time.deltaTime;

        while(delta > 0) {

            for (int ind = 0; ind !=to_spawn_objects.Count; ind++ )
            {
                setupObject(to_spawn_objects[ind]);
            }
            to_spawn_objects.Clear();




            List<Event> minEvents = new List<Event>();
            minEvents.Add(new Event(delta));

            var objectsIterator = objects.GetEnumerator();

            while(objectsIterator.MoveNext()) {
                StateEvent newEvent = objectsIterator.Current.stateCheck(delta);
                if(newEvent != null) {
                    if(newEvent.time == minEvents[0].time) {
                        minEvents.Add(newEvent);
                    } else if(newEvent.time < minEvents[0].time) {
                        minEvents.RemoveAll(x => true);
                        minEvents.Add(newEvent);
                    }
                }
            }

            var moveObjectsIterator = movableObjects.GetEnumerator();

            while(moveObjectsIterator.MoveNext()) {
                CollizionEvent newEvent = mapCollizionTime(moveObjectsIterator.Current, delta);
                if(newEvent != null) {
                    if(newEvent.time == minEvents[0].time) {
                        minEvents.Add(newEvent);
                    } else if(newEvent.time < minEvents[0].time) {
                        minEvents.RemoveAll(x => true);
                        minEvents.Add(newEvent);
                    }
                }
            }
            
            moveObjectsIterator = movableObjects.GetEnumerator();

            while(moveObjectsIterator.MoveNext()) {
                var iter2 = moveObjectsIterator;

                while(iter2.MoveNext()) {
                    if(iter2.Current != moveObjectsIterator.Current && moveObjectsIterator.Current.isCollizion(iter2.Current as MapObject)) {
                        var t = collizionTime(moveObjectsIterator.Current, iter2.Current);
                        if(t != null) {
                            if(t.time == minEvents[0].time) {
                                minEvents.Add(t);
                            } else if (t.time < minEvents[0].time) {
                                minEvents.RemoveAll(x => true);
                                minEvents.Add(t);
                            }
                        }
                    }
                }
            }
            
            objectsIterator = objects.GetEnumerator();
            while(objectsIterator.MoveNext()) {
                objectsIterator.Current.updateObject(minEvents[0].time);
            }
            objectsIterator.Dispose();
            

            var eventIterator = minEvents.GetEnumerator();

            while(eventIterator.MoveNext()) {
                if(eventIterator.Current is CollizionEvent) {
                        ((eventIterator.Current as CollizionEvent).obj1).onCollizion((eventIterator.Current as CollizionEvent).obj2, (eventIterator.Current as CollizionEvent).orientation);
                        ((eventIterator.Current as CollizionEvent).obj2).onCollizion((eventIterator.Current as CollizionEvent).obj1, (eventIterator.Current as CollizionEvent).orientation);
                } else if (eventIterator.Current is StateEvent) {

                }
                
            }




            delta -= minEvents[0].time;
            
            error_catcher++; //err
            if (error_catcher >= 15) //err
            {
                var heart_of_game = GameObject.Find("New Game Object");
                Destroy(heart_of_game);
                print("STOP");
            }
        }
        if (error_catcher >= 6) //err
        {
            print("WARNING");
        }
        
        //TODO: player move input
    
    }
}

public class Group {
    public int groupID;
    public List<Object> objects;
}
