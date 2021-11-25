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
    private float _raycastDistance = 10f;

    private bool _summoned;
    public bool Summoned
    {
        get => _summoned;
        set => _summoned = value;
    }

    public GameObject[] Shields;
    public Transform _playerTransform;

    public void SwitchMode()
    {
        _defenseMode = !_defenseMode;
    }

    public void Summon(Vector3 position)
    {
        //summoning the sand soldier
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, _raycastDistance))
        {
            Debug.DrawRay(position, Vector3.down, Color.yellow);
            Debug.Log("Did Hit");
            transform.position = hit.transform.position + Vector3.up * 2f;
        }
        else
        {
            Debug.DrawRay(position, Vector3.down, Color.red);
            Debug.Log("Did not Hit");
        }

        Summoned = true;
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
