using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeTime;

    private Image fadeOutUIImage;

    public enum FadeDirection
    {
        In,
        Out
    }

    private void Awake()
    {
        fadeOutUIImage = GetComponent<Image>();
    }

    public IEnumerator Fade(FadeDirection _fadeDirection)
    {
        float _alpha = _fadeDirection == FadeDirection.Out ? 1 : 0;
        float _fadeEndValue = _fadeDirection == FadeDirection.Out ? 0 : 1;

        // three parameters: checking fadedirection, alpha value , in and out
        // transition this scene to another scene
      
        if (_fadeDirection == FadeDirection.Out)
        {
            while (_alpha >= _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);

                yield return null;
                // set color image to alpha value ( sight and light)
            }

            fadeOutUIImage.enabled = false;
            // if fadeout is not enabled, it will be false
        }
        else
        {
            fadeOutUIImage.enabled = true;

            while (_alpha <= _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);

                yield return null;
            }
            // if fadeout is enabled, it will be true
        }
    }

    void SetColorImage(ref float _alpha, FadeDirection _fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, _alpha);

        _alpha += Time.deltaTime * (1 / fadeTime) * (_fadeDirection == FadeDirection.Out ? -1 : 1);
        // default alpha value, then add time at scene time in and out
        // setcolor function
    }
}