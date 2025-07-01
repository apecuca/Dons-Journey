using System.Collections.Generic;
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
    [SerializeField] private float xToDestroySection;
    [SerializeField] private float parallaxSpeed = 7.0f;
    public const float groundCeilingHeight = 8.25f;
    public const float sectionHalfSize = 12.5f;
    private float bubbleSpawnTimer = 0.0f;
    private float bubbleSpawnCD = 15.0f;

    [Header("Shark settings")]
    [SerializeField] private float sharkSpawnCD = 24.0f;
    [SerializeField] private float sharkSpawnTimer = 0.0f;
    private float sharkRandTimerMargin = 2.5f;

    [Header("Assignables")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform groundCollider;
    [SerializeField] private GameObject sharkPrefab;

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
        bubbleSpawnTimer = bubbleSpawnCD;
        ResetSharkTimer();

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

        // Bubble spawn timer
        if (bubbleSpawnTimer > 0.0f)
            bubbleSpawnTimer -= 1.0f * Time.deltaTime;

        // Shark spawn timer
        if (sharkSpawnTimer > 0.0f)
            sharkSpawnTimer -= 1.0f * Time.deltaTime;
        else
        {
            ResetSharkTimer();
            SpawnShark();
        }
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

    public bool CanSpawnBubble()
    {
        if (PlayerStats.instance.bubbled) return false;
        if (bubbleSpawnTimer > 0.0f) return false;
        return true;
    }

    public void OnBubbleDestroyed()
    {
        bubbleSpawnTimer = bubbleSpawnCD * 2;
    }

    public void OnBubbleSpawned()
    {
        bubbleSpawnTimer = bubbleSpawnCD;
    }

    public void SpawnShark()
    {
        float yRange = groundCeilingHeight - 2.0f;
        float targetY = UnityEngine.Random.Range(-yRange, yRange);
        float targetX = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)).x + 6.0f;

        Shark newShark = Instantiate(
            sharkPrefab, 
            new Vector3(targetX, targetY, 0.0f), 
            Quaternion.identity
                ).GetComponent<Shark>();

        newShark.InitShark(-targetX);
    }

    private void ResetSharkTimer()
    {
        sharkSpawnTimer = sharkSpawnCD + ((sharkSpawnCD - (sharkSpawnCD * difficulty)) / 2);
        sharkSpawnTimer += UnityEngine.Random.Range(-sharkRandTimerMargin, sharkRandTimerMargin);

        // 27 = valor inicial
        // 0.8 = dificuldade do momento
        // 27 + ((27 - (27 * 0.8)) / 2)
        // 27 + ((27 - 21.6) / 2)
        // 27 + (5.4 / 2) = 29.7

        // 27 + ((27 - (27 * 2)) / 2)
        // 27 + ((27 - 54) / 2)
        // 27 + (-27 / 2) = 13.5
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(xToDestroySection, 0.0f), 0.25f);
    }
}