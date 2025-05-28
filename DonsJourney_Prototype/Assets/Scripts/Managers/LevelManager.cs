using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] sectionPrefabs;
    private List<GameObject> spawnedSections;

    [Header("Section settings")]
    [SerializeField] private float parallaxSpeed;
    [SerializeField] private float xToDestroySection;
    public static float groundCeilingHeight { get; private set; } = 10.0f;
    public static float sectionHalfSize { get; private set; } = 5.0f;

    [Header("Assignables")]
    [SerializeField] private Transform groundCollider;

    private void Start()
    {
        spawnedSections = new List<GameObject>();

        groundCollider.position = groundCollider.position + new Vector3(0, -groundCeilingHeight, 0);

        for (uint i = 0; i < 5; i++)
            SpawnSection();
    }

    private void Update()
    {
        if (GameManager.currentState != GAMESTATE.PLAYING) return;
        if (spawnedSections.Count <= 0) return;

        for (int i = 0; i < spawnedSections.Count; i++)
        {
            Vector2 newPos = spawnedSections[i].transform.position;
            newPos.x -= parallaxSpeed * Time.deltaTime;

            if (newPos.x <= xToDestroySection)
            {
                Destroy(spawnedSections[i]);
                spawnedSections.RemoveAt(i);
                SpawnSection();
            }
            else
                spawnedSections[i].transform.position = newPos;
        }
    }

    private void SpawnSection()
    {
        Vector3 newPos = new Vector3(xToDestroySection + 0.1f, 0.0f, 0.0f);
        if (spawnedSections.Count > 0)
        {
            newPos = spawnedSections[spawnedSections.Count - 1].transform.position;
            newPos.x += sectionHalfSize * 2;
        }

        GameObject newObj = Instantiate(sectionPrefabs[UnityEngine.Random.Range(0, sectionPrefabs.Length)],
             newPos, quaternion.identity);
        spawnedSections.Add(newObj);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(xToDestroySection, 0.0f), 0.25f);
    }
}