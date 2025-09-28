using System.Collections;
using UnityEngine;

public class FroggerPlayerScript : MonoBehaviour
{
    AudioManager audioManager;
    GameManager gameManager;

    [SerializeField] private bool isDead;
    [SerializeField] private bool onRiver;
    [SerializeField] private bool onPlatform;

    [SerializeField] private bool canMove;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        anim = GetComponentInChildren<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerUpdate();
    }

    void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            PlayerMove(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            PlayerMove(Vector3.down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            PlayerMove(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            PlayerMove(Vector3.right);
        }
    }

    void PlayerMove(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;

        Collider2D _barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Barrier"));
        Collider2D _platform = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Platform"));

        if (_barrier != null) return;

        StartCoroutine(SmoothMove(destination));
    }

    IEnumerator SmoothMove(Vector3 destination)
    {
        Vector3 startPos = transform.position;
        anim.SetTrigger("Hop");

        float elapsed = 0f;
        float duration = .125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }

    IEnumerator MoveTime()
    {
        canMove = false;
        yield return new WaitForSeconds(.175f);
        canMove = true;
    }
}
