using UnityEngine;
using UnityEngine.Tilemaps;



[CreateAssetMenu(fileName = "TileBridgeLayout", menuName = "Custom/Tile Bridge Layout")]
public class TileBridgeLayout : ScriptableObject
{
    public Tile[] bridgeLayout = new Tile[21];
}