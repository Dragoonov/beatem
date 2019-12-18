using System;
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
    GameObject readyPanel;
    GameObject tournamentMessage;
    GameObject generateButton;
    GameObject findButton;
    GameObject backButton;
    TournamentDataWrapper wrapper;
    UserManager user;
    bool ready;
    bool competitorReady;
    string competitor;
    bool showReadyPanel;
    string lvlAddress = "PGlvVo316taJC96GF8lJuUkDHtG2";
    string playerRole;
    Text obstaclesTravelled;
    int enemyObstaclesTravelledAmount;

    void Start()
    {
        obstaclesTravelled = GameObject.Find("TravelledObstacles").GetComponent<Text>();
        enemyObstaclesTravelledAmount = 0;
        readyPanel = GameObject.Find("ReadyPanel");
        readyPanel.SetActive(false);
        user = GameObject.Find("User").GetComponent<UserManager>();
        ready = false;
        competitorReady = false;
        checkFirebaseVersion();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://beatem-d1472.firebaseio.com/");
        level = GameObject.Find("LevelGenerator").GetComponent<GenerateTournamentLevel>();
        startingPanel = GameObject.Find("StartingPanel");
        tournamentMessage = GameObject.Find("TournamentMessage");
        generateButton = GameObject.Find("GenerateButton");
        findButton = GameObject.Find("FindButton");
        backButton = GameObject.Find("BackButton");
        tournamentMessage.SetActive(false);

        if (user.GetLoggedUser().UserId.Equals(lvlAddress))
        {
            playerRole = "creator";
        }
        else
        {
            playerRole = "guest";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showReadyPanel)
        {
            readyPanel.SetActive(true);
        }
        UpdateCompetitorNameLabel();
        if(ready && competitorReady)
        {
            if(playerRole == "guest")
            {
                level.SetLevel(wrapper.levelWrapper.level);
            }
            level.StartLevel();
            startingPanel.SetActive(false);
            readyPanel.SetActive(false);
            ready = false;
            showReadyPanel = false;
        }
    }

    private void UpdateCompetitorNameLabel()
    {
        if(competitor != null && !competitor.Equals("") && readyPanel.activeInHierarchy == true)
        {
            GameObject.Find("CompetitorName").GetComponent<Text>().text = competitor + " will be your enemy";
            competitor = "";
        }
    }

    public void OnTravelObstacle()
    {
        enemyObstaclesTravelledAmount++;
        Dictionary<string, object> obstacleUpdate = new Dictionary<string, object>();
        obstacleUpdate["/Tournaments/" + lvlAddress + "/" + playerRole + "/obstaclesTravelled"] = enemyObstaclesTravelledAmount;
        reference.UpdateChildrenAsync(obstacleUpdate);
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
            new Competitor(email.Substring(0, email.IndexOf('@')), false, 0),
            new Competitor("guest", false, 0),
            new LevelWrapper(level.GenerateLevel()));

        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .SetRawJsonValueAsync(JsonUtility.ToJson(wrapper));

        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .Child("guest")
            .Child("name").ValueChanged += HandleGuestJoined;

        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .Child("guest")
            .Child("ready").ValueChanged += HandleCompetitorReady;

        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .Child("guest")
            .Child("obstaclesTravelled").ValueChanged += HandleCompetitorTravelledObstacle;

        tournamentMessage.GetComponent<Text>().text = "Waiting for opponent...";
    }

    void HandleGuestJoined(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        competitor = (string)args.Snapshot.GetValue(false);
        if(competitor != "guest")
        {
        startingPanel.SetActive(false);
        readyPanel.SetActive(true);
        }
    }

    void HandleCompetitorTravelledObstacle(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
         obstaclesTravelled.text = "Enemy rings: " + args.Snapshot.GetValue(false);
    }

    void HandleCompetitorReady(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        competitorReady = (bool)args.Snapshot.GetValue(false);
    }

    public void OnFind()
    {
        HideLayout();
        tournamentMessage.GetComponent<Text>().text = "Downloading level...";
        FirebaseDatabase.DefaultInstance
            .GetReference("Tournaments")
            .Child(lvlAddress)
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
                    competitor = wrapper.creator.name;
                    JoinMatch();
               }
            });
    }

    private void JoinMatch()
    {
        Dictionary<string, object> nameUpdate = new Dictionary<string, object>();
        string email = user.GetLoggedUser().Email;
        nameUpdate["/Tournaments/" + lvlAddress + "/guest/name"] = email.Substring(0, email.IndexOf('@'));
        reference.UpdateChildrenAsync(nameUpdate);
        showReadyPanel = true;
        reference.Child("Tournaments")
            .Child(lvlAddress)
            .Child("creator")
            .Child("ready").ValueChanged += HandleCompetitorReady;

        reference.Child("Tournaments")
            .Child(lvlAddress)
            .Child("creator")
            .Child("obstaclesTravelled").ValueChanged += HandleCompetitorTravelledObstacle;
    }

    public void OnReadyClick()
    {
        ready = true;
        GameObject.Find("ReadyButton").GetComponent<Button>().interactable = false;
        Dictionary<string, object> readyUpdate = new Dictionary<string, object>();
        readyUpdate["/Tournaments/" + lvlAddress + "/" + playerRole + "/ready"] = ready;
        reference.UpdateChildrenAsync(readyUpdate);
    }
}
