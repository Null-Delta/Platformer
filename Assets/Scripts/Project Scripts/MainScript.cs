using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScript : MonoBehaviour
{
    public GameObject map;
    List<Object> objects = new List<Object>();
    Dictionary<int, List<Object>> groups = new Dictionary<int, List<Object>>();

    void Start()
    {
        
        map = new GameObject();
        //Time.timeScale = 0.1f;

        //objects.Add(new Bullet(1.3f, 10.2f));
        //objects.Add(new Bullet(6.3f, 10.2f));
        //objects.Add(new Walker(10,10));

        SetRect<Wall>(0,0,60,30,0);
        SetRect<Wall>(3,3,5,1,0);
        
        objects.Add(new Wall(3,28));
        objects.Add(new Wall(1,28));
        objects.Add(new Wall(3,26));
        objects.Add(new Wall(2,26));
        objects.Add(new Wall(1,26));
        objects.Add(new CheckPoint(8,10));
        objects.Add(new CheckPoint(14,14));
        objects.Add(new CheckPoint(14,15));
        //SetRect<Floor>(30,30,50,50,1);
        //SetRectSpike(30,30,50,50,1, 0.25f, 1, 0f);
        //objects.Add(new Spike(8,15, 0.25f, 1, 0f));
        // objects.Add(new Spike(7,15, 0.25f, 1, 0.2f));
        // objects.Add(new Spike(6,15, 0.25f, 1, 0.4f));
        // objects.Add(new Spike(5,15, 0.25f, 1, 0.6f));

        objects.Add(new Spike(8,16, 0.25f, 1, 0.2f));
        objects.Add(new Spike(7,16, 0.25f, 1, 0.4f));
        objects.Add(new Spike(6,16, 0.25f, 1, 0.6f));
        // objects.Add(new Spike(7,16, 0.25f, 1, 0.4f));
        // objects.Add(new Spike(6,16, 0.25f, 1, 0.6f));
        // objects.Add(new Spike(5,16, 0.25f, 1, 0.8f));

        // objects.Add(new Spike(8,17, 0.25f, 1, 0.4f));
        // objects.Add(new Spike(7,17, 0.25f, 1, 0.6f));
        // objects.Add(new Spike(6,17, 0.25f, 1, 0.8f));
        // objects.Add(new Spike(5,17, 0.25f, 1, 1f));

        // objects.Add(new Spike(8,18, 0.25f, 1, 0.6f));
        // objects.Add(new Spike(7,18, 0.25f, 1, 0.8f));
        // objects.Add(new Spike(6,18, 0.25f, 1, 1.0f));
        // objects.Add(new Spike(5,18, 0.25f, 1, 1.2f));

        // objects.Add(new Spike(8,15, 0.25f, 1, 0));
        // objects.Add(new Spike(7,15, 0.25f, 1, 0));
        // objects.Add(new Spike(6,15, 0.25f, 1, 0));
        // objects.Add(new Spike(5,15, 0.25f, 1, 0));

        // objects.Add(new Spike(8,16, 0.25f, 1, 0));
        // objects.Add(new Spike(7,16, 0.25f, 1, 0));
        // objects.Add(new Spike(6,16, 0.25f, 1, 0));
        // objects.Add(new Spike(5,16, 0.25f, 1, 0));

        // objects.Add(new Spike(8,17, 0.25f, 1, 0));
        // objects.Add(new Spike(7,17, 0.25f, 1, 0));
        // objects.Add(new Spike(6,17, 0.25f, 1, 0));
        // objects.Add(new Spike(5,17, 0.25f, 1, 0));

        // objects.Add(new Spike(8,18, 0.25f, 1, 0));
        // objects.Add(new Spike(7,18, 0.25f, 1, 0));
        // objects.Add(new Spike(6,18, 0.25f, 1, 0));
        // objects.Add(new Spike(5,18, 0.25f, 1, 0));




        objects.Add(new Wall(19,19));

        objects.Add(new Teleport(10,15, 15, 15, new List<Command>(){}));
        objects.Add(new Teleport(15,15, 15, 10, new List<Command>(){}));
        objects.Add(new Teleport(15,10, 10, 10, new List<Command>(){}));
        objects.Add(new Teleport(10,10, 10, 15, new List<Command>(){}));
        
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

        // var main = new MovingFloor(16,5,new List<Vector2Int>() {
        //     Vector2Int.left,
        //     Vector2Int.left,
        //     Vector2Int.left,
        //     Vector2Int.right,
        //     Vector2Int.right,
        //     Vector2Int.right,
        // }, 0.25f);

        // var subMain = new MovingFloor(14,5, new List<Vector2Int>() {
        //     Vector2Int.up,
        //     Vector2Int.up,
        //     Vector2Int.right,
        //     Vector2Int.right,
        //     Vector2Int.right,
        //     Vector2Int.right,
        //     Vector2Int.down,
        //     Vector2Int.down,
        //     Vector2Int.down,
        //     Vector2Int.down,
        //     Vector2Int.left,
        //     Vector2Int.left,
        //     Vector2Int.left,
        //     Vector2Int.left,
        //     Vector2Int.up,
        //     Vector2Int.up,
        // }, 0.25f);

        // var subSubMain = new MovingFloor(13,5, new List<Vector2Int>() {
        //     Vector2Int.up,
        //     Vector2Int.right,
        //     Vector2Int.right,
        //     Vector2Int.down,
        //     Vector2Int.down,
        //     Vector2Int.left,
        //     Vector2Int.left,
        //     Vector2Int.up,
        // }, 0.25f);

        // subMain.setTarget(main);
        // subSubMain.setTarget(subMain);

        // objects.Add(main);
        // objects.Add(subMain);
        // objects.Add(subSubMain);

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

        SetRect<Floor>(19,0,20,20,1);

        //SetRect<Grass>(0,0,100,100,1);

        //objects.RemoveAll(x => x is Grass && 
            //objects.Find(y => y is MapObject && (!(y is Grass) && (y as MapObject).position == (x as MapObject).position) || (y is MovingFloor)) == null);

        //Random.InitState(228);

        //objects.RemoveAll(x => x is Grass && objects.Find(y => (y is Floor) && (y as MapObject).position == (x as MapObject).position) == null);
        //objects.RemoveAll(x => x is Grass && Random.Range(0,5) == 1);

        List<Grass> newGrasses = new List<Grass>();

        objects.ForEach(x => {
            if(x is Floor) {
                if(UnityEngine.Random.Range(0f,1f) < 0.75f)
                    newGrasses.Add(new Grass((int)(x as MapObject).position.x, (int)(x as MapObject).position.y));
            }
        });

        objects.AddRange(newGrasses);

             if (true)
        {
            objects.Add(new Jumper(22,9));
            objects.Add(new WallSaw(3,2,Vector2.right, Vector2.up, 18, 5));
            objects.Add(new LazerChel(26,9));
            objects.Add(new EarthWizard(36,9));
            objects.Add(new Runner(36,10));
            objects.Add(new RoundWizard(36,5));
            objects.Add(new Assassin(36,13));
            objects.Add(new Warrior(32,15));
            objects.Add(new Bowman(33,2));
            objects.Add(new Bull(34,7));
            objects.Add(new Mina(24,7));
            objects.Add(new Mina(27,14));
            objects.Add(new Mina(22,3));
            objects.Add(new Mina(25,9));
        }

        //objects.RemoveAll(x => x is MapObject && new Rect(3,3,4,4).Contains((x as MapObject).position));

        // objects.Add(new BreakableFloor(6,6,0.5f,2f));
        // objects.Add(new BreakableFloor(6,5,0.5f,2f));
        // objects.Add(new BreakableFloor(6,4,0.5f,2f));
        // objects.Add(new BreakableFloor(6,3,0.5f,2f));

        // objects.Add(new BreakableFloor(5,6,0.5f,2f));
        // objects.Add(new BreakableFloor(5,5,0.5f,2f));
        // objects.Add(new BreakableFloor(5,4,0.5f,2f));
        // objects.Add(new BreakableFloor(5,3,0.5f,2f));

        // objects.Add(new BreakableFloor(4,6,0.5f,2f));
        // objects.Add(new BreakableFloor(4,5,0.5f,2f));
        // objects.Add(new BreakableFloor(4,4,0.5f,2f));
        // objects.Add(new BreakableFloor(4,3,0.5f,2f));

        // objects.Add(new BreakableFloor(3,6,0.5f,2f));
        // objects.Add(new BreakableFloor(3,5,0.5f,2f));
        // objects.Add(new BreakableFloor(3,4,0.5f,2f));
        // objects.Add(new BreakableFloor(3,3,0.5f,2f));
        
        map.AddComponent<Map>();
        map.GetComponent<Map>().tilemap = GameObject.Find("Tilemap - lvl 0").GetComponent<Tilemap>();
        map.GetComponent<Map>().tilemap1 = GameObject.Find("Tilemap - lvl 1").GetComponent<Tilemap>();
        map.GetComponent<Map>().tilemap2 = GameObject.Find("Tilemap - lvl 2").GetComponent<Tilemap>();
        map.GetComponent<Map>().tilemap3 = GameObject.Find("Tilemap - lvl 3").GetComponent<Tilemap>();
        
        map.GetComponent<Map>().setupObjects(objects);
        map.GetComponent<Map>().setupGroups(groups);
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

    void SetRectSpike(int x, int y, int widht, int height, int hollow, float activationTime, float disable, float startOffset)
    {
        for(int i = x; i < x + widht; i++)
            for(int j = y; j < y + height; j++)
            {
                if(hollow == 0)
                    if (!(i == x || j == y || i == x + widht -1 || j == y + height -1)) continue;

                Spike obj = new Spike(i,j,activationTime,disable,startOffset);
                //obj.position = new Vector2(i,j);
                objects.Add(obj);
            }
    }

    
}
