using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderIADescription : MonoBehaviour
{
    public TextMeshProUGUI TmpText;

    private void Start(){
        OnSliderChanged(1);
    }

    public void OnSliderChanged(float value)
    {
        if (value == 1) 
        {
            TmpText.text = IAMove.description;
            PlayerSetUpManager.IAName = "Units/IAMove";
        }
        else if (value == 2) 
        {
            TmpText.text = IAWall.description;
            PlayerSetUpManager.IAName = "Units/IAWall";
        }
        else if (value == 3) 
        {
            TmpText.text = IAMoveWall.description;
            PlayerSetUpManager.IAName = "Units/IAMoveWall";
        }
        else if (value == 4)
        {
            TmpText.text = "IA MINMAX";//IAMiniMax.description;
            PlayerSetUpManager.IAName = "Units/IAMiniMax";
        }
        else TmpText.text = "" + value;
    }
}
