using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour {
	[Header("Prefabs and External Objects")]
    TopGridManager topGridManager;
	Animator animator;
	AudioSource audioSource;
	
	[Header("Settings")]
	[SerializeField]private float timeBeforePopup = 10f;
	
	[Header("Audio")]
	public AudioClip molePoppingOut;
	public AudioClip waving;
	public AudioClip moleLeaving;
	
	[Header("Current Values")]
    Vector2Int currentPos;
	private bool outside;
	
    void Start() {
		topGridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		
        currentPos = Vector2Int.RoundToInt(topGridManager.GetGridSize() / 2);
		transform.position = PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)currentPos * topGridManager.GetDistance());
		
		StartCoroutine(Stop());
    }
	
	IEnumerator Stop() {
		while(true) {
			outside = true;
			animator.SetBool("pop", true);
			audioSource.clip = molePoppingOut;
			audioSource.Play();
			
			yield return new WaitForSeconds(molePoppingOut.length * 2);
			
			animator.SetBool("pop", false);
			animator.SetBool("wave", true);
			audioSource.clip = waving;
			audioSource.Play();
			
			yield return new WaitForSeconds(waving.length * 1.5f);
			
			animator.SetBool("wave", false);
			animator.SetBool("disappear", true);
			audioSource.clip = moleLeaving;
			audioSource.Play();
			
			yield return new WaitForSeconds(moleLeaving.length);
			
			animator.SetBool("disappear", false);
			outside = false;
			TryMove();
			
			yield return new WaitForSeconds(timeBeforePopup);
		}
	}

	void TryMove() {
		//Before starting pathfinding, check if there the mole is trapped.
		if(topGridManager.GetTileByPosition(currentPos + new Vector2Int(0, 1)).GetDugUp() &&
		topGridManager.GetTileByPosition(currentPos + new Vector2Int(1, 0)).GetDugUp() &&
		topGridManager.GetTileByPosition(currentPos + new Vector2Int(0, -1)).GetDugUp() &&
		topGridManager.GetTileByPosition(currentPos + new Vector2Int(-1, 0)).GetDugUp())
			return;
			
		bool gridFound = false;
		while(!gridFound) {
			// Find a random accessible grid
			List<Vector2Int> accessibleGrids = new List<Vector2Int>();
			for (int x = 0; x < topGridManager.GetGridSize().x; x++) {
				for (int y = 0; y < topGridManager.GetGridSize().y; y++) {
					Vector2Int possiblePos = new Vector2Int(x, y);
					if (!topGridManager.GetTileByPosition(possiblePos).GetDugUp()) {
						accessibleGrids.Add(possiblePos);
					}
				}
			}

			if (accessibleGrids.Count == 0) {
				return;
			}

			Vector2Int targetPos = accessibleGrids[Random.Range(0, accessibleGrids.Count)];

			// Use A* pathfinding to check if there's a path to the target grid
			Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
			Queue<Vector2Int> frontier = new Queue<Vector2Int>();
			cameFrom[currentPos] = currentPos;
			frontier.Enqueue(currentPos);

			while (frontier.Count > 0) {
				Vector2Int current = frontier.Dequeue();
				if (current == targetPos) {
					break;
				}

				List<Vector2Int> neighbors = new List<Vector2Int> {
					current + new Vector2Int(1, 0),
					current + new Vector2Int(-1, 0),
					current + new Vector2Int(0, 1),
					current + new Vector2Int(0, -1)
				};

				foreach (Vector2Int next in neighbors) {
					if (next.x >= 0 && next.y >= 0 && next.x + 1< topGridManager.GetGridSize().x && next.y + 1 < topGridManager.GetGridSize().y &&
						!cameFrom.ContainsKey(next) && !topGridManager.GetTileByPosition(next).GetDugUp()) {
						cameFrom[next] = current;
						frontier.Enqueue(next);
					}
				}
			}

			// If a path is found, teleport the mole to the target grid
			if (cameFrom.ContainsKey(targetPos)) {
				gridFound = true;
				currentPos = targetPos;
				transform.position = PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)currentPos * topGridManager.GetDistance());
			}
		}
	}
	
	public bool GetOutside() {
		return outside;
	}
	
	public Vector2Int GetCurrentPos() {
		return currentPos;
	}
}