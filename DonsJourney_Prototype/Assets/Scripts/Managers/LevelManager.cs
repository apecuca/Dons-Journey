using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static float distance { get; private set; } = 0.0f;

    [SerializeField] private GameObject[] sectionPrefabs;
    private List<GameObject> spawnedSections;

    [Header("Difficulty settings")]
    [Tooltip("0.0083f = 0.5 difficulty per minute")]
    [SerializeField] private float diffIncreasePerSecond = 0.0083f;
    [SerializeField] private float maxDifficulty;
    public static float difficulty { get; private set; } = 0.8f;

    [Header("Section settings")]
    [SerializeField] private float parallaxSpeed;
    [SerializeField] private float xToDestroySection;
    public static float groundCeilingHeight { get; private set; } = 8.25f;
    public static float sectionHalfSize { get; private set; } = 12.5f;

    [Header("Assignables")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform groundCollider;

    public static LevelManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        // Starting stats
        difficulty = 0.8f;
        distance = 0.0f;

        spawnedSections = new List<GameObject>();

        groundCollider.position = player.position + new Vector3(0, -groundCeilingHeight, 0);

        for (uint i = 0; i < 3; i++)
            SpawnSection(true);
    }

    private void Update()
    {
        if (GameManager.currentState != GAMESTATE.PLAYING) return;
        if (spawnedSections.Count <= 0) return;

        for (int i = 0; i < spawnedSections.Count; i++)
        {
            Vector2 newPos = spawnedSections[i].transform.position;
            newPos.x -= parallaxSpeed * difficulty * Time.deltaTime;

            if (newPos.x <= xToDestroySection)
            {
                Destroy(spawnedSections[i]);
                spawnedSections.RemoveAt(i);
                SpawnSection();
            }
            else
                spawnedSections[i].transform.position = newPos;
        }

        // Increase distance travelled
        distance += parallaxSpeed * difficulty * Time.deltaTime;
        GameManager.instance.UpdateDistance(distance);

        // increase difficulty
        if (difficulty < maxDifficulty)
            difficulty += diffIncreasePerSecond * Time.deltaTime;
        else difficulty = maxDifficulty;
    }

    private void SpawnSection(bool startingSection = false)
    {
        Vector3 newPos = new Vector3(xToDestroySection + 0.1f, 0.0f, 0.0f);
        if (spawnedSections.Count > 0)
        {
            newPos = spawnedSections[spawnedSections.Count - 1].transform.position;
            newPos.x += sectionHalfSize * 2;
        }

        int selectedObj = startingSection ? 0 : UnityEngine.Random.Range(0, sectionPrefabs.Length);
        GameObject newObj = Instantiate(sectionPrefabs[selectedObj],
             newPos, quaternion.identity);
        spawnedSections.Add(newObj);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(xToDestroySection, 0.0f), 0.25f);
    }
}