using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseConnector : MonoBehaviour
{
    // Start is called before the first frame update

    GenerateTournamentLevel level;
    DatabaseReference reference;
    GameObject startingPanel;
    GameObject tournamentMessage;
    GameObject generateButton;
    GameObject findButton;
    GameObject backButton;

    void Start()
    {
        checkFirebaseVersion();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://beatem-d1472.firebaseio.com/");
        level = GameObject.Find("LevelGenerator").GetComponent<GenerateTournamentLevel>();
        startingPanel = GameObject.Find("StartingPanel");
        tournamentMessage = GameObject.Find("TournamentMessage");
        generateButton = GameObject.Find("GenerateButton");
        findButton = GameObject.Find("FindButton");
        backButton = GameObject.Find("BackButton");
        tournamentMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideLayout()
    {
        tournamentMessage.SetActive(true);
        generateButton.SetActive(false);
        findButton.SetActive(false);
        backButton.SetActive(false);
    }

    private void checkFirebaseVersion()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void OnGenerate()
    {
        HideLayout();
        tournamentMessage.GetComponent<Text>().text = "Setting everything ready...";
        reference.SetRawJsonValueAsync(JsonUtility.ToJson(level.GenerateLevel()));

    }

    public void OnFind()
    {
        HideLayout(); 
        level.GenerateLevel();
        level.StartLevel();
        startingPanel.SetActive(false);
        //find level
    }
}
