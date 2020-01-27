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
    Text myRingsTravelled;
    TournamentDataWrapper wrapper;
    UserManager user;
    bool ready;
    bool competitorReady;
    string competitor;
    bool showReadyPanel;
    string lvlAddress;
    string playerRole;
    Text obstaclesTravelled;
    int enemyObstaclesTravelledAmount;
    bool initialized = false;

    bool handleListeners = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            obstaclesTravelled = GameObject.Find("TravelledObstacles").GetComponent<Text>();
            enemyObstaclesTravelledAmount = 0;
            readyPanel = GameObject.Find("ReadyPanel");
            readyPanel.SetActive(false);
            user = GameObject.Find("User").GetComponent<UserManager>();
            ready = false;
            competitorReady = false;
            checkFirebaseVersion();
            level = GameObject.Find("LevelGenerator").GetComponent<GenerateTournamentLevel>();
            startingPanel = GameObject.Find("StartingPanel");
            tournamentMessage = GameObject.Find("TournamentMessage");
            generateButton = GameObject.Find("GenerateButton");
            findButton = GameObject.Find("FindButton");
            backButton = GameObject.Find("BackButton");
            myRingsTravelled = GameObject.Find("TravelledObstaclesMy").GetComponent<Text>();
            tournamentMessage.SetActive(false);
            lvlAddress = user.GetLoggedUser().UserId;
            initialized = true;
        }
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
        myRingsTravelled.text = "Your rings: " + enemyObstaclesTravelledAmount;
        Dictionary<string, object> obstacleUpdate = new Dictionary<string, object>();
        obstacleUpdate["/Tournaments/" + lvlAddress + "/" + playerRole + "/obstaclesTravelled"] = enemyObstaclesTravelledAmount;
        reference.UpdateChildrenAsync(obstacleUpdate);
    }

    private void HideLayout()
    {
        tournamentMessage.SetActive(true);
        generateButton.SetActive(false);
        findButton.SetActive(false);
        //backButton.SetActive(false);
    }

    private void checkFirebaseVersion()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://beatem-d1472.firebaseio.com/");
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
        playerRole = "creator";
        HideLayout();
        tournamentMessage.GetComponent<Text>().text = "Setting everything ready...";

        string email = user.GetLoggedUser().Email;

        wrapper = new TournamentDataWrapper(
            new Competitor(email.Substring(0, email.IndexOf('@')), false, 0, false),
            new Competitor("guest", false, 0, false),
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

        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .Child("guest")
            .Child("finished").ValueChanged += HandleCompetitorFinished;

        tournamentMessage.GetComponent<Text>().text = "Waiting for opponent...";
    }

    void HandleGuestJoined(object sender, ValueChangedEventArgs args)
    {
        if(handleListeners)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            competitor = (string)args.Snapshot.GetValue(false);
            if (competitor != "guest")
            {
                startingPanel.SetActive(false);
                readyPanel.SetActive(true);
            }
        }
        
    }

    void HandleCompetitorFinished(object sender, ValueChangedEventArgs args)
    {
        if(handleListeners)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            bool finished = (bool)args.Snapshot.GetValue(false);
            if (finished)
            {
                GameObject levelTemp = GameObject.FindGameObjectWithTag("Level");
                LevelDisplay levelDisplay = levelTemp.GetComponent<LevelDisplay>();
                levelDisplay.Finish();
                levelTemp.GetComponent<LevelSpeed>().Finish();
                levelTemp.GetComponent<UserInterface>().Finish();
                GameObject.Find("LevelUI").SetActive(false);
                levelDisplay.ShowFinishPanel("You lost!");
                RemoveConnections(playerRole.Equals("creator") ? "guest" : "creator");
                //reference.Child("Tournaments")
                //.Child(user.GetLoggedUser().UserId)
                //.SetRawJsonValueAsync(JsonUtility.ToJson(wrapper));
            }
        }
        
    }

    public void OnFinish()
    {
        Dictionary<string, object> finishUpdate = new Dictionary<string, object>();
        finishUpdate["/Tournaments/" + lvlAddress + "/" + playerRole + "/finished"] = true;
        reference.UpdateChildrenAsync(finishUpdate);
        reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .SetRawJsonValueAsync(JsonUtility.ToJson(wrapper));
        RemoveConnections(playerRole.Equals("creator") ? "guest" : "creator");
    }


    void HandleCompetitorTravelledObstacle(object sender, ValueChangedEventArgs args)
    {
        if(handleListeners)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            obstaclesTravelled.text = "Enemy rings: " + args.Snapshot.GetValue(false);
        }
        
    }

    void HandleCompetitorReady(object sender, ValueChangedEventArgs args)
    {
        if(handleListeners)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            competitorReady = (bool)args.Snapshot.GetValue(false);
        }
    }

    private void FindCompetitor(DataSnapshot snapshotData)
    {
        foreach (DataSnapshot dataSnapshot in snapshotData.Children)
        {
            if (!dataSnapshot.Key.Equals(user.GetLoggedUser().UserId))
            {
                lvlAddress = dataSnapshot.Key;
                break;
            }
        }
        FirebaseDatabase.DefaultInstance
            .GetReference("Tournaments")
            .Child(lvlAddress)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    wrapper = JsonUtility.FromJson<TournamentDataWrapper>(snapshot.GetRawJsonValue());
                    competitor = wrapper.creator.name;
                    JoinMatch();
                }
            });
    }

    public void OnFind()
    {
        playerRole = "guest";
        HideLayout();
        tournamentMessage.GetComponent<Text>().text = "Downloading level...";
        FirebaseDatabase.DefaultInstance.GetReference("Tournaments")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Tournaments successed");
                    DataSnapshot snapshot = task.Result;
                    FindCompetitor(snapshot);
                }
            });
    }

    public void DeleteMatch()
    {
        reference.Child("Tournaments")
            .Child(lvlAddress)
            .RemoveValueAsync();
    }

    private void RemoveConnections(string player)
    {
        reference.Child("Tournaments")
            .Child(lvlAddress)
            .Child(player)
            .Child("ready").ValueChanged -= HandleCompetitorReady;

        reference.Child("Tournaments")
            .Child(lvlAddress)
            .Child(player)
            .Child("obstaclesTravelled").ValueChanged -= HandleCompetitorTravelledObstacle;

        reference.Child("Tournaments")
            .Child(lvlAddress)
            .Child(player)
            .Child("finished").ValueChanged -= HandleCompetitorFinished;

        if(playerRole.Equals("creator"))
        {
            reference.Child("Tournaments")
            .Child(user.GetLoggedUser().UserId)
            .Child("guest")
            .Child("name").ValueChanged -= HandleGuestJoined;
        }
        handleListeners = false;
        Debug.Log("Removed from lvlAddres: " + lvlAddress + ", player: " + player);
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

        reference.Child("Tournaments")
            .Child(lvlAddress)
            .Child("creator")
            .Child("finished").ValueChanged += HandleCompetitorFinished;
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
