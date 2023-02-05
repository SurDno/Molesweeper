using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Beetroot : MonoBehaviour {
	[Header("Prefabs and External Objects")]
	private TopGridManager gridManager;
	public CharacterMovement characterMovement;
	Animator animator;
	AudioSource audioSource;
	
	[Header("Audio")]
	public AudioClip beatrootAppear;
	public AudioClip beatrootPunch;
	public AudioClip beatrootDisappear;
	
    // Start is called before the first frame update
    void Start() {
		gridManager = GameObject.Find("Tile Generator").GetComponent<TopGridManager>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		
		StartCoroutine(Punch());
    }
	
	IEnumerator Punch() {
		audioSource.clip = beatrootAppear;
		audioSource.Play();
		
		yield return new WaitForSeconds(beatrootAppear.length);

		animator.SetBool("punch", true);
		audioSource.clip = beatrootPunch;
		audioSource.Play();
		
		yield return new WaitForSeconds(0.2f);
		
		Vector2Int sentPlayerPosition = characterMovement.GetFacingGrid();
		
        switch(characterMovement.GetFacingDirectionAsInt()) {
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
		
		yield return new WaitForSeconds(1f);
		
		animator.SetBool("disappear", true);
		audioSource.clip = beatrootDisappear;
		audioSource.Play();
	}

}
