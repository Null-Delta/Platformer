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
    public List<movement> movements = new List<movement>();

    //время которое волкер находится в покое перед следующим ходом
    public float stayDelay = 0.0f;

    //текущее целочисленное положение объекта на карте
    public Vector2Int mapLocation;
    
    //волкер, относительно которого двигается данный волкер
    public WalkableObject targetWalker = null;

    //волкеры, которые двигаются относительно данного волкера
    public List<WalkableObject> subWalkers = new List<WalkableObject>();

    //отступ, относительно координат targetWalker'а
    Vector2Int targetOffset = Vector2Int.zero;

    //переменная, отвечающяя за игнорирование перемещений
    private bool _isIgnoreMoves = false;
    public bool isIgnoreMoves {
        get {
            return _isIgnoreMoves;
        }
        set{
            if(value) {
                movements.RemoveAll(x => true);
            }
            _isIgnoreMoves = value;
        }
    }

    //-----внутренние переменные для перемещения-----
    Vector2 translate = Vector2.zero;
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
        if (movements.Count == 0 && !isWalk)
            animationTime = 0;

        if(move.isAnimate && !isIgnoreMoves) {
            movements.Add(move);
        } else if(!move.isAnimate) {
            
            movements.Add(move);

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

            movements.RemoveAt(0);
        }
    }

    //вызывается, когда объект переместил свои координаты в матрице карты. здесь объект пытается найти волкера, относительно которого он будет двигаться
    //это не конечная реализация
    virtual public bool tryFindTarget() {
        if(targetWalker != null) clearTarget();
        
        if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is MovingFloor) != null) {
            setTarget(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is MovingFloor)[0] as MovingFloor);
            translate = -(position - targetWalker.position);
            targetOffset = Vector2Int.zero;
            moveStartPosition = -translate;
            return true;
        }

        return false;
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
    public void setLocationOnMap() {
        Vector2Int move = movements[0].point;

        map.removeMapObject(mapLocation, this);

        subWalkers.ForEach(x => {
            map.removeMapObject(x.mapLocation,x);
        });

        mapLocation = move + mapLocation;

        map.insertMapObject(mapLocation, this);

        subWalkers.ForEach( x => {
            x.mapLocation = mapLocation + x.targetOffset;
            Debug.Log(x.targetOffset);
        });
        subWalkers.ForEach(x => {
            map.insertMapObject(x.mapLocation,x);
        });

        setLocationInSubTargets();
    }

    public void setLocationInSubTargets() {
        subWalkers.ForEach(x => {
            x.movements.Insert(0, new movement(Vector2Int.zero,false));
            x.setLocationOnMap();
            x.movements.RemoveAt(0);
        });
    }

    void setTarget(WalkableObject target) {
        targetWalker = target;
        targetWalker.subWalkers.Add(this);
        targetOffset = mapLocation - targetWalker.mapLocation;
    }
    void clearTarget() {
        targetWalker.subWalkers.Remove(this);
        targetWalker = null;
    }
    void setupMoving() {

        setLocationOnMap();

        Vector2Int move = movements[0].point;

        bool isFindTarget = tryFindTarget();

        if(targetWalker == null) {
            moveStartPosition = position;
            translate = move + ((mapLocation - move) - moveStartPosition);

        } else {
            moveStartPosition = targetWalker.position - position;

            if(!isFindTarget) {
                translate = move;
                targetOffset += move;
            }
        }
    }

    public override void updateObject()
    {
        animationTime += Time.deltaTime;
        
        if(targetWalker != null) {
            position = targetWalker.position - moveStartPosition;
        }

        if(animationTime > stayDelay && movements.Count == 0) {
            animationTime = stayDelay;
        }

        repeateMove:
        if(animationTime > stayDelay && movements.Count != 0) {
            if(!isWalk) {
                if(!canMoveOn(mapLocation + movements[0].point)) {
                    movements.RemoveAt(0);
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
                movements.RemoveAt(0);
                isWalk = false;

                animationTime -= stayDelay + moveDelay;

                if(targetWalker != null) {
                    position = targetWalker.position - moveStartPosition + translate;
                    moveStartPosition = targetWalker.mapLocation - mapLocation;
                } else {
                    targetOffset = Vector2Int.zero;
                    position = moveStartPosition + translate;
                }

                if(map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject) != null) {
                    map.getMapObjects<MapObject>(mapLocation.x, mapLocation.y, x => x is PressableObject).ForEach(x => {
                        (x as PressableObject).OnPressStart(this);
                    });
                }

                onEndWalk();

                if(movements.Count != 0)
                    goto repeateMove;

            } else {
                if(targetWalker == null) {
                    position = moveStartPosition + translate * ((animationTime - stayDelay) / moveDelay);
                } else {
                    position = targetWalker.position - moveStartPosition + translate * ((animationTime - stayDelay) / moveDelay);
                }
            }
        }
    }


    public WalkableObject(int x,int y) : base(x,y) {
        mapLocation = new Vector2Int(x,y);
    }
}