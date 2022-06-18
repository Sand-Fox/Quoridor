using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ScrollbarScript : MonoBehaviour
{
    [SerializeField] private GameObject partiePanelPrefab;

    private void Start()
    {
        foreach(FileInfo file in SaveSystem.GetFiles())
        {
            GameObject partiePanel = Instantiate(partiePanelPrefab, transform);
            partiePanel.GetComponentInChildren<TextMeshProUGUI>().text = file.Name;
        }
    }
}
