using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI timeText;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    private float timeLeft; // declare time left variable
    private float spawnRate = 1.5f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        // make food spawn based on difficulty level chosen by players
        spawnRate /= difficulty; 
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        StartCoroutine(CountDownTimer());
        score = 0;
        timeLeft = 30.0f;
        UpdateScore(0);
        UpdateTimer(timeLeft); // set time left to initial time when game starts
        titleScreen.SetActive(false);
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
            
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        // increase score everytime player clickes food
        score += scoreToAdd;
        // add the increased score to score text so as to be able to display it
        scoreText.text = "Score: " + score;
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        // display game over text when game over
        gameOverText.gameObject.SetActive(true);
        // display restart button when game over
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateTimer(float timer)
    {
        // displays the time left for the player to see by making the time text show the count down
        timeText.text = "Time Left: " + timer;
    }

    IEnumerator CountDownTimer()
    {
        float timeLeft = 30.0f;

        while (timeLeft > 0 && isGameActive) // while there is still time and the game is still on
        {
            timeLeft--; // decrease time
            yield return new WaitForSeconds(1f); // after 1 second            
            UpdateTimer(timeLeft); // update time text to the time left value
        }
        if (timeLeft == 0) // if the time is 0
        {
            GameOver(); // game is over
        }
    }

}
