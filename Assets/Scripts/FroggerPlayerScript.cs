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

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        anim = GetComponentInChildren<Animator>();
        canMove = true;
        isDead = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) PlayerUpdate();
    }

    void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            PlayerMove(Vector3.up);
            StartCoroutine(MoveTime());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            PlayerMove(Vector3.down);
            StartCoroutine(MoveTime());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            PlayerMove(Vector3.left);
            StartCoroutine(MoveTime());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            PlayerMove(Vector3.right);
            StartCoroutine(MoveTime());
        }
    }

    void PlayerMove(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;

        Collider2D _barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Barrier"));
        Collider2D _platform = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Platform"));
        

        if (_barrier != null) return;
        
;       if (_platform != null)
        {
            transform.SetParent(_platform.transform);
            onPlatform = true;
        }
        else
        {
            transform.SetParent(null);
            onPlatform = false;
        }

            StartCoroutine(SmoothMove(destination));
    }

    IEnumerator SmoothMove(Vector3 destination)
    {
        Vector3 startPos = transform.position;
        anim.SetTrigger("Hop");
        audioManager._audi.PlayOneShot(audioManager.soundFX[1]);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle" && !isDead) StartCoroutine(PlayerDeath(3));
        if (other.gameObject.tag == "River")
        {
            onRiver = true;
        }
        if (other.gameObject.tag == "KillVolume")
        {
            if (onRiver) StartCoroutine(PlayerDeath(2));
            else StartCoroutine(PlayerDeath(3));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "River")
        {
            onRiver = false;
        }
    }

    IEnumerator MoveTime()
    {
        canMove = false;
        yield return new WaitForSeconds(.175f);
        canMove = true;
        if (onRiver)
        {
            if (!onPlatform) StartCoroutine(PlayerDeath(2));
        }
    }

    IEnumerator PlayerDeath(int sfx)
    {
        isDead = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        anim.SetTrigger("Death");
        audioManager._audi.PlayOneShot(audioManager.soundFX[sfx]);
        yield return new WaitForSeconds(1.5f);
        gameManager.NewFrog();
        Destroy(gameObject);
    }
}
