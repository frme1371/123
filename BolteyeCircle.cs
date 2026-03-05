using System.Collections;
using UnityEngine;

public class BolteyeCircle : MonoBehaviour
{
    public GameObject largeCirclePrefab;
    public GameObject mediumCirclePrefab;
    public GameObject smallCirclePrefab;

    private void Start()
    {
        StartCoroutine(SpawnCircles());
    }

    private IEnumerator SpawnCircles()
    {
        // Spawn large circle after 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        Instantiate(largeCirclePrefab, GetRandomPosition(), Quaternion.identity);

        // Spawn medium circle after 1 second
        yield return new WaitForSeconds(0.5f);
        Instantiate(mediumCirclePrefab, GetRandomPosition(), Quaternion.identity);

        // Spawn small circle after 1.5 seconds
        yield return new WaitForSeconds(0.5f);
        Instantiate(smallCirclePrefab, GetRandomPosition(), Quaternion.identity);
    }

    private Vector3 GetRandomPosition()
    {
        // Logic to determine a random position
        return new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
    }
}