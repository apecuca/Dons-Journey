using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSection : MonoBehaviour
{
    [SerializeField] private GameObject bubbleObj;

    private void OnEnable()
    {
        if (bubbleObj == null)
            return;

        if (!LevelManager.instance.CanSpawnBubble())
            bubbleObj.SetActive(false);
        else
            LevelManager.instance.OnBubbleSpawned();
    }
    private void OnDrawGizmos()
    {
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
