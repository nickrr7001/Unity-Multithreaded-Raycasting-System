using UnityEngine;
public class Example : MonoBehaviour
{
    public LayerMask mask;
    private int reqindex = 0;
    private void Update()
    {
        // 1:Cast Position 2:Direction 3:LayerMask 4: Distance
        reqindex = RaycastRequesting.NewRequest(new RaycastRequest(transform.position, transform.forward, mask, 50f));
        StartCoroutine(accessRequestedRaycast());
    }
    private IEnumerator accessRequestedRaycast()
    {
    //Waiting for end of frame because the raycasts are processed in late update, this however can be altered
    //For example make requests in fixed update, then process job in update function, then process output in late update
        yield return new WaitForEndOfFrame();
        RaycastHit hit = RaycastRequesting.getResponse(i);
            if (hit.collider != null )
            {
                Debug.Log(hit.collider.gameObject.name);
            }
    }
}
