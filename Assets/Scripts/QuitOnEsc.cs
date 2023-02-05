using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnEsc : MonoBehaviour {
    // Start is called before the first frame update
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
    }
}
