using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
    }

    public void NewFrog()
    {
        Instantiate(playerPrefab, startPoint.position, startPoint.rotation);
    }
}
