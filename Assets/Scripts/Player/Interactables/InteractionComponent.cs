using UnityEngine;
using UniversalMobileController;

public class InteractionComponent : MonoBehaviour
{
    public Transform cameraTransform;
    public float maxDistance;
    public LayerMask layerMask;

    public GameObject button;
    public SpecialButton buttonScript;
    
    private GameObject _activeInteractable;
    private int _activeInteractableLayer;

    private void Start()
    {
        button = buttonScript.gameObject;
    }

    void Update()
    {
        if(_activeInteractable == null) button.SetActive(false);
        if (Physics.Linecast(cameraTransform.position, cameraTransform.position + (cameraTransform.forward) * maxDistance, out var hit, layerMask)
            && (hit.transform.TryGetComponent<Interactable>(out var interactable) ||
                (hit.transform.parent != null && hit.transform.parent.TryGetComponent(out interactable))))
        {
            if (buttonScript.isDown)
            {
                interactable.Interact(this.transform.parent.gameObject);
            }

            if (_activeInteractable == hit.transform.gameObject)
                return;

            if (hit.transform.gameObject != _activeInteractable && _activeInteractable != null)
            {
                _activeInteractable.layer = _activeInteractableLayer;
            }
            _activeInteractable = hit.transform.gameObject;
            _activeInteractableLayer = _activeInteractable.layer;
            _activeInteractable.layer = 12;
            button.SetActive(true);
        }
        else if(_activeInteractable != null)
        {
            button.SetActive(false);
            _activeInteractable.layer = _activeInteractableLayer;
            _activeInteractable = null;
        }
    }
}
