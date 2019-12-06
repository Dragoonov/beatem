using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Action
{
    LoadLevel,
    LoadUIElement
}

public class SceneLoader : MonoBehaviour
{
    public Button button;
    public string levelName;
    public string UIElementName;
    public Action action;
    void Start()
    {
        if (action == Action.LoadLevel)
        {
            button.GetComponent<Button>().onClick.AddListener(LoadLevel);
        }
        else if (action == Action.LoadUIElement)
        {
            button.GetComponent<Button>().onClick.AddListener(LoadUI);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeUIs()
    {
        GameObject.FindGameObjectWithTag("MainMenu").SetActive(true);
        GameObject.FindGameObjectWithTag("LevelList").SetActive(false);
    }

    private void ClearUIs()
    {
        GameObject.FindGameObjectWithTag("MainMenu").SetActive(false);
        GameObject.FindGameObjectWithTag("LevelList").SetActive(false);
    }

    public void LoadLevel()
    {
        if(levelName.Equals("Exit"))
        {
            Debug.Log("Exit");
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    public void LoadUI()
    {
        ClearUIs();
        GameObject.FindGameObjectWithTag(UIElementName).SetActive(true);
    }
}
