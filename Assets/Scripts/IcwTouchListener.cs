using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwTouchListener : MonoBehaviour
{
    List<GameObject> routelist = new List<GameObject>();
    List<GameObject> completedroutelist = new List<GameObject>();
    private Camera _camera;
    public IWalker player;
    public GameObject RoutePrefab;
    IGame agame;

    private void Awake()
    {
        _camera = Camera.main;
        agame = transform.parent.GetComponent<IGame>();
    }
    private void AddRouteToList()
    {
        
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended) continue;
            Vector3 pos = _camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y));
            pos.z = 0;
            if (!routelist.Exists((GameObject o) => Vector3.Distance(o.GetComponent<IcwRoute>().finish, pos) < 0.5f))
            {
                GameObject newRouteObject = Instantiate(RoutePrefab, this.transform).gameObject;
                IcwRoute newRoute = newRouteObject.GetComponent<IcwRoute>();
                if (routelist.Count > 0)
                {
                    newRoute.start = routelist[^1].GetComponent<IcwRoute>().finish;
                }
                else
                { 
                    newRoute.start = player.Position; 
                }
                newRoute.finish = pos;
                newRoute.walker = player;
                routelist.Add(newRouteObject);
            }
        }
        return;
    }

    private void Update()
    {
        
       
        if (agame.gameState != IGame.EnumGameState.InProgress) return;

        player = agame.GetPlayer();
        if (player == null) return;
        
        if (Input.touchCount > 0)
        {
            AddRouteToList();
        }

        if (routelist.Count > 0 && routelist[0].GetComponent<IcwRoute>().Completed)
        {
            completedroutelist.Add(routelist[0]);
            routelist.RemoveAt(0);
        }
        if (routelist.Count > 0 )
        {
            if (!routelist[0].GetComponent<IcwRoute>().inProgress)
            {
                routelist[0].GetComponent<IcwRoute>().inProgress = true;
                player.SetTarget(routelist[0].GetComponent<IcwRoute>().finish);
            }
        }
        else if (player.IsMoving) player.SetTarget(null);
    }
}

