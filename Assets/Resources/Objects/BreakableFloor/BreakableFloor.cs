using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloor : PressableObject
{
    public override string objectName => "BreakableFloor";

    float timer;
    float timeToDestroy;
    float timeToRestore;
    bool isDestroying;
    public bool isReal;

    public override void OnPressStart(WalkableObject walker)
    {
        isDestroying = true;
        timer =timeToDestroy;
        // начало исчезновения
        // gameObject.GetComponent<Animator>().Play("Break",0,0f);
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
                timer = timeToRestore;
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
                gameObject.GetComponent<Animator>().Play("Break",0,0f);
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
                gameObject.GetComponent<Animator>().Play("Restore",0,0f);
            }
        }
    }

    public BreakableFloor(int x, int y, float time, float restore):base(x,y)
    {
        position = new Vector2(x,y);
        timeToDestroy = time;
        timeToRestore = restore;
    }
}
