using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapper : MonoBehaviour {

    public Camera mapCamera;
    public GameObject maps;

    private GameObject[] mapsZs;

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

            if (mapsZs[i].name == "Map At " + playerPosition.z + " Z")
            {
                mapsZs[i].SetActive(true);
            }
            else
            {
                mapsZs[i].SetActive(false);
            }
        }

        mapCamera.transform.position = new Vector3(playerPosition.x, playerPosition.y, mapCamera.transform.position.z);

        if (playerPosition.z == 10)
        {
            mapCamera.gameObject.SetActive(false);
        }
        else
        {
            mapCamera.gameObject.SetActive(true);
        }


    }

}
