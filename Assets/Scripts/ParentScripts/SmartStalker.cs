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
    public static float HeuristicRange(Vector2 start, Vector2 end, int type)
    {
        //return (end-start).magnitude;
        //(Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y))*
        if (type == 1)
            return  (Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y)) * (Mathf.Abs(end.x - start.x)+1) * (Mathf.Abs(end.y - start.y)+1);
        else if (type == 0)
        {
            return  (Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y));
        }
        else
            return (end-start).magnitude;
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
                rezult.Add( new PathPoint(item, this.rangeFromStart + neighbourRange(), HeuristicRange(item, end, me.typeOfWalk), this));
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
    public int typeOfWalk =0;

    float waitTime = 0;
    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.3f;
        order = ObjectOrder.wall;

        hp = 100;
        foundRange = 5;
        canFall = true;
    }

    public Vector2Int toInt(Vector2 v)
    {
        return new Vector2Int((int)v.x,(int)v.y);
    }

    public override bool canMoveOn(Vector2Int point)
    {
        bool tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
                tmpReturn = (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements[0].point));
                return tmpReturn;
            }
        }

        tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        return tmpReturn;
    }

    public virtual bool canBePath(Vector2 item)
    {
        return !(map.getMapObjects<MapObject>((int)item.x, (int)item.y, x=> (x.isCollisiable && !(x is Player)) ) != null || map.getMapObjects<MapObject>((int)item.x, (int)item.y, x=> x is Floor || x is MovingFloor || (x is BreakableFloor && (x as BreakableFloor).isReal)) == null);
    }

    override public void updateObject()
    {
        base.updateObject();
        if (waitTime >0)
            waitTime-=Time.deltaTime;
    }

    //////////////////////////////////////////////////////////////////////////////
    


    public Vector2 aStar(Vector2 start, Vector2 end)
    {
        start = toInt(start);
        if (!this.canBePath(end))
            return new Vector2(0,0);
        DoubleList waitPoint = new DoubleList();
        DoubleList lookedPoint = new DoubleList();
        PathPoint startPoint = new PathPoint(start, 0, PathPoint.HeuristicRange(start,end, this.typeOfWalk));
        waitPoint.add(startPoint);
        int count =0;
        while (waitPoint.count > 0)
        {
            count++;
            if (count > 100)
            {
                return new Vector2(0,0);
            }
            PathPoint nowPoint = waitPoint.takeMinPoint();
            if (nowPoint.position == end)
            {
                return nowPoint.takePath();//??????????
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
    public override void foundWay()
    {
        if (waitTime<=0 && foundTarget)
            if (Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y) > rangeOfAttack)
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                {
                    waitTime = 1f;
                    return;
                }
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


    public override void firstLook()
    {
        foundWay();
    }



    public SmartStalker(int x, int y): base(x,y) {
        
    }
}
