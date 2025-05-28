using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSection : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (GameManager.currentState != GAMESTATE.IDLE)
            return;

        Vector2 drawPos = transform.position;

        // Section boundaries
        drawPos.y = -LevelManager.groundCeilingHeight;
        drawPos.x -= LevelManager.sectionHalfSize;
        Gizmos.DrawWireSphere(drawPos, 0.25f);
        drawPos.y = LevelManager.groundCeilingHeight;
        drawPos.x += LevelManager.sectionHalfSize * 2;
        Gizmos.DrawWireSphere(drawPos, 0.25f);
    }
}
