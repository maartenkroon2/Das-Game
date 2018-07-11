using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private GameObject mapPlane;
    private Terrain terrain;

    [SerializeField]
    private float detectionRadius = 10f;

    CapsuleCollider collider;
    int terrainResolution;
    float lowestPointBelowWater, waterLevel = 0; // This should be a fancy function that gets the waterLevel.

    // Use this for initialization.
    void Start()
    {
        mapPlane = GameObject.Find("MapPlane");
        terrain = Terrain.activeTerrain;
        terrainResolution = terrain.terrainData.heightmapResolution - 1;

        lowestPointBelowWater = GetLowestPointBelowWater();
        CreateTexture();

        // Creates a collider for detection of objects.
        collider = gameObject.AddComponent<CapsuleCollider>();
        collider.radius = detectionRadius;

        //The collider height should be twice the height of the lowest point in the terrain.
        collider.height = lowestPointBelowWater * 2;
        Debug.Log(lowestPointBelowWater);
    }

    // Finds lowest point in the map.
    private float GetLowestPointBelowWater()
    {
        float lowestPoint = terrain.terrainData.GetHeight(0, 0);
        for (int x = 0; x < terrainResolution; x++)
        {
            for (int y = 0; y < terrainResolution; y++)
            {
                float currentHeight = terrain.terrainData.GetHeight(x, y);
                lowestPoint = lowestPoint < currentHeight ? lowestPoint : currentHeight;
            }
        }
        return lowestPoint + (waterLevel + terrain.transform.position.y);
    }

    void CreateTexture()
    {
        // Places the mapPlane over the terrain and makes it the same size as the terrain.       
        mapPlane.transform.position = new Vector3(terrain.transform.position.x + terrain.terrainData.size.x / 2, terrain.transform.position.y, terrain.transform.position.z + terrain.terrainData.size.z / 2);
        mapPlane.transform.localScale = new Vector3(terrain.terrainData.size.x / 10, 0, terrain.terrainData.size.z / 10);
        mapPlane.layer = LayerMask.NameToLayer("Map");

        // Use a color array since this is much quicker than calling texture.SetPixel for each pixel in the texture.
        Color[] textureColors = new Color[terrainResolution * terrainResolution];

        // Finds the corect color for each pixel in the texture 
        for (int y = 0; y < terrainResolution; y++)
        {
            for (int x = 0; x < terrainResolution; x++)
            {
                float terrainheight = terrain.terrainData.GetHeight(y, x) + terrain.transform.position.y;
                if (terrainheight > waterLevel)
                {
                    textureColors[x + (y * terrainResolution)] = new Color(0.95f, 0.9f, 0.7f, 1); // Color for terrain above waterLevel
                }
                else if (terrainheight > waterLevel - 6) // Vraag van Maarten: wat is 6? Is dit shoreOffset of iets dergelijks?
                {
                    textureColors[x + (y * terrainResolution)] = Color.white;
                }
                else
                {
                    //Vraag van Maarten: the numbers Mason what do they mean!
                    textureColors[x + (y * terrainResolution)] = new Color(0, Mathf.Floor((terrainheight - lowestPointBelowWater - 6) / (waterLevel - lowestPointBelowWater - 6) * 10) / 10, 1, 1); // Color between blue(0,0,1,1) and cyan(0,1,1,1).
                }
            }
        }

        // Creates the texture for the planeMap and sets the colors using the color array.
        Texture2D texture = new Texture2D(terrainResolution, terrainResolution);
        texture.SetPixels(textureColors);
        texture.Apply();
        mapPlane.GetComponent<Renderer>().material.mainTexture = texture;
    }
}
