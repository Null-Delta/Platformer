using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    const int width = 256, height = 256;
    List<Object> objects = new List<Object>();
    List<Group> groups = new List<Group>();
    List<MapObject>[,] mapMatrix = new List<MapObject>[width,height];

    public void insertMapObject(Vector2 point, MapObject obj)
    {
        if(point.x < 0 || point.y < 0 || point.x >= width || point.y >= height) return; 
        mapMatrix[(int)point.x, (int)point.y].Add(obj);
    }
    public void removeMapObject(Vector2 point, MapObject obj)
    {
        if(point.x < 0 || point.y < 0 || point.x >= width || point.y >= height) return; 
        mapMatrix[(int)point.x, (int)point.y].Remove(obj);
    }

    public List<T> getMapObjects<T>(int x, int y, Predicate<T> predicate = default(Predicate<T>)) where T: MapObject {
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
            if(obj is MapObject) {
                mapMatrix[(int)(obj as MapObject).position.x,(int)(obj as MapObject).position.y].Add(obj as MapObject);
            }
        }

        // if(obj is MovableMapObject) {
        //     movableObjects.Add(obj as MovableMapObject);
        // }
    }

    public void destroyObject(Object obj) {
        objects.Remove(obj);

        if (obj is Walker) {
            var taked_points_iterator = (obj as Walker).taked_points.GetEnumerator();
            while(taked_points_iterator.MoveNext())
                mapMatrix[(int)taked_points_iterator.Current.x,(int)taked_points_iterator.Current.y].Remove(obj as Walker);
        }
        else if (obj is MapObject) {
            mapMatrix[(int)(obj as MapObject).position.x,(int)(obj as MapObject).position.y].Remove(obj as MapObject);
        }

        Destroy(obj.gameObject);
    }

    public void executeGroup(List<Command> actions) {
        var actionIterator = actions.GetEnumerator();

        while(actionIterator.MoveNext()) {
            var group = groups.Find(x => x.groupID == actionIterator.Current.groupID);
            if(group == null) return;

            var objectsIterator = group.objects.GetEnumerator();
            
            while(objectsIterator.MoveNext()) {
                objectsIterator.Current.execute(actionIterator.Current);
            }
        }

    }
    public void setupObjects(List<Object> objs) {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                mapMatrix[x,y] = new List<MapObject>();
            }
        }

        var objectsIterator = objs.GetEnumerator();

        while(objectsIterator.MoveNext()) {
            setupObject(objectsIterator.Current);
        }
    }

    public void setupObject(Object obj) {
        obj.map = this;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + obj.objectName);

        if(obj is MapObject) {
            obj.gameObject = Instantiate(prefab,(obj as MapObject).position, Quaternion.identity);
        } else {
            obj.gameObject = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
        }
        obj.gameObject.GetComponent<ObjectController>().obj = obj;
        addObject(obj);
    }
}

public class Group {
    public int groupID;
    public List<Object> objects;
}
