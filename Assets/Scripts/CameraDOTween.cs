using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening;

public class CameraDOTween : MonoBehaviour
{
    public Transform camera;
    public Transform positionEmpty;
    public Transform firstPositionEmpty;

    public void OnClick_moveCamera()
    {
        Debug.Log("OnClick");
        camera.DOMove(positionEmpty.transform.position, 1f);
        StartCoroutine(BackToFirstPosition());
    }

    IEnumerator BackToFirstPosition()
    {
        yield return new WaitForSeconds(3);
        camera.DOMove(firstPositionEmpty.transform.position, 1f);
    }
}
