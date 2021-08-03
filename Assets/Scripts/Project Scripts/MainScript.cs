using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject map;
    List<Object> objects = new List<Object>();
    Dictionary<int, List<Object>> groups = new Dictionary<int, List<Object>>();
    //int i = 0; Нигде не используется

    //float time = 0f; Нигде не используется
    void Start()
    {
        
        map = new GameObject();
        //Time.timeScale = 0.1f;

        //objects.Add(new Bullet(1.3f, 10.2f));
        //objects.Add(new Bullet(6.3f, 10.2f));
        //objects.Add(new Walker(10,10));

        SetRect<Wall>(0,0,30,30,0);
        objects.Add(new Wall(3,28));
        objects.Add(new Wall(1,28));
        objects.Add(new Wall(3,26));
        objects.Add(new Wall(2,26));
        objects.Add(new Wall(1,26));
        objects.Add(new CheckPoint(14,14));
        objects.Add(new CheckPoint(14,15));
        objects.Add(new Spike(9,15, 1));
        //SetRect<Wall>(5,5,1,1,0);
        //SetRect<Wall>(5,5,20,20,0);
        //SetRect<Wall>(12,12,6,6,1);
        //objects.Add(new Player(5,10));
        //objects.Add(new Player(2,10));
        //objects.Add(new Player(3,10));
        //objects.Add(new WalkableObject(10,10));

        // objects.Add(new Walker(9,10));
        // Door testD = new Door(20,20);
        // Door testD1 = new Door(20,19);
        // Key testK = new Key(22,22);
        // Key testK1 = new Key(21,22);
        // testD.addKey(testK);
        // testD.addKey(testK1);

        // testD1.addKey(testK);
        // testD1.addKey(testK1);
        //Time.timeScale = 1f;
        //Application.targetFrameRate = 10;
        // objects.Add(testD);
        // objects.Add(testD1);
        // objects.Add(testK);
        // objects.Add(testK1);
        objects.Add(new BreakableFloor(20,19, 1));
        objects.Add(new BreakableFloor(21,19, 1));
        objects.Add(new BreakableFloor(22,19, 1));
        objects.Add(new BreakableFloor(23,19, 1));
        objects.Add(new UsualStalker(18,18));

        objects.Add(new Teleport(10,15, 15, 15, new List<Command>(){}));
        objects.Add(new Teleport(15,15, 15, 10, new List<Command>(){}));
        objects.Add(new Teleport(15,10, 10, 10, new List<Command>(){}));
        objects.Add(new Teleport(10,10, 10, 15, new List<Command>(){}));
        // objects.Add(new Teleport(10,15, 11, 10, new List<Command>() {}));
        // objects.Add(new Teleport(11,10, 16, 15, new List<Command>(){}));

        // objects.Add(new OnPressObject(25,25));
        //objects.Add(new Walker(3,15));
        // objects.Add(new Bullet(10,10));
        // objects.Add(new Bullet(11,10));
        // objects.Add(new Bullet(12,10));
        // objects.Add(new Bullet(13,10));
        //objects.Add(new Turrel_bullet(15,25, 0.3f, 0.3f));
        //objects.Add(new Live_wall(5,25, 0.8f, 0.8f));
        //objects.Add(new PlayerStalker(7, 7, MainPlayer));
        //objects.Add(new MovingFloor(9, 21));
        List<Vector2Int> moveLeft = new List<Vector2Int>() {
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right,
        };

        List<Vector2Int> moveRight = new List<Vector2Int>() {
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right,
        };

        objects.Add(new MovingFloor(9, 20,moveLeft,0.5f));
        objects.Add(new MovingFloor(9, 21,moveRight,0.5f));
        objects.Add(new MovingFloor(9, 22,moveLeft,0.5f));
        objects.Add(new MovingFloor(9, 23,moveRight,0.5f));
        objects.Add(new MovingFloor(9, 24,moveLeft,0.5f));

        objects.Add(new MovingFloor(10, 20,moveLeft,0.5f));
        objects.Add(new MovingFloor(10, 21,moveRight,0.5f));
        objects.Add(new MovingFloor(10, 22,moveLeft,0.5f));
        objects.Add(new MovingFloor(10, 23,moveRight,0.5f));
        objects.Add(new MovingFloor(10, 24,moveLeft,0.5f));

        objects.Add(new MovingFloor(11, 20,moveLeft,0.5f));
        objects.Add(new MovingFloor(11, 21,moveRight,0.5f));
        objects.Add(new MovingFloor(11, 22,moveLeft,0.5f));
        objects.Add(new MovingFloor(11, 23,moveRight,0.5f));
        objects.Add(new MovingFloor(11, 24,moveLeft,0.5f));

        objects.Add(new MovingFloor(12, 20,moveLeft,0.5f));
        objects.Add(new MovingFloor(12, 21,moveRight,0.5f));
        objects.Add(new MovingFloor(12, 22,moveLeft,0.5f));
        objects.Add(new MovingFloor(12, 23,moveRight,0.5f));
        objects.Add(new MovingFloor(12, 24,moveLeft,0.5f));

        objects.Add(new MovingFloor(13, 20,moveLeft,0.5f));
        objects.Add(new MovingFloor(13, 21,moveRight,0.5f));
        objects.Add(new MovingFloor(13, 22,moveLeft,0.5f));
        objects.Add(new MovingFloor(13, 23,moveRight,0.5f));
        objects.Add(new MovingFloor(13, 24,moveLeft,0.5f));

        //objects.Add(new MovingFloor(20, 9));
        SetRect<Floor>(0,0,20,20,1);
        SetRect<Floor>(0,25,20,20,1);

        var door = new Door(8,8);
        objects.Add(door);

        groups[0] = new List<Object>();
        groups[0].Add(door);

        var openTimer = new Timer(3f, new List<Command>(){
            new Command("Open",0,new List<string>(), new List<string>()),
            new Command("Restart",2,new List<string>(), new List<string>())
        }, true);

        objects.Add(openTimer);
        groups[1] = new List<Object>();
        groups[1].Add(openTimer);

        var closeTimer = new Timer(3f, new List<Command>(){
            new Command("Close",0,new List<string>(), new List<string>()),
            new Command("Restart",1,new List<string>(), new List<string>())
        }, false);

        objects.Add(closeTimer);
        groups[2] = new List<Object>();
        groups[2].Add(closeTimer);
        
        var door2 = new Door(9,8);
        objects.Add(door2);
        groups[3] = new List<Object>() {door2};

        var counter = new Counter(3, new List<Command>() {
            new Command("Open", 3, new List<string>(), new List<string>())
        }, new List<Command>());

        objects.Add(counter);
        groups[4] = new List<Object>() {counter};

        objects.Add(new Key(10,12,new List<Command>() {
            new Command("Add", 4, new List<string>(), new List<string>())
        }));
        objects.Add(new Key(11,12,new List<Command>() {
            new Command("Add", 4, new List<string>(), new List<string>())
        }));
        objects.Add(new Key(12,12,new List<Command>() {
            new Command("Add", 4, new List<string>(), new List<string>())
        }));

        Player MainPlayer = new Player(8,10);
        objects.Add(MainPlayer);

        objects.Add(new Box(13,13));
        objects.Add(new Box(14,13));

        SetRect<Grass>(0,0,100,100,1);

        //objects.RemoveAll(x => x is Grass && 
            //objects.Find(y => y is MapObject && (!(y is Grass) && (y as MapObject).position == (x as MapObject).position) || (y is MovingFloor)) == null);

        Random.InitState(228);

        objects.RemoveAll(x => x is Grass && objects.Find(y => (y is Floor) && (y as MapObject).position == (x as MapObject).position) == null);
        objects.RemoveAll(x => x is Grass && Random.Range(0,5) == 1);

        List<Grass> newGrasses = new List<Grass>();
        objects.ForEach(x => {
            if(x is Wall) {
                newGrasses.Add(new Grass((int)(x as MapObject).position.x, (int)(x as MapObject).position.y));
            }
        });

        objects.AddRange(newGrasses);
        map.AddComponent<Map>();
        map.GetComponent<Map>().setupObjects(objects);
        map.GetComponent<Map>().setupGroups(groups);

        //groups[5] = new List<Object>();

    }

    void SetRect<T>(int x, int y, int widht, int height, int hollow) where T : MapObject, new()
    {
        for(int i = x; i < x + widht; i++)
            for(int j = y; j < y + height; j++)
            {
                if(hollow == 0)
                    if (!(i == x || j == y || i == x + widht -1 || j == y + height -1)) continue;

                T obj = new T();
                obj.position = new Vector2(i,j);
                objects.Add(obj);
            }
    }

    
}
