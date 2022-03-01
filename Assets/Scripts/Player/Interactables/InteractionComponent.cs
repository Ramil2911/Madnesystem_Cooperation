using UnityEngine;

public class InteractionComponent : MonoBehaviour
{
    public Transform cameraTransform;
    public float maxDistance;
    public LayerMask layerMask;
    
    
    private GameObject _activeInteractable;
    private int _activeInteractableLayer;
    
    void Update()
    {
        if (Physics.Linecast(cameraTransform.position, cameraTransform.position + (cameraTransform.forward) * maxDistance, out var hit, layerMask)
            && (hit.transform.TryGetComponent<Interactable>(out var interactable) || hit.transform.parent.TryGetComponent(out interactable)))
        {
            if (Input.GetKeyDown(KeyCode.E))
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
        }
        else if(_activeInteractable != null)
        {
            _activeInteractable.layer = _activeInteractableLayer;
            _activeInteractable = null;
        }
    }
}
