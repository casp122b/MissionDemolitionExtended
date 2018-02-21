using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle, playing, levelEnd
}

public class MissionDemolition: MonoBehaviour
{
    public static MissionDemolition S;

    public GameObject[] castles;
    public Text gtLevel;
    public Text gtScore;
    public Vector3 castlePos;

    private int level;
    private int levelMax;
    private int shotsTaken;
    private GameObject castle;
    private GameMode mode = GameMode.idle;
    private string showing = "SlingShot";

    private void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        if (castle != null)
            Destroy(castle);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;


        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
        SwitchView("Both");
    }

    private void ShowGT()
    {
        gtLevel.text = "Level: " + (level + 1);
        gtScore.text = "Score: " + shotsTaken;
    }

    private void Update()
    {
        ShowGT();

        if (mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            Invoke("NextLevel", 2f);
            SwitchView("Both");
        }
    }

    private void NextLevel()
    {
        level++;
        if (level == levelMax)
            level = 0;
        StartLevel();
    }

    private void OnGUI()
    {
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);
        switch (showing)
        {
            case "SlingShot":
                if (GUI.Button(buttonRect, "Show Castle"))
                {
                    SwitchView("Castle");
                }
                break;
            case "Castle":
                if (GUI.Button(buttonRect, "Show Both"))
                {
                    SwitchView("Both");
                }
                break;
            case "Both":
                if (GUI.Button(buttonRect, "Show SlingShot"))
                {
                    SwitchView("SlingShot");
                }
                break;
        }
    }

    private void SwitchView(string eView)
    {
        S.showing = eView;
        switch (S.showing)
        {
            case "SlingShot":
                FollowCam.s.poi = null;
                break;

            case "Castle":
                FollowCam.s.poi = S.castle;
                break;

            case "Both":
                FollowCam.s.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    public static void shotFired()
    {
        S.shotsTaken++;
    }

}