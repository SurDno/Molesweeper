using UnityEngine;
public class Beetroot : MonoBehaviour {
	[Header("Prefabs and External Objects")]
	private TopGridManager gridManager;
	private CharacterMovement characterMovement;
	
    // Start is called before the first frame update
    void Start() {
		gridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
        characterMovement = GameObject.Find("Player1").GetComponent<CharacterMovement>();
		
    }
	
	void FixedUpdate() {
		if(!Input.GetKey(KeyCode.U))
			return;
		
		Vector2Int sentPlayerPosition = characterMovement.GetFacingGrid();
		
        switch(characterMovement.GetFacingDirectionAsInt())
        {
            case 0:
                sentPlayerPosition.y = (int)(gridManager.GetGridSize().y) - 1;
                break;
            case 1:
                sentPlayerPosition.x = 0;
                break;
            case 2:
                sentPlayerPosition.y = 0;
                break;
            case 3:
                sentPlayerPosition.x = (int)(gridManager.GetGridSize().x) - 1;
                break;
        }
		
		characterMovement.StartCoroutine(characterMovement.LerpPos(characterMovement.GetCurrentGrid(), sentPlayerPosition, 0.01f));
	}

}
