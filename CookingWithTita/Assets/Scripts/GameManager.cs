using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Data
    [SerializeField] private Player player;
    public Player GetPlayer() { return player; }
    public int playerScore;

    #endregion
    private void Awake() {
        player = FindObjectOfType<Player>();
        if (player)
            player.InitializePlayer(this);
    }

    private void Update() {
        
    }
}
