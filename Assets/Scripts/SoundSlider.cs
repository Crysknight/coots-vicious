using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Start()
    {
        slider.value = CootsGameManager.Instance.soundVolume;
    }

    public void OnValueChanged(float value) {
        CootsGameManager.Instance.SetSoundVolume(value);
    }
}
