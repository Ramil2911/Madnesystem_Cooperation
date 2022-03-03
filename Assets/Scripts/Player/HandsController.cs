using System;
using TheFirstPerson;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HandsController : MonoBehaviour
{
    public FPSController fpsController;

    public GameObject empty;

    public uint slot;

    private InventoryItem _currentItem;


    public InteractionComponent _interactionComponent;

    private InteractionComponent interactionComponent //i'm gonna write an attribute for this
    {
        get
        {
            _interactionComponent ??= GetComponent<InteractionComponent>();
            return _interactionComponent;
        }
    }

    public InventoryComponent inventory;

    public InventoryItem
        item //i got an error indicating that some start function had started later than another one, so this is just a convinient way to avoid errors 
    {
        get
        {
            inventory ??= GetComponent<InventoryComponent>();
            return inventory[(int) slot];
        }
    }

    private RecoilController _recoilController;

    public RecoilController recoilController
    {
        get
        {
            _recoilController ??= GetComponent<RecoilController>();
            return _recoilController;
        }
    }

    public UnityEvent activeSlotChangedEvent = new();

    public GameObject hands;
    private ushort SIZE = 8;

    // Start is called before the first frame update
    void Start()
    {
        fpsController = GetComponent<FPSController>();
        activeSlotChangedEvent.AddListener(UpdateHands);
        GetComponentInParent<InventoryComponent>().inventoryChangedEvent.AddListener(OnInventoryUpdated);
    }

    private void OnInventoryUpdated(InventoryComponent inventory)
    {
        if (item != _currentItem)
        {
            UpdateHands();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            var obj = item.description;
            if (item?.description != null && inventory.Remove(item))
            {
                obj.SpawnWorldRepresentation(
                    _interactionComponent.transform.position + _interactionComponent.transform.forward,
                    Quaternion.identity);
            }
        }
    }

    public void FixedUpdate()
    {
        var startValue = slot;
        if (Input.GetAxis("Mouse ScrollWheel") * 100 > 0f)
        {
            slot--;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") * 100 < 0f)
        {
            slot++;
        }

        slot = (ushort) (slot % SIZE);
        if (slot != startValue) activeSlotChangedEvent.Invoke();
    }

    public void UpdateHands()
    {
        _currentItem = item;
        if (item?.description == null)
        {
            SpawnEmpty();
            return;
        }


        //var rotation = Quaternion.identity;
        if (hands != null)
        {
            //rotation = _hands.transform.rotation;
        }

        DestroyImmediate(hands);
        hands = item.description.SpawnHandsRepresentation(this.gameObject, this);
        //_hands.transform.rotation = rotation;
        //_fpsController.cam = _hands.transform;
        var weaponController = hands.GetComponent<WeaponController>();
        weaponController.weaponObject = (WeaponDescription) item.description; //TODO: correct type conversion
        weaponController.recoilController = recoilController;
        interactionComponent.cameraTransform = hands.transform;
    }

    private void SpawnEmpty()
    {

        var rotation = Quaternion.identity;
        if (hands != null)
        {
            rotation = hands.transform.rotation;
        }

        DestroyImmediate(hands);
        hands = Instantiate(empty, this.transform.position, rotation, this.transform);
        //_fpsController.cam = _hands.transform;
        interactionComponent.cameraTransform = hands.transform;
    }

    public void SelectNext(bool noInvoke = false)
    {
        var startValue = slot;
        slot++;
        slot = (ushort) (slot % SIZE);
        while (inventory[(ushort) slot] == null || inventory[(ushort) slot].description == null)
        {
            slot++;
            slot = (ushort) (slot % SIZE);
        }

        if (slot != startValue && !noInvoke) activeSlotChangedEvent.Invoke();
    }

    public void SelectPrevious(bool noInvoke = false)
        {
            var startValue = slot;
            //unsigned magic: 0-1=65535, 65536%8=7
            slot--;
            slot = (ushort) (slot % SIZE);
            while (inventory[(ushort) slot] == null || inventory[(ushort) slot].description == null)
            {
                slot--;
                slot = (ushort) (slot % SIZE);
            }

            if (slot != startValue && !noInvoke) activeSlotChangedEvent.Invoke();
        }
    }
