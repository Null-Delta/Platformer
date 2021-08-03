using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MapObject
{
    float activationTime;
    float disableTime;
    float damage = 10;
    float timer;
    bool isActiv;

    public override string objectName => "Spike";

    public override void updateObject()
    {
        timer -= Time.deltaTime;
        if (timer <0)
        {
            timer = (isActiv ? disableTime : activationTime) + timer;

            if (isActiv)
            {
                isActiv = false;
                gameObject.GetComponent<Animator>().Play("Hide", 0, 0);
            }
            else
            {
                isActiv = true;
                gameObject.GetComponent<Animator>().Play("Show", 0, 0);
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
        //spik = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public Spike(int x, int y, float time, float disable, float startOffset): base(x,y) {
        activationTime = time;
        timer = time + startOffset;
        disableTime = disable;
     }
}
