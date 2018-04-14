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
        if (playerPosition.z == 8)
        {
            mapCamera.gameObject.SetActive(false);
            return;
        }
        else
        {
            mapCamera.gameObject.SetActive(true);
        }

        for (int i = 0; i < mapsZs.Length; i++)
        {
            if (mapsZs[i].name == "Level " + playerPosition.z)
            {
                mapsZs[i].SetActive(true);

                RoomSprite[] visualRooms = mapsZs[i].GetComponentsInChildren<RoomSprite>();

                for (int g = 0; g < visualRooms.Length; g++)
                {

                    if (visualRooms[g].GetComponent<RoomSprite>().myRoom.roomPosition != playerPosition)
                    {
                        SpriteRenderer[] mapChilds = visualRooms[g].GetComponentsInChildren<SpriteRenderer>();

                        for (int f = 0; f < mapChilds.Length; f++)
                        {
                            mapChilds[f].sortingLayerName = "Default";
                        }
                        visualRooms[g].GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }
                    else
                    {
                        SpriteRenderer[] mapChilds = visualRooms[g].GetComponentsInChildren<SpriteRenderer>();
                        for (int f = 0; f < mapChilds.Length; f++)
                        {
                            mapChilds[f].sortingLayerName = "CurrentRoom";
                        }
                        visualRooms[g].GetComponent<SpriteRenderer>().sortingLayerName = "CurrentRoom";

                    }
                }

            }
            else
            {
                mapsZs[i].SetActive(false);
            }



        }

        mapCamera.transform.position = new Vector3(playerPosition.x, playerPosition.y, mapCamera.transform.position.z);


    }

}
