using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwTouchListener : MonoBehaviour
{
    private Camera _camera;
    private IcwRouteBuilder routeBuilder;

    private void Awake()
    {
        _camera = Camera.main;
        routeBuilder = this.gameObject.GetComponent<IcwRouteBuilder>();
        if (routeBuilder == null)
        {
            Debug.LogWarning("Can't find IcwRoteBuilder. Check prefab");
            Destroy(this.gameObject);
            return;
        }
    }
    
    private void Update()
    {
        
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended) continue;
                Vector3 pos = _camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y));
                pos.z = 0;

                routeBuilder.TryAddRoute(pos);
            }
        }

        
    }
}

