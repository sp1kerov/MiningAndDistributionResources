using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private float _timerResourceExtraction;

    private IEnumerator MoveTo(Transform target, float timeToReach)
    {
        while (true)
        {
            _timerResourceExtraction += Time.fixedDeltaTime / timeToReach;
            transform.position = Vector3.Lerp(transform.position, target.position, _timerResourceExtraction);

            if (_timerResourceExtraction >= timeToReach)
            {
                transform.position = target.position;
                _timerResourceExtraction = 0;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
    public void MoveToLocalParent(float timeToReach)
    {
        if (timeToReach > 0)
        {
            StartCoroutine(MoveTo(transform.parent, timeToReach));
        }
        else
        {
            transform.position = transform.parent.position;
        }
    }

}
