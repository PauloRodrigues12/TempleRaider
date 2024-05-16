using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeedText : MonoBehaviour
{
    WorldGenerator worldGenerator;

    public TextMeshProUGUI seedText;

    void Update()
    {
        Debug.Log(seedText);
        seedText.text = "" + worldGenerator.seedString.ToString();
    }
}
