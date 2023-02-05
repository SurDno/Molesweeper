using UnityEngine;

public class TopGridManager : MonoBehaviour {
	[Header("Prefabs and External Objects")]
	[SerializeField] private GameObject tilePrefab;
	
	[Header("Settings")]
	[SerializeField] private Vector2Int gridSize = new Vector2Int(9, 9);
	[SerializeField] private int distanceBetweenGrids = 16;
	
	[Header("Current Values")]
	private Tile[,] tiles;
	
    void Start() {
		tiles = new Tile[gridSize.x, gridSize.y];
        MakeTopGrid(gridSize);
    }
	
    void MakeTopGrid(Vector2Int size) {
		float unitDistanceBetweenGrids = PixelConversion.ConvertPixelDistanceToWorldDistance(distanceBetweenGrids);
		
        for(int i = 0; i < size.x; i++)
            for(int j = 0; j < size.y; j++) {
                Vector2 position = new Vector2(i * unitDistanceBetweenGrids, j * unitDistanceBetweenGrids);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
				tile.GetComponent<SpriteRenderer>().flipX = Random.Range(0, 1f) > 0.5f;
				tile.GetComponent<SpriteRenderer>().flipY = Random.Range(0, 1f) > 0.5f;
				tile.GetComponent<Tile>().SetMyPos(new Vector2Int(i, j));
				tiles[i, j] = tile.GetComponent<Tile>();
            }
			
    }
	
	public Vector2 GetGridSize() {
		return gridSize;
	}
	
	public float GetDistance() {
		return distanceBetweenGrids;
	}
	
	public Tile GetTileByPosition(Vector2Int position) {
		return tiles[position.x, position.y];
	}
	
	public Tile GetTileByPosition(int x, int y) {
		return tiles[x, y];
	}
}
