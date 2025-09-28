using UnityEngine;

public class ScoringZoneScript : MonoBehaviour
{
    public int zoneScore = 30;
    public bool zoneTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        zoneTrigger = false;
    }
}
