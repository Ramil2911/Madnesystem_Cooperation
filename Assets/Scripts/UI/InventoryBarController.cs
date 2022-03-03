
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBarController : MonoBehaviour
{
    [SerializeField] private InventoryComponent inventoryComponent;
    [SerializeField] private HandsController handsController;

    private InventoryUIItem bar;

    void Start()
    {
        var item = this.transform.GetChild(0);
        bar = new InventoryUIItem()
        {
            panelImage = item.GetComponent<Image>(),
            spriteImage = item.GetChild(0).GetComponent<Image>(),
            text = item.GetChild(1).GetComponent<TextMeshProUGUI>()
        };
        SlotChangedListener();
        handsController.activeSlotChangedEvent.AddListener(SlotChangedListener);
        inventoryComponent.inventoryChangedEvent.AddListener(InventoryChangedListener);
    }

    private void InventoryChangedListener(InventoryComponent _)
    => SlotChangedListener();
    

    private void SlotChangedListener()
    {
        var index = (int)handsController.slot;
        if (inventoryComponent[index] != null && inventoryComponent[index].description != null)
        {
            bar.panelImage.color = Color.white;
            bar.spriteImage.sprite = inventoryComponent[index].description.Sprite;
            bar.text.text = inventoryComponent[index].Stackable
                ? inventoryComponent[index].amount.ToString()
                : "";
        }
        else
        {
            bar.panelImage.color = Color.red;
            bar.spriteImage.sprite = null;
            bar.text.text = "";
        }
    }

    private struct InventoryUIItem
    {
        public Image panelImage;
        public Image spriteImage;
        public TextMeshProUGUI text;
    }
}
