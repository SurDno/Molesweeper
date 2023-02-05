using UnityEngine;
using System.Collections;

public class CharacterDigging : MonoBehaviour {
	[Header("Prefabs and External Objects")]
	[SerializeField] private GameObject digUpPrefab;
	private CharacterMovement characterMovement;
	private TopGridManager gridManager;
	
	[Header("Settings")]
	[SerializeField] private bool controlledWithArrow;
	[SerializeField] private float timeToDig;
	
	[Header("Current Values")]
	[SerializeField] private bool digging;
	
	void Start() {
		characterMovement = GetComponent<CharacterMovement>();
		gridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
	}
	
	void FixedUpdate() {
		// Check for input.
		if(controlledWithArrow) {
			if(Input.GetKey(KeyCode.Return))
				Dig();
		} else {
			if(Input.GetKey(KeyCode.Space))
				Dig();
		}
	}
	
	void Dig() {
		Vector2Int gridToDig = characterMovement.GetFacingGrid();
		
		// If we're trying to dig outside of world border, ignore digging.
		if((gridToDig.x >= gridManager.GetGridSize().x)
			|| (gridToDig.y >= gridManager.GetGridSize().y)
			|| (gridToDig.x < 0)
			|| (gridToDig.y < 0))
			return;
			
		// If we're moving, we can't dig.
		if(characterMovement.GetMovingValue())
			return;
		
		// If we already dug this up, ignore digging.
		if(gridManager.GetTileByPosition(gridToDig).GetDugUp())
			return;
		
		// If there's a mole, ignore digging.
		if(!gridManager.GetTileByPosition(gridToDig).CanDig())
			return;
		
		StartCoroutine(DigAHole(gridToDig));
	}
	
	IEnumerator DigAHole(Vector2Int gridToDig) {
		digging = true;
		
		yield return new WaitForSeconds(timeToDig);
		
		digging = false;
		
		Instantiate(digUpPrefab, PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)gridToDig * gridManager.GetDistance()), Quaternion.identity);
		gridManager.GetTileByPosition(gridToDig).SetDugUp(true);
	}
	
	public bool GetDigging() {
		return digging;
	}
	
}
