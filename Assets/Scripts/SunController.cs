using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] private Light sun;

    [SerializeField] private float minElevation = 20f;
    [SerializeField] private float maxElevation = 70f;

    [SerializeField] private float minAzimuth = 0f;
    [SerializeField] private float maxAzimuth = 360f;

    public void RandomizeSun()
    {
        float elevation = Random.Range(minElevation, maxElevation);
        float azimuth = Random.Range(minAzimuth, maxAzimuth);

        sun.transform.rotation = Quaternion.Euler(elevation, azimuth, 0f);
    }
}