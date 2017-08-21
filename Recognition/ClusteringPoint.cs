/// <summary>
/// Контейнер для кластеризируемой точки 
/// </summary>
public class ClusteringPoint
{
    public int ClusterId;
    public bool IsVisited;
    public PointData ClusterPoint;

    public ClusteringPoint(PointData x)
    {
        ClusterPoint = x;
        IsVisited = false;
        ClusterId = (int)ClusterID.UNCLASSIFIED;
    }
}
