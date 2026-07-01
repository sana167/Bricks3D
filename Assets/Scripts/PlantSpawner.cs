using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] plantPrefabs;

    [SerializeField] private int plantCount = 50;

    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;

    [SerializeField] private float minZ = -20f;
    [SerializeField] private float maxZ = 20f;

    [SerializeField] private float exclusionRadius = 8f;

    [SerializeField] private Transform foliage;

    private void Start()
    {
        if (plantPrefabs == null || plantPrefabs.Length == 0)
        {
            Debug.LogWarning("No plant prefabs assigned to PlantSpawner.");
            return;
        }
        for (int i = 0; i < plantCount; i++)
        {
            SpawnPlant();
        }
    }

    private void SpawnPlant()
    {
        Vector3 pos;

        do
        {
            pos = new Vector3(
                Random.Range(minX, maxX),
                0f,
                Random.Range(minZ, maxZ));

        } while (pos.magnitude < exclusionRadius);

        GameObject plant = Instantiate(
            plantPrefabs[Random.Range(0, plantPrefabs.Length)],
            pos,
            Quaternion.Euler(0, Random.Range(0f, 360f), 0),
            foliage);

        float scale = Random.Range(1f, 4f);
        plant.transform.localScale *= scale;
    }
}