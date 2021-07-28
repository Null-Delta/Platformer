using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Walker
{

    int nowDirection;

    public override string objectName => "Box";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool readyCheck()
    {
        return nowDirection != -1;
    }

    public override void onWalkFinish()
    {
        nowDirection = -1;
    }
    public bool willMove() {
        return nowDirection != -1;
    }

    public override void onWalkStart()
    {

    }

    public void setDirection(int dir) {
        linearMove.x = 0;
        linearMove.y = 0;

        nowDirection = dir;

        switch (dir) {
            case 0:
                linearMove.x = -1;
            break;
            case 1:
                linearMove.y = 1;
            break;
            case 2:
                linearMove.x = 1;
            break;
            case 3:
                linearMove.y = -1;
            break;
        }

        if(map.getMapObjects<MapObject>((int)(position.x + linearMove.x), (int)(position.y + linearMove.y), x => x.isCollisiable == true || x as Walker != null) != null)
        {
            nowDirection = -1;
        }

        sum_time = 0f;

    }

    public Box(int x, int y): base(x,y) {
        
    }

    public override void startObject()
    {
        base.startObject();
        nowDirection = -1;
        move_delay = 0f;
        animation_time = 0.1f;
    }
}
