using UnityEngine;

public class SandSoldierBehavior : MonoBehaviour
{

    enum ShieldPos
    {
        Up,
        Right,
        Left
    }

    private bool _defenseMode;
    private ShieldPos _currentShield;

    public GameObject[] Shields;
    public Transform _playerTransform;

    public void DefenseMode()
    {
        _defenseMode = true;
    }

    public void PlatformMode()
    {
        _defenseMode = false;
    }

    public void Summon()
    {
        transform.position = _playerTransform.position + Vector3.right * 2f;
        gameObject.SetActive(true);
    }

	private void Update()
    {
        if (_defenseMode)
        {
            if (_playerTransform.position.x < transform.position.x)
            {
                Shields[(int)_currentShield].SetActive(false);
                Shields[(int)ShieldPos.Right].SetActive(true);
                _currentShield = ShieldPos.Right;
            }
            else
            {
                Shields[(int)_currentShield].SetActive(false);
                Shields[(int)ShieldPos.Left].SetActive(true);
                _currentShield = ShieldPos.Left;
            }
        }
        else
        {
            Shields[(int)_currentShield].SetActive(false);
            Shields[(int)ShieldPos.Up].SetActive(true);
            _currentShield = ShieldPos.Up;
        }
    }
}
