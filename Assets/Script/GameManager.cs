using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isRemove = false;
    public int arObjIndex = 0;
    public GameObject[] arObject;
    public GameObject selectUI;
    public TextMeshProUGUI textUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PoketballHit() {
        selectUI.SetActive(true);
        textUI.text = "Select Pokemon";
    }
}