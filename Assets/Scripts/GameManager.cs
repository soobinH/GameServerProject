using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    public bool isGameOver { get; private set; }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //플레이어 사망 이벤트 발생시 게임오버 이벤트를 추가
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    public void EndGame()
    {
        isGameOver = true;
        UIManager.Instance.SetActiveGameOverUI(true);
    }
}
