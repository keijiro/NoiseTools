using UnityEngine;

namespace Klak.Math
{
    public class CellularNoise
    {
        #region Private members

        int _freq;
        int _repeat;
        int _seed;

        int Repeat(int i)
        {
            i %= _repeat;
            if (i < 0) i += _repeat;
            return i;
        }

        float GetHash01(int id, int seed)
        {
            var hash = XXHash.GetHash(id, _seed + seed);
            return hash / (float)uint.MaxValue;
        }

        #endregion

        #region Constructor

        public CellularNoise(int frequency, int repeat, int seed = 0)
        {
            _freq = frequency;
            _repeat = repeat * frequency;
            _seed = seed;
        }

        #endregion

        #region 2D noise

        int GetCellID(int cx, int cy)
        {
            return Repeat(cy) * _repeat + Repeat(cx);
        }

        Vector2 PointInCell(int cx, int cy)
        {
            var id = GetCellID(cx, cy);
            return new Vector2(
                GetHash01(id, 0) + cx,
                GetHash01(id, 1) + cy
            );
        }

        public float Get(float x, float y)
        {
            return Get(new Vector2(x, y));
        }

        public float Get(Vector2 point)
        {
            point *= _freq;

            var cx = Mathf.FloorToInt(point.x);
            var cy = Mathf.FloorToInt(point.y);

            var minDist = 2.0f;
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var p = PointInCell(cx + i, cy + j);
                    var dist = Vector2.Distance(p, point);
                    minDist = Mathf.Min(minDist, dist);
                }
            }

            return minDist;
        }

        #endregion

        #region 3D noise

        int GetCellID(int cx, int cy, int cz)
        {
            return (Repeat(cz) * _repeat + Repeat(cy)) * _repeat + Repeat(cx);
        }

        Vector3 PointInCell(int cx, int cy, int cz)
        {
            var id = GetCellID(cx, cy, cz);
            return new Vector3(
                GetHash01(id, 0) + cx,
                GetHash01(id, 1) + cy,
                GetHash01(id, 2) + cz
            );
        }

        public float Get(float x, float y, float z)
        {
            return Get(new Vector3(x, y, z));
        }

        public float Get(Vector3 point)
        {
            point *= _freq;

            var cx = Mathf.FloorToInt(point.x);
            var cy = Mathf.FloorToInt(point.y);
            var cz = Mathf.FloorToInt(point.z);

            var minDist = 2.0f;
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    for (var k = -1; k <= 1; k++)
                    {
                        var p = PointInCell(cx + i, cy + j, cz + k);
                        var dist = Vector3.Distance(p, point);
                        minDist = Mathf.Min(minDist, dist);
                    }
                }
            }

            return minDist;
        }

        #endregion
    }
}
