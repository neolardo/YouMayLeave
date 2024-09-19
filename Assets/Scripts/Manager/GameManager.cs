using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private UIInput uiInput;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private UIManager uiManager;
    private void Awake()
    {
        SetUpActions();
        StartGame();
    }

    private void SetUpActions()
    {
        player.Died += () => uiManager.ShowScreen(UIScreenType.DeathScreen);
        player.HealthChanged += inGameUI.UpdateHealth;
        uiInput.anythingAction.performed += (_) => RevivePlayerIfDead();
    }

    private void RevivePlayerIfDead()
    {
        if(!player.IsAlive)
        {
            player.Revive();
            uiManager.ShowScreen(UIScreenType.InGameScreen);
        }
    }


    private void StartGame()
    {
        uiManager.ShowScreen(UIScreenType.InGameScreen);
    }

}
