using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct movement {
    public Vector2Int point;
    public bool isAnimate;

    public movement(Vector2Int p, bool isAnim) {
        point = p;
        isAnimate = isAnim;
    }
}

public class WalkableObject: MapObject {

    public override string objectName => "Walker";

    //время, за которое происходит перемещение объекта(одинаково для всех волкеров)
    public const float moveDelay = 0.2f;

    //список запланированных перемещений
    public Queue<movement> movements = new Queue<movement>();

    //время которое волкер находится в покое перед следующим ходом
    public float stayDelay = 0.0f;

    //текущее целочисленное положение объекта на карте
    public Vector2Int mapLocation;

    //волкер, относительно которого двигается данный волкер
    public WalkableObject localTarget = null;

    //-----внутренние переменные для перемещения-----
    Vector2 translate = Vector2.zero;
    Vector2 targetOffset = Vector2.zero;
    Vector2 moveStartPosition = Vector2.zero;
    float animationTime = 0f;
    bool isWalk = false;
    //-----------------------------------------------


    public override void startObject()
    {
        base.startObject();
        order = ObjectOrder.wall;
    }

    //вызывается перед началом перемещения
    virtual public void onStartWalk() { 
        
    }

    //вызывается в конце перемещения
    virtual public void onEndWalk() { 

    }

    //добавляет в очередь перемещение
    public void addMovement(movement move) { 
        if(move.isAnimate) {
            movements.Enqueue(move);
        } else {
            movements.Enqueue(move);

            if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject) != null) {
                map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject).ForEach(x => {
                    (x as PressableObject).OnPressEnd(this);
                });
            }

            setLocationOnMap();

            gameObject.transform.position = new Vector3(mapLocation.x,mapLocation.y,0);
            position = gameObject.transform.position;

            if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject) != null) {
                map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject).ForEach(x => {
                    (x as PressableObject).OnPressStart(this);
                });
            }

            movements.Dequeue();
        }
    }

    //вызывается, когда объект переместил свои координаты в матрице карты. здесь объект пытается найти волкера, относительно которого он будет двигаться
    //это не конечная реализация
    virtual public void tryFindTarget() {
        if(localTarget as MovingFloor != null && (localTarget as MovingFloor).movingObject == this) (localTarget as MovingFloor).movingObject = null;
        localTarget = null;
        
        if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is MovingFloor) != null) {
            localTarget = (map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is MovingFloor)[0] as MovingFloor);
            (localTarget as MovingFloor).movingObject = this;
        }
    }


    //вызывается перед попыткой соверщить перемещение в точку point.
    //возвращает логическое значение, означающее может ли объект перепеститься в точку point
    //если метод вернет false, то первое перемещение из очереди movements удаляется и происходит проверка со стедующим в очереди перемещением
    //Возможно такое рещение будет переделанно
    virtual public bool canMoveOn(Vector2Int point) {
        return true;
    }

    //метод перемещяющий объект в матрице карты. 
    //он виртуальный для переопределения в двигяющемся полу.
    //Возможно такое рещение будет переделанно
    virtual public void setLocationOnMap() {
        Vector2Int move = movements.Peek().point;

        map.removeMapObject(mapLocation, this);
        mapLocation = move + mapLocation;
        map.insertMapObject(mapLocation, this);
    }

    virtual public void setTarget(WalkableObject target) {
        localTarget = target;
        targetOffset = position - localTarget.position;
    }

    void setupMoving() {

        setLocationOnMap();

        Vector2Int move = movements.Peek().point;

        tryFindTarget();

        if(localTarget != null) {
            targetOffset = position - localTarget.position;
            translate = -targetOffset;
            moveStartPosition = localTarget.position + targetOffset;
        } else {
            targetOffset = Vector3.zero;
            moveStartPosition = gameObject.transform.position;
            translate = move + ((mapLocation - move) - moveStartPosition);
        }
    }

    public override void updateObject()
    {
        animationTime += Time.deltaTime;

        if(localTarget != null) {
            moveStartPosition = localTarget.position + targetOffset;
            position = moveStartPosition;
        }

        if(animationTime > stayDelay && movements.Count == 0) {
            animationTime = stayDelay;
        }

        repeateMove:
        if(animationTime > stayDelay && movements.Count != 0) {
            if(!isWalk) {

                if(!canMoveOn(mapLocation + movements.Peek().point)) {
                    movements.Dequeue();
                    if(movements.Count != 0)
                        goto repeateMove;
                    return;
                }

                isWalk = true;
                
                onStartWalk();

                if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject) != null) {
                    map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject).ForEach(x => {
                        (x as PressableObject).OnPressEnd(this);
                    });
                }

                setupMoving();
            }

            if(animationTime > stayDelay + moveDelay) {
                movements.Dequeue();

                animationTime -= stayDelay + moveDelay;
                position = moveStartPosition + translate;

                if(localTarget != null) {
                    targetOffset = position - localTarget.position;
                } else {
                    targetOffset = Vector3.zero;
                }
                isWalk = false;

                if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject) != null) {
                    map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject).ForEach(x => {
                        (x as PressableObject).OnPressStart(this);
                    });
                }

                onEndWalk();

                if(movements.Count != 0)
                    goto repeateMove;

            } else {
                position = moveStartPosition + translate * ((animationTime - stayDelay) / moveDelay);
            }
        }
    }


    public WalkableObject(int x,int y) : base(x,y) {
        mapLocation = new Vector2Int(x,y);
    }
}