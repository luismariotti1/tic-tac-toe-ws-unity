using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFeedback : MonoBehaviour
{
    // there will be to colors in one array
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject opponent;
    
    private void Start()
    {
        var playerData = GameManager.Instance.player;
        // the first child is the text tmp
        var playerText = player.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Log(playerText.text);
        playerText.text = PlayerPrefs.GetString("username");
    }
}
