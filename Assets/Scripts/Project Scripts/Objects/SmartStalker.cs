using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PathPoint
{
    public float rangeFromStart;
    public float rangeFromEnd;
    public Vector2 position;
    public PathPoint PreviosPoint;

    float neighbourRange()
    {
        return 1;
    }
    public static float HeuristicRange(Vector2 start, Vector2 end)
    {
        return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
    }



    public PathPoint( Vector2 point,float start = 0, float end=0, PathPoint back = null)
    {
        position = point;
        rangeFromStart = start;
        rangeFromEnd = end;
        PreviosPoint = back;
    }
    public PathPoint(){}
    
    public float takeRange()
    {
        return this.rangeFromStart + this.rangeFromEnd;
    }

    public Vector2 takePath()
    {
        if (this.PreviosPoint.PreviosPoint != null)
            return this.PreviosPoint.takePath();
        else
            return this.position;
    }

    public List<PathPoint> takeNeigbours(Vector2 end, Map map, SmartStalker me)
    {
        List<Vector2> neighbours = new List<Vector2>
        {
            this.position+Vector2.up, this.position+Vector2.down,this.position+Vector2.left,this.position+Vector2.right
        };
        List<PathPoint> rezult = new List<PathPoint>();
        foreach (var item in neighbours)
        {
            if (me.canBePath(item))
                rezult.Add( new PathPoint(item, this.rangeFromStart + neighbourRange(), HeuristicRange(item, end), this));
        }
        return rezult;  
    }

}
public class DoubleList 
{
    public DoubleList next = null;
    DoubleList back = null;
    public PathPoint point;
    public int count = 0;

    public void add(PathPoint newPoint)
    {
        if (this.point !=null)
        {
            DoubleList newList = new DoubleList();
            newList.point = newPoint;
            this.back.next = newList;
            newList.back =this.back;
            newList.next =this;
            this.back = newList;
            count++;
        }
        else
        {
            this.point = newPoint;
            this.next = this;
            this.back = this;
            count = 1;
        }
    }

    public PathPoint takeMinPoint()
    {
        DoubleList begin = this;
        DoubleList runner = this.next;
        DoubleList minPoint = this;
        float minRange = this.point.takeRange();
        
        do
        {
            if (minRange > runner.point.takeRange())
            {
                minRange =runner.point.takeRange();
                minPoint = runner;
            }
            runner = runner.next;
        } while (runner != begin);


        if (count==1)
        {
            count--;
            PathPoint tmpPoint =  minPoint.point;
            minPoint.point = null;
            return tmpPoint;
        }
        else if (minPoint == begin)
        {
            count--;
            PathPoint tmpPoint =  minPoint.point;
            this.point = this.next.point;
            minPoint = this.next;
            minPoint.back.next = minPoint.next;
            minPoint.next.back = minPoint.back;
            return tmpPoint;
        }
        else
        {
            count--;
            minPoint.back.next = minPoint.next;
            minPoint.next.back = minPoint.back;
            return minPoint.point;
        }
        
    }

    public PathPoint foundPoint<T>(Predicate<T> predicate) where T: PathPoint
    {
        if (this.count == 0)
            return null;
        DoubleList begin = this;
        DoubleList runner = this;
        PathPoint rezult = null;
        do
        {
            if (predicate((runner.point as T)))
            {
                rezult = runner.point;
                break;
            }
            runner = runner.next;
        } while (runner != begin);

        return rezult;
    }


    public DoubleList(){}
}







public class SmartStalker : Seeker
{
    public override string objectName => "SmartStalker";
    
    public bool isAttack = false;
    public float rangeOfAttack = 1;

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.3f;
        order = ObjectOrder.wall;

        hp = 100;
        immortalTimeForHit = 0.5f;
        foundRange = 5;
        canFall = true;
    }

    Vector2Int toInt(Vector2 v)
    {
        return new Vector2Int((int)v.x,(int)v.y);
    }

    public override bool canMoveOn(Vector2Int point)
    {
        bool tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
                tmpReturn = (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements.Peek().point));
                if (!tmpReturn)
                {
                    var tmpMov = movements.Peek();

                    var tmpInt = tmpMov.point.x;
                    tmpMov.point.x = tmpMov.point.y;
                    tmpMov.point.y = tmpInt;

                    addMovement(tmpMov);
                }
                return tmpReturn;
            }
        }

        tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if (!tmpReturn)
        {
            var tmpMov = movements.Peek();

            var tmpInt = tmpMov.point.x;
            tmpMov.point.x = tmpMov.point.y;
            tmpMov.point.y = tmpInt;

            addMovement(tmpMov);
        }
        return tmpReturn;
    }

    public virtual bool canBePath(Vector2 item)
    {
        return !(map.getMapObjects<MapObject>((int)item.x, (int)item.y, x=> (x.isCollisiable && !(x is Player)) ) != null || map.getMapObjects<MapObject>((int)item.x, (int)item.y, x=> x is Floor || x is MovingFloor || (x is BreakableFloor && (x as BreakableFloor).isReal)) == null);
    }

    override public void updateObject()
    {
        base.updateObject();
        if (movements.Count == 0 && foundTarget)
        {
            foundTarget = false;
        }
        if (target == null)
        {
            saveTarget = Camera.main.GetComponent<PlayerControl>().CurrentPlayer;
            getTarget(Camera.main.GetComponent<PlayerControl>().CurrentPlayer);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    


    Vector2 aStar(Vector2 start, Vector2 end)
    {
        start = toInt(start);
        if (!this.canBePath(end))
            return new Vector2(0,0);
        DoubleList waitPoint = new DoubleList();
        DoubleList lookedPoint = new DoubleList();
        PathPoint startPoint = new PathPoint(start, 0, PathPoint.HeuristicRange(start,end));
        waitPoint.add(startPoint);

        while (waitPoint.count > 0)
        {
            PathPoint nowPoint = waitPoint.takeMinPoint();
            if (nowPoint.position == end)
            {
                return nowPoint.takePath();//выход
            }
            
            lookedPoint.add(nowPoint);
            
            foreach (PathPoint item in nowPoint.takeNeigbours(end, map, this))
            {
                if (lookedPoint.foundPoint<PathPoint>(x=> x.position == item.position) != null) continue; 
                var tmpList = waitPoint.foundPoint<PathPoint>(x=> x.position == item.position);
                
                if (tmpList == null) waitPoint.add(item);
                else if (tmpList.rangeFromStart > item.rangeFromStart)
                {
                    tmpList.rangeFromStart = item.rangeFromStart;
                    tmpList.PreviosPoint = item;
                }
                
            }
            
        }
        return new Vector2(0,0);
    }

    //////////////////////////////////////////////////////////////////////////////
    void foundWay()
    {
        if (foundTarget)
            if (Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y) > rangeOfAttack)
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                    return;
                addMovement(new movement(toInt(tmpVector -position), true));
            }
            else
            {
                isAttack = true;
                startOfAttack();
            }
    }

    override public void onStartWalk()
    {
        base.onStartWalk();
        
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public override void firstLook()
    {
        foundWay();
    }

    public virtual void startOfAttack()
    {
    }



    public SmartStalker(int x, int y): base(x,y) {
        
    }
}
