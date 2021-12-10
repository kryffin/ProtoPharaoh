using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private GameManager() { }

    public Transform PlayerSpawn;
    public GameObject Player;
    public GameObject SandSoldier;
    public FadeToBlack FTB;

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

    public void PlayerHitSpike()
    {
        // Fade screen to black
        // Reposition player
        SandSoldier.SetActive(false);
        Player.transform.position = PlayerSpawn.position;
        FTB.Fade();
    }
}
