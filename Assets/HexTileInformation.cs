using UnityEngine;
using System.Collections;

public class HexTileInformation : MonoBehaviour
{
    public HexType HexType;
}

public enum HexType
{
    Wall,
    Mirror
}