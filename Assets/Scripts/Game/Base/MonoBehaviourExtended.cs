using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourExtended : MonoBehaviour
{
    
    protected void StopRoutine(Coroutine coroutine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }



    protected IEnumerator LerpRotateTransform(Transform transform, float speed, float targetRotation)
    {
        float curRotationValue = transform.rotation.y;

        while (Mathf.Abs(curRotationValue - targetRotation) > 0.3f)
        {
            curRotationValue = Mathf.Lerp(curRotationValue, targetRotation, Time.deltaTime * speed);

            transform.rotation = Quaternion.Euler(0, curRotationValue, 0);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, targetRotation, 0);
    }

    protected IEnumerator LerpMoveLocalTransformCoroutine(Transform transform, float speed, Vector3 targetPosition)
    {
        var curPosition = transform.localPosition;

        while (Vector3.Distance(curPosition, targetPosition) > 0.05f)
        {
            transform.localPosition = curPosition = Vector3.Lerp(curPosition, targetPosition, Time.deltaTime * speed);

            yield return null;
        }
    }
}
