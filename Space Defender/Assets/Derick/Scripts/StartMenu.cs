using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Text start;
    public Text highScore;
    public GameObject panel;
    public static string line;
    public GameObject minimap;
    string path; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        line = "";
        path = Application.persistentDataPath + "/highscore.txt";

        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }

        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(path);
            //Read the first line of text
            line = sr.ReadLine();
            //Continue to read until you reach end of file

            sr.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        highScore.text = "High Score: " + line;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) // A button
        {
            start.enabled = false;
            highScore.enabled = false;
            panel.SetActive(false);

            PlayerControllerNew.playing = true;
        }

        if (PlayerControllerNew.howManyDead == 10)
        {
            start.text = "Press A to restart";

            PlayerControllerNew.playing = false;
            minimap.SetActive(false);

            if (PlayerControllerNew.T > Int32.Parse(line))
            {
                start.enabled = true;
                highScore.enabled = true;
                panel.SetActive(true);
                int hs = (int)PlayerControllerNew.T;
                highScore.text = "New High Score: " + hs;

                StreamWriter sw = new StreamWriter(path);
                sw.Write(hs);
                sw.Close();
            }
            else
            {
                start.enabled = true;
                highScore.enabled = true;
                panel.SetActive(true);
                int hs = (int)PlayerControllerNew.T;
                highScore.text = "Your Score: " + hs + ", High Score: " + line;
            }
            if (Input.GetButtonDown("Jump")) // A button
            {
                start.enabled = false;
                highScore.enabled = false;
                panel.SetActive(false);
                if (Input.GetButtonDown("Jump")) // A button
        {
            start.enabled = false;
            highScore.enabled = false;
            panel.SetActive(false);

            PlayerControllerNew.playing = true;
        }
                PlayerControllerNew.playing = true;
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }
}
