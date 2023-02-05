using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
	[Header("Prefabs and External Objects")]
	[SerializeField] private GameObject digUpPrefab;
	private TopGridManager gridManager;
	Animator animator;
	AudioSource audioSource;
	
	[Header("Settings")]
	public Vector2Int thisGrid;
	public bool spawnedByPlayerOne;
	
	[Header("Audio")]
	public AudioClip potatoFlash;
	public AudioClip potatoExplosion;
	
	
    // Start is called before the first frame update
    void Start() {     
		gridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		
        StartCoroutine(Explosion());
    }
	
	IEnumerator Explosion() {
		yield return new WaitForSeconds(0.01f);
		
		audioSource.clip = potatoFlash;
		audioSource.Play();
		
		yield return new WaitForSeconds(potatoFlash.length);
		
		audioSource.clip = potatoFlash;
		audioSource.Play();
		
		yield return new WaitForSeconds(potatoFlash.length);

		animator.SetBool("explode", true);
		audioSource.clip = potatoExplosion;
		audioSource.Play();
		
			for(int i=-1;i<2;i++)
				for(int j=-1;j<2;j++) {
					if(!(i == 0 && j == 0)) {
						Vector2Int gridToDig = thisGrid + new Vector2Int(i, j);
						if(!(gridToDig.x < 0 || gridToDig.y < 0 || gridToDig.x >= gridManager.GetGridSize().x || gridToDig.y >= gridManager.GetGridSize().y)) {
							
							if(!gridManager.GetTileByPosition(gridToDig).GetDugUp()) {
								Instantiate(digUpPrefab, PixelConversion.ConvertPixelPositionToWorldPosition((Vector2)gridToDig * gridManager.GetDistance()), Quaternion.identity);
								gridManager.GetTileByPosition(gridToDig).SetDugUp(true);
								
								if(gridManager.GetTileByPosition(gridToDig).IsMoleHidden()) {
									if(spawnedByPlayerOne)
										Application.LoadLevel("EndScreenP1");
									else
										Application.LoadLevel("EndScreenP2");
								} 
							}
						}
					}
				}
		
	}
}
