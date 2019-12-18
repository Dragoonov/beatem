using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWhenShrinked : MonoBehaviour
{
    // Start is called before the first frame update
    FirebaseConnector connector;
    void Start()
    {
        GameObject tournament = GameObject.Find("TournamentConnector");
        if (tournament != null)
        {
            connector = tournament.GetComponent<FirebaseConnector>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 0.7)
        {
            Destroy(gameObject);
            if (connector != null)
            {
                connector.OnTravelObstacle();
            }
        }
    }
}
