using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private float moveSpeed = 8f;
    private float xToDestroy;

    public void InitShark(float _xToDestroy)
    {
        xToDestroy = _xToDestroy;
    }

    private void Update()
    {
        if (GameManager.currentState != GAMESTATE.PLAYING)
            return;

        transform.position -= new Vector3(
            moveSpeed * LevelManager.difficulty * Time.deltaTime,
            0.0f,
            0.0f);

        if (transform.position.x <= xToDestroy)
            Destroy(this.gameObject);
    }
}
