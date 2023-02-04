using UnityEngine;

public class Tile : MonoBehaviour {
    [SerializeField]private bool dugUp;
	
	public void SetDugUp(bool newDugUp) {
		dugUp = newDugUp;
	}
	
	public bool GetDugUp() {
		return dugUp;
	}
}
