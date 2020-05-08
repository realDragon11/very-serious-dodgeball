using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public Image healthGuage;
    public Sprite full, twoThirds, oneThird, empty;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthGuage.sprite = full;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        if (CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }
        else gameObject.SetActive(false);
    }

    public void DeductHealth()
    {
        health--;
        if (health == 2) healthGuage.sprite = twoThirds;
        else if (health == 1) healthGuage.sprite = oneThird;
        else if (health <= 0)
        {
            healthGuage.sprite = empty;
            Die();
        }
    }
}
