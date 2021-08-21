using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : PressableObject
{
    float activationTime;
    float disableTime;

    float stayTime = 1;
    float damage = 10;
    float time;
    public bool isActiv;

    bool[] b = new bool[]{true, true, true, true};
    public override string objectName => "Spike";
    public override void updateObject()
    {
        time += Time.deltaTime;
        if (time > 0)
        {
            if(b[0])
            {
                gameObject.GetComponentInChildren<Animator>().Play("Show", 0, 0);
                gameObject.transform.GetChild(0).GetComponentInChildren<ParticleSystem>().Play();
                b[0] = false;
            }

            if (time > activationTime/2)
            {
                if(b[1])
                {
                    isActiv = true;
                    if(map.getMapObjects<MapObject>((int)position.x, (int)position.y, x => x is WalkAndLive) != null)
                    {
                        (map.getMapObjects<MapObject>((int)position.x, (int)position.y, x => x is WalkAndLive)[0] as WalkAndLive).getDamage(damage);
                    }
                    b[1] = false;
                }

                if (time > stayTime + activationTime)
                {
                    if(b[2])
                    {
                        
                        gameObject.GetComponentInChildren<Animator>().Play("Hide", 0, 0);
                        b[2] = false;
                    }

                    if (time > stayTime + activationTime + activationTime/2)
                    {
                        isActiv = false;

                        if(time > stayTime + activationTime*2)
                        {
                            time = -disableTime;
                            b[0] = true;
                            b[1] = true;
                            b[2] = true;
                        }
                    }
                }
            }
        }
    }

    public override void OnPressStart(WalkableObject walker)
    {
        if(isActiv)
            if(map.getMapObjects<MapObject>((int)position.x, (int)position.y, x => x is WalkAndLive) != null)
            {
                (map.getMapObjects<MapObject>((int)position.x, (int)position.y, x => x is WalkAndLive)[0] as WalkAndLive).getDamage(damage);
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

    public Spike(int x, int y, float activationTime, float disable, float startOffset): base(x,y) {
        this.activationTime = activationTime;
        time = -startOffset;
        disableTime = disable;
     }
}