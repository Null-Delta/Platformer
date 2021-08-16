using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : PressableObject
{
    float activationTime = 0.5f;
    float damage = 10;
    float timeRunner = 0;

    public override string objectName => "Mina";

    public override void updateObject()
    {
        if (timeRunner > 0)
        {
            timeRunner -= Time.deltaTime;
            //активна, ждёт взрыва
            this.gameObject.transform.localScale += new Vector3(0.01f, 0.01f,0);//to delet
            if (timeRunner <=0)
            { //бум
                var tmpList =map.getMapObjects<WalkAndLive>((int)position.x, (int)position.y, x => x is WalkAndLive);
                if(tmpList != null)
                {
                    for (int i = 0; i != tmpList.Count; i++)
                        tmpList[i].getDamage(damage);
                }
                map.destroyObject(this);
            }
        }
    }

    public override void OnPressStart(WalkableObject walker)
    {
        if (timeRunner == 0)
            timeRunner = activationTime;
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        order = ObjectOrder.underWall;
        
    }

    public Mina(int x, int y, float _activationTime, float _damage): base(x,y) {
        this.activationTime = _activationTime;
        this.damage = _damage;
    }
    public Mina(int x, int y): base(x,y) {
        this.activationTime = 0.5f;
        this.damage = 10;
    }
}
