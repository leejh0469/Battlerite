using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private Button gameStart_Host_Btn;
    [SerializeField] private Button gameStart_Client_Btn;

    [SerializeField] private BasicSpawner spawner;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        gameStart_Host_Btn.onClick.AddListener(() => OnGameStart_BtnPressed(GameMode.Host));
        gameStart_Client_Btn.onClick.AddListener(() => OnGameStart_BtnPressed(GameMode.Client));
    }

    private void OnGameStart_BtnPressed(GameMode mode)
    {
        spawner.StartGame(mode);
    }
}
