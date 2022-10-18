using UnityEngine;
public interface IRoute
{
    Vector3 start { get; set; }
    Vector3 finish { get; set; }
    bool inProgress { get; set; }
    IWalker walker { get; set; }
    bool Completed { get; set; }

}
