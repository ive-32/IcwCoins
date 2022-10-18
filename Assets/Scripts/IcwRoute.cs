using System.Collections.Generic;
using UnityEngine;

public class IcwRoute : MonoBehaviour, IRoute
{
    [System.NonSerialized] public GameObject target;
    [System.NonSerialized] public List<GameObject> arrows = new List<GameObject>();
    private bool isCompleted;
    public GameObject targetPrefab;
    public GameObject arrowPrefab;
    public Vector3 start { get; set; }
    public Vector3 finish { get; set; }
    public bool inProgress { get; set; }
    public IWalker walker { get; set; }
    public bool Completed
    {
        get => isCompleted;
        set
        {
            isCompleted = value;
            if (value)
                RouteCompleted();
        }
    }

    private void Start()
    {
        target = MonoBehaviour.Instantiate(targetPrefab, finish, Quaternion.identity, this.transform);
        isCompleted = false;
        float distance = Vector2.Distance(finish, start);
        float spaces = (distance - Mathf.FloorToInt(distance)) / 2f;
        Vector3 direction = (finish - start).normalized;
        for (int i = 1; i <= Mathf.FloorToInt(distance); i++)
        {
            float angle = Vector3.SignedAngle(Vector3.right, finish - start, Vector3.forward);

            Vector3 pos = start + direction * (spaces + i );
            arrows.Add(Instantiate(arrowPrefab, pos, Quaternion.identity, this.transform));
            arrows[^1].transform.Rotate(new Vector3(0, 0, angle));
        }
    }

    private void RouteCompleted()
    {
        MonoBehaviour.Destroy(target);
    }

}
