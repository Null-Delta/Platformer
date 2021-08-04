using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : PressableObject
{
    float respawnTime = 2; //время респавна
    float timer;

    public override string objectName => "OnPressObject";

    public override void OnPressStart(WalkableObject walker)
    {
        if (walker is Player)
        {
            (walker as Player).nowCheckPoint = this;
            //игрок зашёл на чекпоинт
        }
    }

    public override void OnPressEnd(WalkableObject walker)
    {
        if (walker is Player)
        {
            (walker as Player).nowCheckPoint = this;
            //игрок вышел на чекпоинт
        }
    }

    public void spawnPlayer()
    {
        timer = respawnTime;
    }
    public override void updateObject()
    {
        if (timer > 0)
            {
                timer -=Time.deltaTime;
                if (timer <= 0)
                    {
                        Player tmpPlayer = new Player((int)position.x, (int)position.y);
                        tmpPlayer.nowCheckPoint = this;
                        map.setupObject(tmpPlayer);
                        //игрок возродился
                    }
            }
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = true;
        gameObject.transform.position = position;
        order = ObjectOrder.underWall;
    }

    public CheckPoint(int x, int y): base(x,y) { }
}
