using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField]
    private SliderType slider;
    private enum SliderType { None, Volume, SensitivityX, SensitivityY }

    private Slider ui;

    private void Start()
    {
        ui = GetComponent<Slider>();
        switch (slider)
        {
            case SliderType.Volume:
                ui.minValue = Persistent.VolumeMin;
                ui.maxValue = Persistent.VolumeMax;
                ui.value = Persistent.Volume;
                break;
            case SliderType.SensitivityX:
                ui.minValue = Persistent.SensitivityMin;
                ui.maxValue = Persistent.SensitivityMax;
                ui.value = Persistent.SensitivityX;
                break;
            case SliderType.SensitivityY:
                ui.minValue = Persistent.SensitivityMin;
                ui.maxValue = Persistent.SensitivityMax;
                ui.value = Persistent.SensitivityY;
                break;
        }
    }

    private void Update()
    {
        switch (slider)
        {
            case SliderType.Volume:
                Persistent.Volume = ui.value;
                AudioListener.volume = Persistent.Volume / 100f;
                break;
            case SliderType.SensitivityX:
                Persistent.SensitivityX = ui.value;
                break;
            case SliderType.SensitivityY:
                Persistent.SensitivityY = ui.value;
                break;
        }
    }
}
