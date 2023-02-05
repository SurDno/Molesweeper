using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
	enum Direction {Down, Right, Up, Left};
	
	[Header("Prefabs and External Objects")]
	private TopGridManager gridManager;
	private CharacterDigging characterDigging;
	
	[Header("Settings")]
	[SerializeField] private bool controlledWithArrow;
	[SerializeField] private Vector2Int startGridPosition;
	
	[Header("Current Values")]
	private Vector2Int currentGridPosition;
	private Direction facingDirection;
	private bool moving;
	
	void Start() {
		currentGridPosition = startGridPosition;
		gridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
		characterDigging = GetComponent<CharacterDigging>();
	}
	
    void FixedUpdate() {
		// Check for input.
		if(controlledWithArrow) {
			if(Input.GetKey(KeyCode.UpArrow))
				Move(Direction.Up);
			else if(Input.GetKey(KeyCode.LeftArrow))
				Move(Direction.Left);
			else if(Input.GetKey(KeyCode.DownArrow))
				Move(Direction.Down);
			else if(Input.GetKey(KeyCode.RightArrow))
				Move(Direction.Right);
		} else {
			if(Input.GetKey(KeyCode.W))
				Move(Direction.Up);
			else if(Input.GetKey(KeyCode.A))
				Move(Direction.Left);
			else if(Input.GetKey(KeyCode.S))
				Move(Direction.Down);
			else if(Input.GetKey(KeyCode.D))
				Move(Direction.Right);
		}
    }
	
	private void Move(Direction newMovementDirection) {
		// Ignore all other movement commands if we're already moving.
		if(moving)
			return;
		
		// If we're trying to move while digging, don't move.
		if(characterDigging.GetDigging())
			return;
		
		// If we're not facing the direction we need to go, change facing direction and don't move.
		if(facingDirection != newMovementDirection) {
			FaceNewDirection(newMovementDirection);
			return;
		}
		
		// If we're trying to move against world border, don't move.
		if((newMovementDirection == Direction.Right && currentGridPosition.x + 1 == gridManager.GetGridSize().x)
			|| (newMovementDirection == Direction.Up && currentGridPosition.y + 1 == gridManager.GetGridSize().y)
			|| (newMovementDirection == Direction.Left && currentGridPosition.x == 0)
			|| (newMovementDirection == Direction.Down && currentGridPosition.y == 0))
			return;
		
		Vector2 increment = Vector2.zero;
		switch(newMovementDirection) {
			case Direction.Up:
				increment = Vector2.up;
				break;
			case Direction.Left:
				increment = Vector2.left;
				break;
			case Direction.Down:
				increment = Vector2.down;
				break;
			case Direction.Right:
				increment = Vector2.right;
				break;
		}
		
		StartCoroutine(LerpPos(currentGridPosition, currentGridPosition + Vector2Int.RoundToInt(increment), 0.16f));
	}
	
	private void FaceNewDirection(Direction newFacingDirection) {
		facingDirection = newFacingDirection;
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, (int)facingDirection * 90.0f);
		
	}
	
	public IEnumerator LerpPos(Vector2Int initialPos, Vector2Int newPos, float percentageIncrease) {
		moving = true;
		
		float percentage = 0;
		Vector2 worldInitialPos = PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)initialPos * gridManager.GetDistance());
		Vector2 worldNewPos = PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)newPos * gridManager.GetDistance());
		
		while(percentage <= 1.0f) {
			Vector2 tempPos = Vector2.Lerp(initialPos, newPos, percentage);
			tempPos = PixelConversion.SnapWorldPositionToPixelGrid(tempPos);
			transform.position = tempPos;
			
			percentage += percentageIncrease;
			yield return new WaitForSeconds(0.01f);
		}
		
		currentGridPosition = newPos;
		
		moving = false;
	}
	
	public Vector2Int GetCurrentGrid() {
		return currentGridPosition;
	}
	
	public Vector2Int GetFacingGrid() {
		Vector2 increment = Vector2.zero;
		switch(facingDirection) {
			case Direction.Up:
				increment = Vector2.up;
				break;
			case Direction.Left:
				increment = Vector2.left;
				break;
			case Direction.Down:
				increment = Vector2.down;
				break;
			case Direction.Right:
				increment = Vector2.right;
				break;
		}
		
		return Vector2Int.RoundToInt(currentGridPosition + Vector2Int.RoundToInt(increment));
	}
	
	public bool GetMovingValue() {
		return moving;
	}
	
	public int GetFacingDirectionAsInt() {
		return (int)facingDirection;
	}
	
}
