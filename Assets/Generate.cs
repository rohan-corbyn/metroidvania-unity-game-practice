using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile groundTopTile;
    public Tile groundBottomTile;
    public Tile darkTile;

    private const int levelWidth = 200;
    private const int levelHeight = 15;
    private const int groundHeight = 2;
    private const int startY = 4;
    private const int minY = 3;
    private const int maxY = 10;
    private const int maxSameYTiles = 11;
    private const int bridgeLength = 20;
    private const int bridgeHeight = 6;
    private const int minGroundWidth = 2;
    public TileBridgeLayout bridgeLayoutAsset; // Reference to the TileBridgeLayout asset

    private int sameYCount = maxSameYTiles;
   

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int currentY = startY;


        for (int x = 0; x < levelWidth; x++)
        {
            // Check if the maximum consecutive tiles with the same y position is reached


            if (sameYCount == 6)
            {
                DrawBridge(x, currentY);
                x = x + bridgeLength - 1;
                sameYCount = Random.Range(minGroundWidth, maxSameYTiles);

            }
            else
            {
                DrawGroundColumn(x, currentY, sameYCount);
                x = x + sameYCount - 1;
                sameYCount = Random.Range(minGroundWidth, maxSameYTiles);
                float prob = currentY < (maxY - minY) / 2 ? 0.2f : -0.2f;
                float rand = Random.Range(-1, 2);
                currentY = rand > prob ? currentY + 1 : currentY - 1;
                currentY = Mathf.Clamp(currentY, minY, maxY); // Clamp the y position within the min and max range
            }





        }
    }

    private void DrawGroundColumn(int groundXPos, int groundYPos, int groundLength)
    {
        for (int x = groundXPos; x < groundXPos + groundLength; x++)
        {

            for (int i = 0; i < groundYPos + 10; i++)
            {
                Vector3Int tilePosition = new Vector3Int(x, groundYPos - i, 0);
                tilemap.SetTile(tilePosition, darkTile);
            }

            for (int i = 0; i < groundHeight; i++)
            {
                Vector3Int tilePosition = new Vector3Int(x, groundYPos + i, 0);
                tilemap.SetTile(tilePosition, i == 0 ? groundBottomTile : groundTopTile);
            }




        }
    }

    private void DrawBridge(int startX, int y)
    {
        y += 4;
        int index = 0;

        for (int i = 0; i < bridgeHeight; i++)
        {
            for (int x = startX; x < startX + bridgeLength; x++)
            {
                if (bridgeLayoutAsset.bridgeLayout[index] is null == false)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y - i, 0);
                    tilemap.SetTile(tilePosition, bridgeLayoutAsset.bridgeLayout[index]);
                }
                index++;
            }
        }
        for (int i = 0; i < y + 10; i++)
        {
            for (int x = startX; x < startX + bridgeLength; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y - bridgeHeight - i, 0);
                tilemap.SetTile(tilePosition, darkTile);
            }
        }




    }
}