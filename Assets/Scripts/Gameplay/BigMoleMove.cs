using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMoleMove : MonoBehaviour
{
	public GameObject SpaceBar;
	
    float timer =0;
    float lerpPosition, moveSpeed = 0.5f;
    Vector3 currentPosition, targetPosition;
    bool moving;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCurrentPosition();
        targetPosition = new Vector3(22,4,0);
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(moving && timer > 2)
        {
            Move();
        }
    }

    void UpdateCurrentPosition()
    {
        int currentX = (int)transform.position.x;
        int currentY = (int)transform.position.y;
        currentPosition = new Vector3(currentX, currentY, 0);
    }

    void Move()
    {
        lerpPosition += Time.deltaTime * moveSpeed;
        transform.position = Vector3.Lerp(currentPosition, targetPosition, lerpPosition);

        if(lerpPosition >= 1)
        {
            moving = false;
            lerpPosition = 0;
            UpdateCurrentPosition();
			SpaceBar.SetActive(true);
        }
		
    }
}