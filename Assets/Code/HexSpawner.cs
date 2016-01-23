using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexSpawner : MonoBehaviour
{
    public static HexSpawner Instance { get; private set; }

    public GameObject Hex;

    private string[] hexGrid = new string[] {
    ".........xxxxxxxxx.........",
    ".....xxxxxxxxxxxxxxxx......",
    "...xxxxxxxxxxxxxxxxxxxxx...",
    "...xxxxxxxxxxxxxx..xxxx....",
    "...xxxxx.xxxxxxx....xxxxxx.",
    "...xxxx..xxxxxxxxx..xxxxxxx",
    ".....xxxxxx...xxxxxxxxxxxxx",
    "...xxxxxxxxxxxxxxxxxxxxx...",
    "...xxx...xxxxxxxxxxxxxxxx..",
    "...xxxxxxxxxxxxxxxxxxxxx...",
    "...xxxxxxxxxxxxxx...xxxx...",
    "...xxxxxxxxx...xxxxxxxxxx..",
    "...xxxxxxxxxx.xxxxxxxxxx...",
    "...xxxxxxxx......xxxxxxx...",
    "...xxxxxxxx..xx..xxxxxxx...",
    "...xxxxxxxxxx.....xxxxxx...",
    ".....xxxxxxxxxxxxxxxxx.....",
    ".......xxxxxxxxxxx........."};

    public float XOffSet;
    public float YOffSet;
    public float ZOffSet;

    List<GameObject> genHexes;

    Bounds bounds;
    
    void Start()
    {
        Instance = this;
        genHexes = new List<GameObject>();
        GenHexes();
    }

    void Update()
    {
        
    }

    public void GenHexes()
    {
        foreach (var hex in genHexes)
        {
            Destroy(hex);
        }

        genHexes.Clear();

        var hexPoints = Hex.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        var hexFlats = Hex.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.z;
        var hexFlatsHalf = (hexFlats / 2);
        var hexPointsHalf = (hexPoints / 2);
        var hexPointsQuart = (hexPoints / 4);

        for (int i = 0; i < hexGrid.Length; i++)
        {
            for (int j = 0; j < hexGrid[i].Length; j++)
            {
                if (hexGrid[i][j] == 'x')
                {
                    GameObject spawnedHex;
                    if (i % 2 == 0)
                    {
                        spawnedHex = (GameObject)Instantiate(Hex, new Vector3(j * hexFlats * XOffSet, 0, i * (hexPointsHalf + hexPointsQuart) * ZOffSet), Quaternion.identity);
                    }
                    else //Ever second row
                    {
                        spawnedHex = (GameObject)Instantiate(Hex, new Vector3((j * hexFlats * XOffSet) + hexFlatsHalf, 0, i * (hexPointsHalf + hexPointsQuart) * ZOffSet), Quaternion.identity);
                    }

                    spawnedHex.transform.SetParent(transform);
                    genHexes.Add(spawnedHex);
                }
            }
        }
    }

    public enum HexType
    {
        None = '.',
        Wall = 'x',
        Mirror = 'm'
    }
}