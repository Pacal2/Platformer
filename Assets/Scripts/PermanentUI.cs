using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentUI : MonoBehaviour
{

    // Player Stats
    public int cherries = 0;
    public int health = 5;
    public TextMeshProUGUI cherryScore;
    public Text healthAmount;

    public static PermanentUI perm;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        cherries = 0;
        cherryScore.text = cherries.ToString();
        health = 5;
        healthAmount.text = health.ToString();
    }
}
