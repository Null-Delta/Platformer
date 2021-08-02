using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloor : PressableObject
{
    public override string objectName => "BreakableFloor";

    float timer;
    float timeToDestroy;
    bool isDestroying;
    public bool isReal;

    public override void OnPressStart(WalkableObject walker)
    {
        isDestroying = true;
        timer =timeToDestroy;
        // начало исчезновения
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        order = ObjectOrder.floor;
        isReal = true;
        isDestroying = false;
    }

    public override void updateObject()
    {
        if (isDestroying && isReal)
        {
            timer -= Time.deltaTime;
            if (timer <=0)
            {
                timer =timeToDestroy;
                isReal = false;
                var tmpOnMe = map.getMapObjects<WalkAndLive>((int)position.x, (int)position.y, x => x.canFall);
                if (tmpOnMe != null)
                {
                    for (int i = 0; i!= tmpOnMe.Count; i++)
                    {
                        tmpOnMe[i].onFall();
                    }
                }
                // полное исчезновение
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if (isDestroying && !isReal)
        {
            timer -= Time.deltaTime;
            if (timer <=0)
            {
                isReal = true;
                isDestroying = false;
                // полное восстановление
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public BreakableFloor(int x, int y, float time):base(x,y)
    {
        position = new Vector2(x,y);
        timeToDestroy = time;
    }
}
