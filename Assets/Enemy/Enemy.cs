using UnityEngine;
using UnityEngine.AI;

public class Enemy
{
    public GameObject enemy;

    private int rows, columns;

    private Vector3 currentPos;
    private NavMeshAgent agent;
    private NavMeshPath path;
    private Vector3 targetPos;
    private bool targetFound;

    // vision - Omniscience
    private float visionDistanceLimit;

    public Enemy(GameObject _enemy, int _number)
    {
        Game game = GameObject.Find("Game Generator").GetComponent<Game>();
        rows = game.rows;
        columns = game.columns;

        // Initialization
        Vector3 randomPos = RandomPos();
        enemy = GameObject.Instantiate(_enemy, randomPos, Quaternion.identity);
        enemy.name = "Enemy " + _number;
        enemy.transform.parent = GameObject.Find("Enemies").transform;

        currentPos = enemy.transform.position;

        enemy.AddComponent<NavMeshAgent>();
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.radius = 0.55f;

        path = new NavMeshPath();

        // Initialize target at a random position
        targetPos = RandomPos();
        // While the path from agent to target is not found, generate a new random position
        while (!pathIsValid(targetPos))
        {
            targetPos = RandomPos();
        }
        agent.SetDestination(targetPos);
        targetFound = false;

        // Initialize view
        visionDistanceLimit = 10f;
    }

    // Update when there is no player object
    public void Update()
    {
        // Update position
        currentPos = enemy.transform.position;

        // If the agent reaches its random target position, generate a new random target position
        if (ReachTarget())
        {
            agent.ResetPath();
            targetPos = RandomPos();
            // While the path from agent to target is not found, generate a new random position
            while (!pathIsValid(targetPos))
            {
                targetPos = RandomPos();
            }
            agent.SetDestination(targetPos);
        }
    }

    // Update when there is a player object
    public void Update(GameObject player)
    {
        // Update position
        currentPos = enemy.transform.position;

        // Update visible targets
        targetFound = CanSee(player);

        // Get target and move towards target
        if (targetFound && pathIsValid(player.transform.position))
        {
            targetPos = player.transform.position;
            agent.SetDestination(targetPos);
        }
        else
        {
            // If the agent reaches its random target position, generate a new random target position
            if (ReachTarget())
            {
                agent.ResetPath();
                targetPos = RandomPos();
                // While the path from agent to target is not found, generate a new random position
                while (!pathIsValid(targetPos))
                {
                    targetPos = RandomPos();
                }
                agent.SetDestination(targetPos);
            }
        }
    }

    public bool ReachPlayer(GameObject player)
    {
        float distanceToTarget = Vector3.Distance(currentPos, player.transform.position);
        return distanceToTarget <= enemy.transform.localScale.x + player.transform.localScale.x;
    }

    public void DestroyObject()
    {
        if (enemy != null)
        {
            GameObject.Destroy(enemy);
        }
    }

    private bool pathIsValid(Vector3 pos)
    {
        agent.CalculatePath(pos, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    private Vector3 RandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(-1.5f, 2.8f * rows - 1.5f), 0f, Random.Range(-1.5f, 2.8f * columns - 1.5f));
        // Keep the enemies initial position away from the player's initial position
        while (Vector3.Distance(randomPos, Vector3.zero) <= 10f)
        {
            randomPos = new Vector3(Random.Range(-1.5f, 2.8f * rows - 1.5f), 0f, Random.Range(-1.5f, 2.8f * columns - 1.5f));
        }
        return randomPos;
    }

    private bool CanSee(GameObject obj)
    {
        Vector3 directToTarget = (obj.transform.position - currentPos).normalized;
        float distanceToTarget = Vector3.Distance(currentPos, obj.transform.position);

        return (distanceToTarget <= visionDistanceLimit);
    }

    private bool ReachTarget()
    {
        return currentPos == targetPos || (currentPos.x >= targetPos.x - 1.5f && currentPos.x <= targetPos.x + 1.5f
        && currentPos.z >= targetPos.z - 1.5f && currentPos.z <= targetPos.z + 1.5f);
    }
}
