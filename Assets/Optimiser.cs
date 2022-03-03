using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimiser : MonoBehaviour
{

    IEnumerator EnableDecor(Transform manager)
    {
        // Ждем появления Decor
        while (true)
        {
            if(manager == null) yield return null;
            if (manager.childCount >= 4)
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }
        manager.GetChild(3).gameObject.SetActive(true);
    }

    IEnumerator DisableDecor(Transform manager)
    {
        // Ждем появления Decor
        while (true)
        {
            if (manager.childCount >= 4)
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }
        manager.GetChild(3).gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        EnableRendering(other);
    }

    private void OnTriggerExit(Collider other)
    {
        DisableRendering(other);
    }

    private void EnableRendering(Collider collider)
    {
        if (collider.gameObject.CompareTag("RoomCollider"))
        {
            GameObject tile = collider.gameObject.transform.parent.gameObject;
            if (tile.CompareTag("Room"))
            {
                if (tile.TryGetComponent<MeshRenderer>(out var renderer))
                {
                    renderer.enabled = true;
                }
                if(tile.transform.Find("Manager")) {
                    StartCoroutine("EnableDecor", tile.transform.Find("Manager"));
                }
            }
        }
    }

    private void DisableRendering(Collider collider)
    {
        if (collider.gameObject.CompareTag("RoomCollider"))
        {
            GameObject tile = collider.gameObject.transform.parent.gameObject;
            if (tile.CompareTag("Room"))
            {
                if (tile.TryGetComponent<MeshRenderer>(out var renderer))
                {
                    renderer.enabled = false;
                }
                if (tile.transform.Find("Manager"))
                {
                    StartCoroutine("DisableDecor", tile.transform.Find("Manager"));
                }
            }
        }
    }
}
