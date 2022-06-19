using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class RegisterManager : MonoBehaviour
{
    public static RegisterManager Instance;
    private Partie partie  = new Partie();

    private void Awake() => Instance = this;

    public void AddCoup(Coup c) => partie.ListCoups.Add(c);

    public void SavePartie()
    {
        string day = (DateTime.Now.Day <= 9) ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString();
        string month = (DateTime.Now.Month <= 9) ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString();
        SaveSystem.Save(partie, day + "-" + month + "-" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
    }
}
