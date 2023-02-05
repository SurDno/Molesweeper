using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class CharacterDigging : MonoBehaviour {
	[Header("Prefabs and External Objects")]
	[SerializeField] private GameObject digUpPrefab;
	[SerializeField] private GameObject potatoPrefab;
	[SerializeField] private GameObject beetrootPrefab;
	[SerializeField] private AudioMixer audioMixer;
	private CharacterMovement characterMovement;
	private TopGridManager gridManager;
	private Mole mole;
	
	AudioSource audioSource;
	
	[Header("Settings")]
	[SerializeField] private bool controlledWithArrow;
	[SerializeField] private float timeToDig;
	
	[Header("Audio")]
	[SerializeField] private AudioClip diggingSound;
	[SerializeField] private AudioClip diggingError;
	[SerializeField] private float minPitchValue = 0.5f;
	[SerializeField] private float maxPitchValue = 2.0f;
	
	[Header("Current Values")]
	[SerializeField] private bool digging;
	private Coroutine diggingCoroutine;
	
	void Start() {
		characterMovement = GetComponent<CharacterMovement>();
		gridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
		audioSource = GetComponent<AudioSource>();
		mole = GameObject.Find("Mole").GetComponent<Mole>();
		
		
		StartCoroutine(PlayDigSoundWhileDigging());
	}
	
	void Update() {
		// Check for input.
		if(controlledWithArrow) {
			if(Input.GetKeyDown(KeyCode.Return))
				Dig();
			if(diggingCoroutine != null && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))) {
				StopAllCoroutines();
				StartCoroutine(PlayDigSoundWhileDigging());
				digging = false;
			}
		} else {
			if(Input.GetKeyDown(KeyCode.Space))
				Dig();
			if(diggingCoroutine != null && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))) {
				StopAllCoroutines();
				StartCoroutine(PlayDigSoundWhileDigging());
				digging = false;
			}
		}
	}
	
	void Dig() {
		Vector2Int gridToDig = characterMovement.GetFacingGrid();
		
		// If we're trying to dig outside of world border, ignore digging.
		if((gridToDig.x >= gridManager.GetGridSize().x)
			|| (gridToDig.y >= gridManager.GetGridSize().y)
			|| (gridToDig.x < 0)
			|| (gridToDig.y < 0)) {
			if(!audioSource.isPlaying) {
				audioMixer.SetFloat("pitch", 1.0f);
				audioSource.clip = diggingError;
				audioSource.Play();
			}
			return;
		}
		
		// If we're moving, we can't dig.
		if(characterMovement.GetMovingValue()) {
			return;
		}
		
		// If we already dug this up, ignore digging.
		if(gridManager.GetTileByPosition(gridToDig).GetDugUp()) {	
			if(!audioSource.isPlaying) {
				audioMixer.SetFloat("pitch", 1.0f);
				audioSource.clip = diggingError;
				audioSource.Play();
			}
			return;
		}
		
		// If there's a mole, ignore digging.
		if(!gridManager.GetTileByPosition(gridToDig).IsMoleShowing()) {		
			if(!audioSource.isPlaying) {
				audioMixer.SetFloat("pitch", 1.0f);
				audioSource.clip = diggingError;
				audioSource.Play();
			}
			return;	
		}
		
		diggingCoroutine = StartCoroutine(DigAHole(gridToDig));
	}
	
	IEnumerator DigAHole(Vector2Int gridToDig) {
		digging = true;
		
		yield return new WaitForSeconds(timeToDig);
		
		digging = false;
		diggingCoroutine = null;
		
		// If we already dug this up, ignore digging.
		if(gridManager.GetTileByPosition(gridToDig).GetDugUp())
			yield break;
		
		// If there's a mole, ignore digging.
		if(!gridManager.GetTileByPosition(gridToDig).IsMoleShowing())	
			yield break;
		
		if(gridManager.GetTileByPosition(gridToDig).IsMoleHidden()) {
			Debug.Log(this.gameObject.name + " won!");
			// Mole appears, game is won. GG!
		} else {
			int rootProb = Random.Range(1, 100);
			GameObject prefabToSpawn = new GameObject();
			bool spawn = false;
			
			prefabToSpawn = beetrootPrefab;
			spawn = true; 
			
			if(spawn) {
				GameObject newObj = Instantiate(prefabToSpawn, PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)gridToDig * gridManager.GetDistance()), Quaternion.identity);
				if(newObj.GetComponent<Beetroot>()) {
					newObj.GetComponent<Beetroot>().characterMovement = characterMovement;
					newObj.transform.rotation = Quaternion.Euler(0, 0, characterMovement.GetFacingDirectionAsInt() * 90 - 180);
				}
			}
		}
		
		Instantiate(digUpPrefab, PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)gridToDig * gridManager.GetDistance()), Quaternion.identity);
		gridManager.GetTileByPosition(gridToDig).SetDugUp(true);
	}
	
	IEnumerator PlayDigSoundWhileDigging() {
		while(true) {
			if(digging && !audioSource.isPlaying) {
				float dist = Vector3.Distance(mole.gameObject.transform.position, gridManager.GetTileByPosition(characterMovement.GetFacingGrid()).gameObject.transform.position);
				float newPitchValue = minPitchValue + (maxPitchValue - (maxPitchValue - minPitchValue) * (dist / 23f));
				newPitchValue = Mathf.Clamp(newPitchValue, 0.5f, 2.0f);
				audioMixer.SetFloat("pitch", newPitchValue);
				audioSource.clip = diggingSound;
				audioSource.Play();
				yield return new WaitForSeconds(diggingSound.length);
			}
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	public bool GetDigging() {
		return digging;
	}
	
}
