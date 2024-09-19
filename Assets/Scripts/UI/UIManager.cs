using System;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas deathCanvas;
    [SerializeField] private Canvas exitCanvas;
    [SerializeField] private Canvas inventoryCanvas;

    private void HideEveryScreen()
    {
        inGameCanvas.enabled = false;
        deathCanvas.enabled = false;
        exitCanvas.enabled = false;
        inventoryCanvas.enabled = false;
    }

    public void ShowScreen(UIScreenType screen)
    {
        HideEveryScreen();
        var canvas = UIScreenToCanvas(screen);
        canvas.enabled = true;
    }

    private Canvas UIScreenToCanvas(UIScreenType screen)
    {
        switch(screen)
        {
            case UIScreenType.InGameScreen:
                return inGameCanvas;
            case UIScreenType.DeathScreen:
                return deathCanvas;
            case UIScreenType.ExitScreen:
                return exitCanvas;
            case UIScreenType.InventoryScreen:
                return inventoryCanvas;
            default:
                throw new Exception($"No such UI canvas as {screen.ToString()}");
        }
    }
   
}
