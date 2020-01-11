using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockLevels : MonoBehaviour
{
	public const int LVLS_AMOUNT = 10;
    UserManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("User").GetComponent<UserManager>();
        for (int i = 1; i <= LVLS_AMOUNT; i++)
		{
            GameObject.Find("Level" + i).GetComponent<Button>().interactable = manager.isUnlocked(i);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
