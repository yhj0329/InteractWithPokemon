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
        text.text = "Use your touch\nto throw a ball\nat Pokemon";
        GameManager.instance.isRemove = true;
    }

    public void PokemonClick() {
        SelectUI.SetActive(false);
        text.text = "Click\non the plane\nto meet\nPokemon";
        GameManager.instance.arObjIndex = objIndex;
        GameManager.instance.isSelect = false;
    }
}
