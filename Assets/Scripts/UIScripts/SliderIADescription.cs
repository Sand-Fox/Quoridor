using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderIADescription : MonoBehaviour
{
    public TextMeshProUGUI TmpText;

    public void OnSliderChanged(float value)
    {
        if (value == 1) TmpText.text = IAMove.description;
        else if (value == 2) TmpText.text = IAWall.description;
        else if (value == 3) TmpText.text = IAMoveWall.description;
        else TmpText.text = "" + value;
    }
}
