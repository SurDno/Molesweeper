using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
    TopGridManager topGridManager;
    [SerializeField] private GameObject digUpPrefab;
    // Start is called before the first frame update
    void Start()
    {     
        //needs a delay, Matt you do it :)
        Explosion();
    }

    // Update is called once per frame
    void Explosion()
    {
        for(int i=-1;i<2;i++)
        {
            for(int j=1;j>-2;j--)
            {
               // Instantiate(digUpPrefab, new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j), Quaternion.identity);
               // topGridManager.GetTileByPosition(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j).SetDugup();
            }
        }
    }
}
