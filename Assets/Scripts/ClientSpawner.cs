using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public GameObject[] clientPrefabs;
    public Vector2 spawnPosition = new Vector2(-10, 0);
    public Vector2 secondPositionTarget = new Vector2(-5, 0);
    public float spawnInterval = 8f;
    private float timer;
    private OrderCreator OC;

    void Start()
    {
        OC = FindAnyObjectByType<OrderCreator>();
        timer = spawnInterval;
    }

    void Update()
    {
        if (GameManager.isOver) return;

        if (OC != null && OC.CanRegister())
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpawnClient();
                spawnInterval = Random.Range(4f, 8f);
                timer = spawnInterval;
            }
        }
    }

    void SpawnClient()
    {
        int randomIndex = Random.Range(0,clientPrefabs.Length);
        GameObject clientPrefab = clientPrefabs[randomIndex];
        GameObject newClientObj = Instantiate(clientPrefab, spawnPosition, Quaternion.identity);
        ClientLogic clientLogic = newClientObj.GetComponent<ClientLogic>();
        if (clientLogic != null)
        {
            clientLogic.StandPosition = secondPositionTarget;
        }
    }
}
