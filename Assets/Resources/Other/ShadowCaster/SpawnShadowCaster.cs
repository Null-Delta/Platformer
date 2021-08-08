using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SpawnShadowCaster : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject shadowCaster;
    [SerializeField] int radius;

    List<GameObject> shadowCasters;
    Vector3 lastPosition;

    
    void Start()
    {
        shadowCasters = new List<GameObject>();
        lastPosition = new Vector3Int((int)gameObject.transform.position.x,(int)gameObject.transform.position.y, 0);

        // for(int i= -400; i < 400; i ++)
        //     for(int j = 300; j > -300; j--)
        //     {
        //         if(tilemap.GetTile(new Vector3Int(i,j,0)) != null)
        //         {
        //             shadowCasters.Add(Instantiate(shadowCaster, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity));
        //         }
        //     }

        foreach(Vector2Int p in FillCircle(radius))
            {
                if(tilemap.GetTile(new Vector3Int((int)gameObject.transform.position.x + p.x, (int)gameObject.transform.position.y + p.y, 0)) != null)
                shadowCasters.Add(Instantiate(shadowCaster, new Vector3((int)gameObject.transform.position.x + 0.5f + p.x,(int)gameObject.transform.position.y + 0.5f + p.y, 0), Quaternion.identity));
            }
    }

    void FixedUpdate()
    {
        Debug.Log(shadowCasters.Count);
        if(lastPosition != new Vector3Int((int)gameObject.transform.position.x,(int)gameObject.transform.position.y, 0))
        {
            
            foreach(GameObject obj in shadowCasters)
            Destroy(obj);
            shadowCasters.Clear();
            foreach(Vector2Int p in FillCircle(radius))
            {
                if(tilemap.GetTile(new Vector3Int((int)gameObject.transform.position.x + p.x, (int)gameObject.transform.position.y + p.y, 0)) != null)
                shadowCasters.Add(Instantiate(shadowCaster, new Vector3((int)gameObject.transform.position.x + 0.5f + p.x,(int)gameObject.transform.position.y + 0.5f + p.y, 0), Quaternion.identity));
            }
            lastPosition = new Vector3Int((int)gameObject.transform.position.x,(int)gameObject.transform.position.y, 0);
        }
          
    }

    List<Vector2Int> FillCircle(int radius)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        for(int i = -radius; i <= radius; i++)
            for(int j = radius; j >= -radius; j--)
            {
                if (i*i + j*j <= radius*radius)
                    points.Add(new Vector2Int(i,j));
            }

        return points;
    }
      
}


