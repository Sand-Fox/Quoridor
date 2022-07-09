using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderIADescription : MonoBehaviour
{
    public int sliderID;
    public TextMeshProUGUI TmpText;

    private void Start(){
        if (sliderID == 1) OnSlider1Changed(1);
        if (sliderID == 2) OnSlider2Changed(1);
    }

    public void OnSlider1Changed(float value)
    {
        if (value == 1) 
        {
            TmpText.text = IAMove.description;
            SceneSetUpManager.IAName1 = "Units/IAMove";
        }
        else if (value == 2) 
        {
            TmpText.text = IAWall.description;
            SceneSetUpManager.IAName1 = "Units/IAWall";
        }
        else if (value == 3) 
        {
            TmpText.text = IAMoveWall.description;
            SceneSetUpManager.IAName1 = "Units/IAMoveWall";
        }
        else if (value == 4)
        {
            TmpText.text = IAMiniMax.description;
            SceneSetUpManager.IAName1 = "Units/IAMiniMax";
        }
        else if (value == 5)
        {
            TmpText.text = IAAlphaBeta.description;
            SceneSetUpManager.IAName1 = "Units/IAAlphaBeta";
        }
        else if (value == 6)
        {
            TmpText.text = IANegaMax.description;
            SceneSetUpManager.IAName2 = "Units/IANegaMax";
        }
        else TmpText.text = "" + value;
    }

    public void OnSlider2Changed(float value)
    {
        if (value == 1)
        {
            TmpText.text = IAMove.description;
            SceneSetUpManager.IAName2 = "Units/IAMove";
        }
        else if (value == 2)
        {
            TmpText.text = IAWall.description;
            SceneSetUpManager.IAName2 = "Units/IAWall";
        }
        else if (value == 3)
        {
            TmpText.text = IAMoveWall.description;
            SceneSetUpManager.IAName2 = "Units/IAMoveWall";
        }
        else if (value == 4)
        {
            TmpText.text = IAMiniMax.description;
            SceneSetUpManager.IAName2 = "Units/IAMiniMax";
        }
        else if (value == 5)
        {
            TmpText.text = IAAlphaBeta.description;
            SceneSetUpManager.IAName2 = "Units/IAAlphaBeta";
        }
        else if (value == 6)
        {
            TmpText.text = IANegaMax.description;
            SceneSetUpManager.IAName2 = "Units/IANegaMax";
        }
        else TmpText.text = "" + value;
    }
}