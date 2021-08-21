using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerChel : SmartStalker
{
    public override string objectName => "LazerChel";
    
    int typeAttack = 0;
    int countOfLazer= 0;
    Vector2Int directionOfAttack;
    float longerTime;
    List<Vector2Int> lazerPoint;

    List<GameObject> g ;//to delet


    // Товарищ Рустам. Надеюсь, читая это сообщение, ты уже сделал текстурку для Лазерного чувака.
    // Кроме того, я очень надеюсь, что ты сделал текстурку для лазера. Так вот, этим лазерам, которые спавнит игрок
    // нужно добавить скрипт, что уничтожает их через несколько секунд.


    public override void startObject()
    {
        base.startObject();   

        hp = 20;
        stayDelay = 0.4f;
        foundRange = 9;
        damage = 10;
        attackTime = 1f;
        startOfDamageTime = 0.8f;
        endOfDamageTime = 0.1f;
        rangeOfAttack = 7;
        typeOfWalk=1;
    }


    public override void startOfAttack()
    {
        base.startOfAttack();
        longerTime = 0.8f;
        countOfLazer = 0;
        lazerPoint = new List<Vector2Int>();
        g = new List<GameObject>();
        
        if (typeAttack ==0)
        {
            var tmpVector = target.position - this.position;
            if (tmpVector.x == 0)
                if (tmpVector.y > 0)
                    directionOfAttack = Vector2Int.up;
                else
                    directionOfAttack = Vector2Int.down;
            else
                if (tmpVector.x > 0)
                    directionOfAttack = Vector2Int.right;
                else
                    directionOfAttack = Vector2Int.left;
        }

    }

    public override void updateObject()
    {
        base.updateObject();
        if (attackRunner > 0) 
        {
            attackRunner -=Time.deltaTime;
            if (attackRunner <= startOfDamageTime && attackRunner >= endOfDamageTime) // нанесение урона
            {
                dealDamage();
            }
            if (attackRunner <=0)  // конец атаки
            {
                isAttack = false;
                for (int i = 0; i != g.Count; i++)
                {
                    map.delet(g[i]);
                }
                foundWay();
            }
        }
    }



    public override void foundWay()
    {
        if (foundTarget)
        {
            float tmpFloat = Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y);
            if (tmpFloat > rangeOfAttack || (tmpFloat <= rangeOfAttack && tmpFloat >= rangeOfAttack-4.5f && Mathf.Abs(target.position.x - position.x)!=0 &&  Mathf.Abs(target.position.y - position.y) !=0) )
            {
                var tmpVector = aStar(position, target.mapLocation);
                if (tmpVector.x == 0 && tmpVector.y == 0)
                    return;
                addMovement(new movement(toInt(tmpVector -position), true));
            }
            else if (tmpFloat <= rangeOfAttack && tmpFloat >= rangeOfAttack-4.5f && (Mathf.Abs(target.position.x - position.x)==0 ||  Mathf.Abs(target.position.y - position.y) ==0))
            {
                typeAttack = 0;
                isAttack = true;
                startOfAttack();
            }
            else if ( tmpFloat <= rangeOfAttack-4.5f)
            {
                typeAttack = 1;
                isAttack = true;
                startOfAttack();
            }
        }
    }

    public override void dealDamage()
    {
        if (typeAttack == 0)
        {
            if (countOfLazer< rangeOfAttack && attackRunner <= longerTime)
            {
                longerTime -=0.03f;
                countOfLazer++;
                lazerPoint.Add(this.mapLocation+directionOfAttack*countOfLazer);
                g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));
            }
            
        }
        else if (typeAttack == 1)
        {
            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.up);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.up+Vector2Int.right);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.up+Vector2Int.left);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.down);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.down+Vector2Int.right);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.down+Vector2Int.left);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.right);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            countOfLazer++;
            lazerPoint.Add(this.mapLocation+Vector2Int.left);
            g.Add(map.createLazerPath(lazerPoint[countOfLazer-1]));

            longerTime = 0;
        }
        
        List<WalkAndLive> tmpList =map.getMapObjects<WalkAndLive>(lazerPoint, x => x is WalkAndLive);
        if(tmpList != null)
        {
            for (int i = 0; i != tmpList.Count; i++)
                if (damagedList.Contains(tmpList[i]) == false)
                {
                    tmpList[i].getDamage(damage, 0);
                    damagedList.Add(tmpList[i]);
                }
        }
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        foundWay();
    }

    public LazerChel(int x, int y): base(x,y) {
        
    }
}
