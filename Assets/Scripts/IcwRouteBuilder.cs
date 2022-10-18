using System;
using System.Collections.Generic;
using UnityEngine;

public class IcwRouteBuilder : MonoBehaviour
{
    public GameObject RoutePrefab;
    private List<GameObject> routelist = new List<GameObject>();
    private List<GameObject> completedroutelist = new List<GameObject>();
    public IWalker walker;
    public int RouteItemsCount => routelist.Count + completedroutelist.Count;

    public void TryAddRoute(Vector3 finish)
    {
        if (walker == null) return;
        if (!routelist.Exists((GameObject o) => Vector3.Distance(o.GetComponent<IRoute>().finish, finish) < 0.5f))
        {
            GameObject newRouteObject = Instantiate(RoutePrefab, this.transform).gameObject;
            IRoute newRoute = newRouteObject.GetComponent<IRoute>();
            if (routelist.Count > 0)
                newRoute.start = routelist[^1].GetComponent<IRoute>().finish;
            else
                newRoute.start = walker.Position;
            newRoute.finish = finish;
            newRoute.walker = walker;
            routelist.Add(newRouteObject);
        }
    }

    private void Update()
    {
        if (walker == null) return;
        if (routelist.Count > 0 && routelist[0].GetComponent<IRoute>().Completed)
        {
            completedroutelist.Add(routelist[0]);
            routelist.RemoveAt(0);
        }
        if (routelist.Count > 0)
        {
            if (!routelist[0].GetComponent<IRoute>().inProgress)
            {
                routelist[0].GetComponent<IRoute>().inProgress = true;
                walker.SetTarget(routelist[0].GetComponent<IRoute>().finish);
            }
        }
        else if (walker.IsMoving) walker.SetTarget(null);
    }
}