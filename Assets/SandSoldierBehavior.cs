using UnityEngine;

public class SandSoldierBehavior : MonoBehaviour
{

    enum ShieldPos
    {
        Up,
        Right,
        Left
    }

    private Transform _playerTransform;
    public Transform PlayerTransform
    {
        get => _playerTransform;
        set => _playerTransform = value;
    }

    private bool _defenseMode;
    private ShieldPos _currentShield;

    public GameObject[] Shields;

    public void SwitchMode()
    {
        _defenseMode = !_defenseMode;
    }

    private void Update()
    {
        if (!_defenseMode)
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
        else
        {
            Shields[(int)_currentShield].SetActive(false);
            Shields[(int)ShieldPos.Up].SetActive(true);
            _currentShield = ShieldPos.Up;
        }
    }
}
