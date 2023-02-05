using UnityEngine;

public class Tile : MonoBehaviour {
	[Header("Prefabs and External Objects")]
	[SerializeField] private Mole mole;
	
	[Header("Current Values")]
	[SerializeField]private Vector2Int myPos;
    [SerializeField]private bool dugUp;
	
	void Start() {
		mole = GameObject.Find("Mole").GetComponent<Mole>();
	}
	
	public bool CanDig() {
		// Cant dig if there's a mole here showing itself.
		if(mole.GetOutside() && mole.GetCurrentPos() == myPos)
			return false;
		
		return true;
	}
	
	public void SetDugUp(bool newDugUp) {
		dugUp = newDugUp;
	}
	
	public bool GetDugUp() {
		return dugUp;
	}
	
	public void SetMyPos(Vector2Int newMyPos) {
		myPos = newMyPos;
	}
}
