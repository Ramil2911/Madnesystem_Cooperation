using UnityEngine;

public class Door : Interactable
{
    private bool _canBeOpened = true;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    public void Lock()
    {
        _canBeOpened = false;
    }
    
    public void Unlock()
    {
        _canBeOpened = true;
    }

    public bool IsAvailable()
    {
        return _canBeOpened;
    }

    public void Close()
    {
        var animator = transform.GetComponent<Animator>();
        var currentState = animator.GetBool(IsOpen);
        if (currentState)
        {
            animator.SetBool(IsOpen, false);
        }
    }
    
    public void Open()
    {
        var animator = transform.GetComponent<Animator>();
        var currentState = animator.GetBool(IsOpen);
        if (!currentState)
        {
            animator.SetBool(IsOpen, true);
        }
    }
    
    public void Activate()
    {
        var animator = transform.GetComponent<Animator>();
        var currentState = animator.GetBool(IsOpen);
        animator.SetBool(IsOpen, !currentState);
    }

    public override void Interact(GameObject actor)
    {
        Activate();
    }
}
