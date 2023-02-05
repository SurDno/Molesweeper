using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour {
	[Header("Prefabs and External Objects")]
    TopGridManager topGridManager;
	Animator animator;
	AudioSource audioSource;
	
	[Header("Settings")]
	[SerializeField]private float moveDelay = 0.05f;
	[SerializeField]private float movingTime = 2f;
	[SerializeField]private float timeBeforePopup = 10f;
	
	[Header("Audio")]
	public AudioClip molePoppingOut;
	public AudioClip waving;
	public AudioClip moleLeaving;
	
	[Header("Current Values")]
    Vector2Int currentPos;
	private bool outside;
	private bool moving;
	
    void Start() {
		topGridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		
        currentPos = Vector2Int.RoundToInt(topGridManager.GetGridSize() / 2);
		transform.position = PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)currentPos * topGridManager.GetDistance());
		
		StartCoroutine(Stop());
		StartCoroutine(Move());
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
			
			moving = true;
			animator.SetBool("disappear", false);
			outside = false;
			
			yield return new WaitForSeconds(movingTime);
			
			moving = false;
			
			yield return new WaitForSeconds(timeBeforePopup);
		}
	}
	
	IEnumerator Move() {
		while(true) { 
			yield return new WaitForSeconds(moveDelay);
			if(moving)
				TryMove();
		}
	}

    void TryMove() {
		Vector2Int possiblePos = currentPos + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
		
		if(possiblePos.x < 0 || possiblePos.y < 0 || possiblePos.x >= topGridManager.GetGridSize().x || possiblePos.y >= topGridManager.GetGridSize().y)
			return;
		
		if(topGridManager.GetTileByPosition(possiblePos).GetDugUp())
			return;
		
		currentPos = possiblePos;
		transform.position = PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)currentPos * topGridManager.GetDistance());
	}
	
	public bool GetOutside() {
		return outside;
	}
	
	public Vector2Int GetCurrentPos() {
		return currentPos;
	}
}
