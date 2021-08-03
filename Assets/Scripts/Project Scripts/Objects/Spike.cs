using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MapObject
{
    float activationTime;
    float damage = 10;
    float timer;
    bool isActiv;
    SpriteRenderer spik;

    public override string objectName => "Spike";

    public override void updateObject()
    {
        timer -= Time.deltaTime;
        if (timer <0)
        {
            timer =activationTime;
            if (isActiv)
            {
                isActiv = false;
                spik.enabled = false;
            }
            else
            {
                isActiv = true;
                spik.enabled = true;
            }
        }
        if (isActiv)
        {
            var tmpList = map.getMapObjects<WalkAndLive>((int)position.x, (int)position.y, x => (x is WalkAndLive) );
            if (tmpList != null)
            {
                var tmpIter = tmpList.GetEnumerator();
                while (tmpIter.MoveNext())
                {
                    tmpIter.Current.getDamage(damage);
                }
            }
        }
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        order = ObjectOrder.underWall;
        spik = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public Spike(int x, int y, float time): base(x,y) {
        activationTime = time;
        timer = time;
     }
}
