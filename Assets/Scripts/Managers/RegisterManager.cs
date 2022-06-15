using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RegisterManager : MonoBehaviour
{
    public static RegisterManager Instance;
    private PartieSO partieSO;
    
    private void Awake()
    { 
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }
    
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.SpawnPlayers) partieSO = PartieSO.CreatePartie();
    }

    public void AddCoup(Coup c) => partieSO.partie.Add(c);
}
