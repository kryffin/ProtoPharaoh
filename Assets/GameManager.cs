using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private GameManager() { }

    public Transform PlayerSpawn;
    public GameObject Player;
    public GameObject SandSoldier;
    public FadeToBlack FTB;

    private void Start()
    {
        Player = Instantiate(Player, PlayerSpawn.position, Quaternion.identity);

        SandSoldier = Instantiate(SandSoldier, Vector3.zero, Quaternion.identity);
        SandSoldier.SetActive(false);
        Player.GetComponent<PlayerMovement>().SandSoldier = SandSoldier.GetComponent<SandSoldierBehavior>();
        SandSoldier.GetComponent<SandSoldierBehavior>().Player = Player;
    }

    public void PlayerHitSpike()
    {
        // Fade screen to black
        // Reposition player
        SandSoldier.SetActive(false);
        Player.transform.position = PlayerSpawn.position;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(FTB.Fade());
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
