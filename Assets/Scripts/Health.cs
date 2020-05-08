using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        //health = maxHealth;
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
        //if (health <= 0) Die();
    }
}
