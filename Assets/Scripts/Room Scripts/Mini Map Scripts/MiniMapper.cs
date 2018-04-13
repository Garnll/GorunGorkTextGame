using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapper : MonoBehaviour {

    public Camera mapCamera;
    public GameObject maps;

    private GameObject[] mapsZs;

    private int orderMultiplier = 10;

    public void Awake()
    {
        mapsZs = new GameObject[maps.transform.childCount];
        for (int i = 0; i < mapsZs.Length; i++)
        {
            mapsZs[i] = maps.transform.GetChild(i).gameObject;
        }
    }

    public void MovePlayerInMap(Vector3 playerPosition)
    {
        for (int i = 0; i < mapsZs.Length; i++)
        {

            if (mapsZs[i].name == "Level " + playerPosition.z)
            {
                mapsZs[i].SetActive(true);

            }
            else
            {
                mapsZs[i].SetActive(false);
            }

            if (mapsZs[i].GetComponent<SpriteRenderer>() != null)
            {
                if (mapsZs[i].GetComponent<RoomSprite>() != null)
                {
                    if (mapsZs[i].GetComponent<RoomSprite>().myRoom.roomPosition != playerPosition)
                    {
                        SpriteRenderer[] mapChilds = mapsZs[i].GetComponentsInChildren<SpriteRenderer>();
                        for (int f = 0; f < mapChilds.Length; f++)
                        {
                            mapChilds[f].sortingLayerName = "Default";
                        }
                        mapsZs[i].GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }
                    else
                    {
                        SpriteRenderer[] mapChilds = mapsZs[i].GetComponentsInChildren<SpriteRenderer>();
                        for (int f = 0; f < mapChilds.Length; f++)
                        {
                            mapChilds[f].sortingLayerName = "CurrentRoom";
                        }
                        mapsZs[i].GetComponent<SpriteRenderer>().sortingLayerName = "CurrentRoom";

                    }
                }
            }
        }

        mapCamera.transform.position = new Vector3(playerPosition.x, playerPosition.y, mapCamera.transform.position.z);

        if (playerPosition.z == 8)
        {
            mapCamera.gameObject.SetActive(false);
        }
        else
        {
            mapCamera.gameObject.SetActive(true);
        }


    }

}
