using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Texture2D texture;
    Sprite s;

    // public Tilemap tilemap;
    // public Tilemap tilemap1;
    // public Tilemap tilemap2;
    // public Tilemap tilemap3;

    public List<List<Tilemap>> tilemaps;

    const int width = 256, height = 256;
    List<Object> objects = new List<Object>();
    Dictionary<int, List<Object>> groups = new Dictionary<int, List<Object>>();
    List<MapObject>[,] mapMatrix = new List<MapObject>[width,height];
    Image preview;


    public GameObject createHpLine(MapObject obj)
    {
        return Instantiate(Resources.Load<GameObject>("Other/hpLine"),obj.position, Quaternion.identity);
    }

    public GameObject createLazerPath(Vector2Int p) // to delet
    {
        return Instantiate(Resources.Load<GameObject>("Other/redLine"),new Vector3(p.x,p.y,0), Quaternion.identity);
    }
    public void delet(GameObject g)
    {
        Destroy(g);
    }


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
            if (iter.Current != null && (iter.Current is T) && predicate((iter.Current as T))) {
                list.Add((iter.Current as T));
            }
        }

        if(list.Count == 0) return null;

        return list;
    }
    public List<T> getMapObjects<T>(List<Vector2Int> points, Predicate<T> predicate = default(Predicate<T>)) where T: MapObject {
        var iterPoints = points.GetEnumerator();
        List<T> list = new List<T>();
        while (iterPoints.MoveNext())
        {
            if(iterPoints.Current.x < 0 || iterPoints.Current.y < 0 || iterPoints.Current.x >= width || iterPoints.Current.y >= height) continue; 

            var iter = mapMatrix[iterPoints.Current.x,iterPoints.Current.y].GetEnumerator();
            while(iter.MoveNext()) {
                if (iter.Current != null && (iter.Current is T) && predicate((iter.Current as T))) {
                    list.Add((iter.Current as T));
                }
            }
        }
        if(list.Count == 0) return null;
        return list;
    }
    public List<T> getMapObjects<T>(List<Vector2> points, Predicate<T> predicate = default(Predicate<T>)) where T: MapObject {
        var iterPoints = points.GetEnumerator();
        List<T> list = new List<T>();
        while (iterPoints.MoveNext())
        {
            if(iterPoints.Current.x < 0 || iterPoints.Current.y < 0 || iterPoints.Current.x >= width || iterPoints.Current.y >= height) continue; 

            var iter = mapMatrix[(int)iterPoints.Current.x,(int)iterPoints.Current.y].GetEnumerator();
            while(iter.MoveNext()) {
                if (iter.Current != null && (iter.Current is T) && predicate((iter.Current as T))) {
                    list.Add((iter.Current as T));
                }
            }
        }
        if(list.Count == 0) return null;
        return list;
    }

    public void addObject(Object obj) {
        objects.Add(obj);
        
        if(obj is MapObject) {
            if(!(obj is Bullet) && !(obj is RoundBullet) && !(obj is WallSaw)) {
                mapMatrix[(int)(obj as MapObject).position.x,(int)(obj as MapObject).position.y].Add(obj as MapObject);
            }
        }
    }

    public void destroyObject(Object obj) {
        objects.Remove(obj);

        if (obj is WalkableObject) {
            mapMatrix[(obj as WalkableObject).mapLocation.x, (obj as WalkableObject).mapLocation.y].Remove(obj as WalkableObject);
            
        }
        else if (obj is MapObject) {
            mapMatrix[(int)(obj as MapObject).position.x,(int)(obj as MapObject).position.y].Remove(obj as MapObject);
        }

        Destroy(obj.gameObject);
        obj = null;
    }

    public void executeGroup(List<Command> actions) {
        var actionIterator = actions.GetEnumerator();

        while(actionIterator.MoveNext()) {
            var group = groups[actionIterator.Current.groupID];
            if(group == null) return;

            var objectsIterator = group.GetEnumerator();
            
            while(objectsIterator.MoveNext()) {
                objectsIterator.Current.execute(actionIterator.Current);
            }
        }

    }

    public void setupGameObject(GameObject gameObject, Vector3 to) {
        Instantiate(gameObject, to, Quaternion.identity);
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

    public void setupGroups(Dictionary<int, List<Object>> grps) {
        groups = grps;
    }

    public void setupObject(Object obj) {
        obj.map = this;

        if(obj is MapObject) {
            if((obj as MapObject).isTile) {
                setTile(obj as MapObject);
            } else {
                GameObject prefab = Resources.Load<GameObject>("Objects/" + obj.objectName + "/" + obj.objectName);
                obj.gameObject = Instantiate(prefab,(obj as MapObject).position, Quaternion.identity);
            }
        } else {
            GameObject prefab = Resources.Load<GameObject>("Objects/" + obj.objectName + "/" + obj.objectName);
            obj.gameObject = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
        }
        
        if(obj.gameObject != null && obj.gameObject.GetComponent<ObjectController>() != null)
            obj.gameObject.GetComponent<ObjectController>().obj = obj;
        else if(obj.gameObject != null && obj.gameObject.GetComponentInChildren<ObjectController>() != null)
            obj.gameObject.GetComponentInChildren<ObjectController>().obj = obj;
            
        addObject(obj);
    }
    public void setTile(MapObject obj) {
        switch(obj.order) {
            case ObjectOrder.floor:
                tilemaps[0][0].SetTile(new Vector3Int((int)(obj as MapObject).position.x, (int)(obj as MapObject).position.y, 0), (obj as MapObject).tile);
            break;
            case ObjectOrder.onFloor:
                tilemaps[0][1].SetTile(new Vector3Int((int)(obj as MapObject).position.x, (int)(obj as MapObject).position.y, 0), (obj as MapObject).tile);
            break;
            case ObjectOrder.underWall:
                tilemaps[0][2].SetTile(new Vector3Int((int)(obj as MapObject).position.x, (int)(obj as MapObject).position.y, 0), (obj as MapObject).tile);
            break;
            case ObjectOrder.wall:
                tilemaps[0][3].SetTile(new Vector3Int((int)(obj as MapObject).position.x, (int)(obj as MapObject).position.y, 0), (obj as MapObject).tile);
            break;
        }

        //tilemap.gett
    }

    public void Start() {
        preview = GameObject.Find("mapPreview").GetComponent<Image>();
        texture = new Texture2D(32,32);
        s = Sprite.Create(texture, new Rect(0,0, 32, 32), new Vector2(0.5f,0.5f), 32);
        //tilemaps = new List<List<Tilemap>>();
    }

    public void Update() {
        texture.filterMode = FilterMode.Point;

        for(int x = 0; x < 32; x++) {
            for(int y = 0; y < 32; y++) {
                var list = mapMatrix[x,y];

                texture.SetPixel(x,y,Color.white);
                

                if(list.Find(x => x is Floor) != null) {
                    texture.SetPixel(x,y,Color.gray);
                }
                if(list.Find(x => x is Wall) != null) {
                    texture.SetPixel(x,y,new Color(0.25f,0.25f,0.25f,1f));
                } 

                if(list.Find(x => x is MovingFloor) != null) {
                    texture.SetPixel(x,y,Color.magenta);
                }

                if(list.Find(x => x is Box) != null) {
                    texture.SetPixel(x,y,Color.green);
                }

                if(list.Find(x => x is Teleport) != null) {
                    texture.SetPixel(x,y,Color.cyan);
                }

                if(list.Find(x => x is Player) != null) {
                    texture.SetPixel(x,y,Color.yellow);
                }
            }
        }

        texture.Apply();

        preview.sprite = s;
    }
}

public class Group {
    public int groupID;
    public List<Object> objects;
}



