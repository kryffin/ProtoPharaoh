using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    private float _fadeClock = -10f;
    private bool _fading;
    private Image _img;

    public float TimeToBlack = 1f;
    public Color ScreenColor;

	private void Start()
	{
        _img = GetComponent<Image>();
	}

	public void Fade()
    {
        _fadeClock = Time.time;
        _fading = true;
    }

	private void Update()
	{
        if (_fading)
        {
            if (Time.time < _fadeClock + TimeToBlack)
            {
                ScreenColor.a = ((_fadeClock + TimeToBlack) - Time.time) / TimeToBlack;
                _img.color = ScreenColor;
            }
            else
            {
                ScreenColor.a = 0f;
                _img.color = ScreenColor;

                _fading = false;
            }
        }
	}
}
