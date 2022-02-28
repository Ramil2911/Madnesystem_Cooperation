
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBarController : MonoBehaviour
{
    [SerializeField] private InventoryComponent inventoryComponent;
    [SerializeField] private HandsController handsController;

    private InventoryUIItem[] bar = new InventoryUIItem[8];

    void Start()
    {
        for (var i = 0; i < 8; i++)
        {
            var item = this.transform.GetChild(i);

            bar[i] = new InventoryUIItem()
            {
                panelImage = item.GetComponent<Image>(),
                spriteImage = item.GetChild(0).GetComponent<Image>(),
                text = item.GetChild(1).GetComponent<TextMeshProUGUI>()
            };
        }
        SlotChangedListener();
        handsController.activeSlotChangedEvent.AddListener(SlotChangedListener);
        inventoryComponent.inventoryChangedEvent.AddListener(InventoryChangedListener);
    }

    private void InventoryChangedListener(InventoryComponent _)
    => SlotChangedListener();
    

    private void SlotChangedListener()
    {
        var item = handsController.item;
        var index = (int)handsController.slot;

        for (var i = 0; i < 8; i++)
        {
            if (inventoryComponent[i] != null && inventoryComponent[i].description != null)
            {
                bar[i].panelImage.color = i == index ? Color.grey : Color.white;
                bar[i].spriteImage.sprite = inventoryComponent[i].description.Sprite;
                bar[i].text.text = inventoryComponent[i].Stackable
                    ? inventoryComponent[i].amount.ToString()
                    : "";
            }
            else
            {
                bar[i].panelImage.color = Color.red;
                bar[i].spriteImage.sprite = null;
                bar[i].text.text = "";
            }
        }
    }
}

internal struct InventoryUIItem
{
    public Image panelImage;
    public Image spriteImage;
    public TextMeshProUGUI text;
}
