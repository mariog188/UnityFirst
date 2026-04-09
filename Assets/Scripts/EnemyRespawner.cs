using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [Space]
    [SerializeField] private float cooldownDecreaseRate = .05f;
    [SerializeField] private float cooldownCap = .7f;
    private float timer;

    private Transform player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 )
        {
            timer = cooldown;
            CreateNewEnemy();

            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        GameObject newEnemy = Instantiate(enemyPrefab, respawnPoints[respawnPointIndex].position, Quaternion.identity);

        bool createdOnRight = newEnemy.transform.position.x > player.transform.position.x;

        if (createdOnRight)
        {
            newEnemy.GetComponent<Enemy>().Flip();
        }
    }
}
