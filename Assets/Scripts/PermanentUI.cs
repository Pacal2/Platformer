using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentUI : MonoBehaviour
{

    // Player Stats
    public int keys = 0;
    public TextMeshProUGUI keyAmount;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int health;
    public int numOfHearts = 3;

    

    public static PermanentUI perm;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
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

        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {

            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void Reset()
    {
        keys = 0;
        keyAmount.text = keys.ToString();
        health = 3;
    }
}
