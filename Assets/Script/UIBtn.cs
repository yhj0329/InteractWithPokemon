using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class UIBtn : MonoBehaviour
{
    public GameObject Btn;
    public GameObject SelectUI;
    public TextMeshProUGUI text;
    public int objIndex = 0;

    public void PoketballClick() {
        Btn.SetActive(false);
        text.text = "Use your touch to throw balls at Pokemon";
        GameManager.instance.isRemove = true;
    }

    public void PokemonClick() {
        SelectUI.SetActive(false);
        text.text = "Click on the plane to meet Pokémon";
        GameManager.instance.arObjIndex = objIndex;
        GameManager.instance.isSelect = false;
    }
}
