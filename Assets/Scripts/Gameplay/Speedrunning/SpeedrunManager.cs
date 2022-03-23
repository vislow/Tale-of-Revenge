using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Player.Components;
using LevelManagement;
using GameManagement;

public class SpeedrunManager : MonoBehaviour {
    public CheckpointManager checkpointManager;
    public DeathCounter deathCounter;
    public Timer timer;
    public PlayerManager player;
    [SerializeField] private GameObject statsUI;

    private bool waitingToReset;
    private PlayerControls controls;

    private void Awake() {
        controls = new PlayerControls();

        controls.Gameplay.Reset.performed += context => TriggerReset();

        PlayerDeathManager.OnDeathStageChanged += DeathEvents;
        //TransitionManager.OnTransitionMiddleStarted += ResetGame;
        GameStateManager.OnGameStateChanged += SetUIActivity;
    }

    private void OnDestroy() {
        PlayerDeathManager.OnDeathStageChanged -= DeathEvents;
        //TransitionManager.OnTransitionMiddleStarted -= ResetGame;
        GameStateManager.OnGameStateChanged -= SetUIActivity;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    private void DeathEvents(DeathStages deathStages) {
        switch (deathStages) {
            case DeathStages.Resetting:
                checkpointManager.OnRespawn();
                deathCounter.OnRespawn();
                timer.OnRespawn();
                break;
        }
    }

    private void SetUIActivity(GameState gameState) {
        if (gameState == GameState.Gameplay) {
            statsUI.SetActive(true);
        } else {
            statsUI.SetActive(false);
        }
    }

    private void TriggerReset() {
        //TransitionManager.instance.TriggerTransition(1f);
        waitingToReset = true;
    }

    private void ResetGame() {
        if (!waitingToReset) return;

        waitingToReset = false;

        checkpointManager.OnReset();
        deathCounter.OnReset();
        timer.OnReset();
    }

    [ContextMenu("Clear Player Prefs")]
    private void ClearPlayerPrefs() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Highscore", 0f);
    }
}
