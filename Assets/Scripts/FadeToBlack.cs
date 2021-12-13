using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    private Image _img;

    public float TimeToClear = 1f;
    public Color ScreenColor;

	private void Start()
	{
        _img = GetComponent<Image>();
	}

    public IEnumerator Fade()
    {
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.01f)
        {
            ScreenColor.a = alpha;
            _img.color = ScreenColor;
            yield return new WaitForSeconds(TimeToClear / 100f);
        }
    }
}
