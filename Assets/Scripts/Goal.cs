using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject NextPanel;
    //[SerializeField] private Button Nextbtn;
    readonly HelixController controller;


    private void Awake()
    {
        NextPanel.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        NextPanel.SetActive(true);
        Time.timeScale = 0f;
        //controller.enabled = false;
        //Nextbtn = NextPanel.GetComponentInChildren<Button>();
        //Nextbtn.onClick.AddListener(NextLevel);
        //
        Debug.Log("Next Level");

    }
    public void NextLevel()
    {
        GameManager.singleton.NextLevel();
        NextPanel.SetActive(false);
        Time.timeScale = 1f;
        //controller.enabled = true;
    }
}
