using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;
using System;

public class SpawnShadowCaster : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject shadowCaster;
    [SerializeField] int radius;

    List<GameObject> shadowCasters;
    GameObject tempShadowCaster;
    Vector3 lastPosition;
    GameObject[,] shadowMap;

    List<Vector2Int> enableShadowCastersPos;
    List<Vector2Int> circlePoints;

    void Start()
    {
        shadowCasters = new List<GameObject>();    
        circlePoints = new List<Vector2Int>();
        
        enableShadowCastersPos = new List<Vector2Int>();
        shadowMap = new GameObject[800,600];
        lastPosition = new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, 0);
        int n = 0;
        for(int i= -400; i < 400; i ++)
            for(int j = 300; j > -300; j--)
            {
                if(tilemap.GetTile(new Vector3Int(i,j,0)) != null)
                {
                    tempShadowCaster = Instantiate(shadowCaster, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity);
                    shadowMap[i+400,j+300] = tempShadowCaster;
                    n++;
                }
            }
        Debug.Log(n);

        circlePoints = FillCircle(radius); 

        foreach (Vector2Int p in circlePoints)
        {
            if(shadowMap[(int)gameObject.transform.position.x + p.x + 400, (int)gameObject.transform.position.y + p.y + 300] != null)
            {
                enableShadowCastersPos.Add(new Vector2Int((int)gameObject.transform.position.x + p.x + 400, (int)gameObject.transform.position.y + p.y + 300));
                shadowMap[(int)gameObject.transform.position.x + p.x + 400, (int)gameObject.transform.position.y + p.y + 300].GetComponent<ShadowCaster2D>().castsShadows = true;
            }
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(shadowCasters.Count);
        if (lastPosition != new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, 0))
        {

            foreach (Vector2Int pos in enableShadowCastersPos)
                shadowMap[pos.x, pos.y].GetComponent<ShadowCaster2D>().castsShadows = false;

            enableShadowCastersPos.Clear();

            foreach (Vector2Int p in FillCircle(radius))
            {
                if(shadowMap[(int)gameObject.transform.position.x + p.x + 400, (int)gameObject.transform.position.y + p.y + 300] != null)
                {
                    enableShadowCastersPos.Add(new Vector2Int((int)gameObject.transform.position.x + p.x + 400, (int)gameObject.transform.position.y + p.y + 300));
                    shadowMap[(int)gameObject.transform.position.x + p.x + 400, (int)gameObject.transform.position.y + p.y + 300].GetComponent<ShadowCaster2D>().castsShadows = true;
                }
                    
            }
            lastPosition = new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, 0);
        }

    }

    List<Vector2Int> FillCircle(int radius)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        for (int i = -radius; i <= 0; i++)
            for (int j = radius; j >= 0; j--)
            {
                if (i * i + j * j <= radius * radius)
                {
                    points.Add(new Vector2Int(i, j));
                    bool isBorder = i == 0 || j == 0;
                    if (!isBorder)
                    {

                        points.Add(new Vector2Int(-i, j));
                        points.Add(new Vector2Int(i, -j));
                        points.Add(new Vector2Int(-i, -j));
                    }
                    else
                    {
                        if (i != 0)
                        {
                            points.Add(new Vector2Int(-i, j));
                            if (j != 0)
                                points.Add(new Vector2Int(-i, -j));
                        }
                        if (j != 0)
                        {
                            points.Add(new Vector2Int(i, -j));
                        }
                    }
                }

            }

        return points;
    }

}


