using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Алгоритм кластеризации(Dbscan)
/// </summary>
public class Clustering
{
    private readonly Func<PointData, PointData, double> _metricFunc;

    public Clustering(Func<PointData, PointData, double> metricFunc)
    {
        _metricFunc = metricFunc;
    }

    private void RegionQuery(ClusteringPoint[] allPoints, PointData p, double epsilon, out ClusteringPoint[] neighborPts)
    {
        neighborPts = allPoints.Where(x => _metricFunc(p, x.ClusterPoint) <= epsilon).ToArray();
    }

    private void ExpandCluster(ClusteringPoint[] allPoints, ClusteringPoint p, ClusteringPoint[] neighborPts, int c, double epsilon, int minPts)
    {
        p.ClusterId = c;
        for (int i = 0; i < neighborPts.Length; i++)
        {
            var pn = neighborPts[i];
            if (!pn.IsVisited)
            {
                pn.IsVisited = true;
                ClusteringPoint[] neighborPts2 = null;
                RegionQuery(allPoints, pn.ClusterPoint, epsilon, out neighborPts2);
                if (neighborPts2.Length >= minPts)
                {
                    neighborPts = neighborPts.Union(neighborPts2).ToArray();
                }
            }
            if (pn.ClusterId == (int)ClusterID.UNCLASSIFIED)
                pn.ClusterId = c;
        }
    }
    public void ComputeCluster(PointData[] allPoints, double epsilon, int minPts, out HashSet<PointData[]> clusters)
    {
        clusters = null;
        var allPointsDbscan = allPoints.Select(x => new ClusteringPoint(x)).ToArray();
        var c = 0;
        foreach (var p in allPointsDbscan)
        {
            if (p.IsVisited)
                continue;
            p.IsVisited = true;

            ClusteringPoint[] neighborPts = null;
            RegionQuery(allPointsDbscan, p.ClusterPoint, epsilon, out neighborPts);
            if (neighborPts.Length < minPts)
                p.ClusterId = (int)ClusterID.NOISE;
            else
            {
                c++;
                ExpandCluster(allPointsDbscan, p, neighborPts, c, epsilon, minPts);
            }
        }
        clusters = new HashSet<PointData[]>(
            allPointsDbscan
            .Where(x => x.ClusterId > 0)
            .GroupBy(x => x.ClusterId)
            .Select(x => x.Select(y => y.ClusterPoint).ToArray())
        );
    }
}
