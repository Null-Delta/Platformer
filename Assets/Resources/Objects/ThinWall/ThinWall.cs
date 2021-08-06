using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinWall : MapObject
{
    public override string objectName => "ThinWall";
    public bool[] sideCollisiable;
    public override void startObject()
    {
        base.startObject();
        isCollisiable = true;

        order = ObjectOrder.wall;
        setupOrder();
    }

    public bool tryWalkInside(Vector2Int point)
    {
        if (point == new Vector2Int (1,0)) return sideCollisiable[2];
        else if (point == new Vector2Int (0,1)) return sideCollisiable[3];
        else if (point == new Vector2Int (-1,0)) return sideCollisiable[0];
        else if (point == new Vector2Int (0,-1)) return sideCollisiable[1];
        return false;
    }

    public bool tryWalkOutside(Vector2Int point)
    {
        if (point == new Vector2Int (1,0)) return sideCollisiable[0];
        else if (point == new Vector2Int (0,1)) return sideCollisiable[1];
        else if (point == new Vector2Int (-1,0)) return sideCollisiable[2];
        else if (point == new Vector2Int (0,-1)) return sideCollisiable[3];
        return false;
    }

    public ThinWall(int x, int y, bool[] sideCollisiable) : base(x, y)
    {
        this.sideCollisiable = sideCollisiable;
    }

    public ThinWall() : base(0, 0)
    {

    }
}

// public override bool canMoveOn(Vector2Int point)
//     {   
//         if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is ThinWall) != null)
//         {
//             if(!map.getMapObjects<ThinWall>(mapLocation.x, mapLocation.y, x => x.objectName == "ThinWall")[0].tryWalkOutside(movements.Peek().point))
//                 return false;
//         }

//         if(map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
//         {   
//             if(map.getMapObjects<MapObject>(point.x, point.y, x => x is ThinWall) != null) {
//                 return (map.getMapObjects<ThinWall>(point.x, point.y, x => x.objectName == "ThinWall")[0].tryWalkInside(movements.Peek().point));
//             }

//             if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
//                 return (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements.Peek().point));
//             }
//         }
        
//         return map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable == true) == null;
//     }
//Box
// if(map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable == true) != null)
//         {
//             if(map.getMapObjects<MapObject>(point.x, point.y, x => x is ThinWall) != null)
//             {
//                 return (map.getMapObjects<ThinWall>(point.x, point.y, x => x.objectName == "ThinWall")[0].tryWalkInside(movements.Peek().point));
//             }
//         }