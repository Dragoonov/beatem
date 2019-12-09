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
    TournamentDataWrapper wrapper;
    UserManager user;
    bool ready;

    void Start()
    {
        user = GameObject.Find("User").GetComponent<UserManager>();
        ready = false;
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
        if(ready)
        {
            level.SetLevel(wrapper.levelWrapper.level);
            level.StartLevel();
            startingPanel.SetActive(false);
            ready = false;
        }
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
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Wszystko git");
            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void OnGenerate()
    {
        HideLayout();
        tournamentMessage.GetComponent<Text>().text = "Setting everything ready...";

        string email = user.GetLoggedUser().Email;

        wrapper = new TournamentDataWrapper(
            new Competitor(email.Substring(0, email.IndexOf('@')), false),
            new Competitor("guest", false),
            new LevelWrapper(level.GenerateLevel()));

        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .SetRawJsonValueAsync(JsonUtility.ToJson(wrapper));

    }

    public void OnFind()
    {
        HideLayout();
        tournamentMessage.GetComponent<Text>().text = "Downloading level...";
        FirebaseDatabase.DefaultInstance
            .GetReference("Tournaments")
            .Child("YYJckYrcZTP0I84iFiFEzTtTuYq2")
            .GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted)
              {
                    Debug.Log("Error heh");
              }
              else if (task.IsCompleted)
              {
                    Debug.Log("Jest dobrze");
                    DataSnapshot snapshot = task.Result;
                    wrapper = JsonUtility.FromJson<TournamentDataWrapper>(snapshot.GetRawJsonValue());
                    ready = true;
                }
            });
    }
}
