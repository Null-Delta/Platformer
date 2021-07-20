using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: Walker
{
    public override string objectName => "Player";
    public int stepCount = 0, dir = 0;
    override public void onWalkStart(bool is_wall) {
        linearMove.dx = 0;
        linearMove.dy = 0;
        stepCount++;

        if(stepCount >= 2) {
            stepCount = 0;
            int lastDir = dir;
            dir = Random.Range(0,5);
            
            if(dir != lastDir)
            switch (dir) {
                case 0:
                gameObject.GetComponent<Animator>().Play("MoveUp",0,0);
                break;
                case 1:
                gameObject.GetComponent<Animator>().Play("MoveDown",0,0);
                break;
                case 2:
                gameObject.GetComponent<Animator>().Play("MoveLeft",0,0);
                break;
                case 3:
                gameObject.GetComponent<Animator>().Play("MoveRight",0,0);
                break;
                case 4:
                gameObject.GetComponent<Animator>().Play("MoveStop",0,0);
                break;
            }
        }

        switch (dir) {
                case 0:
                linearMove.dy = 1;
                break;
                case 1:
                linearMove.dy = -1;
                break;
                case 2:
                linearMove.dx = -1;
                break;
                case 3:
                linearMove.dx = 1;
                break;
            }

            if(map.getMapObjects<StaticMapObject>((int)position.x + (int)linearMove.dx, (int)position.y + (int)linearMove.dy, x => x.objectName == "Floor") == null) {
                if(linearMove.dx != 0) linearMove.dx = 0;
                if(linearMove.dy != 0) linearMove.dy = 0;
            }

        // if(is_wall) {
        //     linearMove.dy = -linearMove.dy;
        //     linearMove.dx = -linearMove.dx;
        // }
    }

    public override void startObject()
    {
        base.startObject();

        linearMove.dx = 1;
        linearMove.dy = 0;
        move_delay = 0.0f;
        animation_time = 0.25f;
    }
    override public void onWalkFinish() {
        
    }

    public override bool isCollizion(MapObject obj)
    {
        return false;
    }

    public Player(float x, float y): base(x,y) {

    }
}
