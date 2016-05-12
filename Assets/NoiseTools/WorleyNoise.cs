using UnityEngine;

namespace NoiseTools
{
    public class WorleyNoise
    {
        #region Public members

        public WorleyNoise(int frequency, int repeat, int seed = 0)
        {
            _freq = frequency;
            _repeat = repeat * frequency;
            _seed = seed;
        }

        public float GetAt(float x, float y)
        {
            return Calculate2D(new Vector2(x, y));
        }

        public float GetAt(Vector2 point)
        {
            return Calculate2D(point);
        }

        public float GetAt(float x, float y, float z)
        {
            return Calculate3D(new Vector3(x, y, z));
        }

        public float GetAt(Vector3 point)
        {
            return Calculate3D(point);
        }

        public float GetFractal(float x, float y, int level)
        {
            return Calculate2DFractal(new Vector2(x, y), level);
        }

        public float GetFractal(Vector2 point, int level)
        {
            return Calculate2DFractal(point, level);
        }

        public float GetFractal(float x, float y, float z, int level)
        {
            return Calculate3DFractal(new Vector3(x, y, z), level);
        }

        public float GetFractal(Vector3 point, int level)
        {
            return Calculate3DFractal(point, level);
        }

        #endregion

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

        float Hash(int id, int seed)
        {
            var hash = XXHash.GetHash(id, _seed + seed);
            return hash / (float)uint.MaxValue;
        }

        #endregion

        #region 2D noise

        int CellID(int cx, int cy)
        {
            return Repeat(cy) * _repeat + Repeat(cx);
        }

        Vector2 Feature(int cx, int cy)
        {
            var id = CellID(cx, cy);
            return new Vector2(
                Hash(id, 0) + cx,
                Hash(id, 1) + cy
            );
        }

        float DistanceToFeature(Vector2 p, int cx, int cy)
        {
            return Vector2.Distance(p, Feature(cx, cy));
        }

        float Calculate2D(Vector2 point)
        {
            point *= _freq;

            var cx = Mathf.FloorToInt(point.x);
            var cy = Mathf.FloorToInt(point.y);

            var d = DistanceToFeature(point, cx, cy);

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy - 1));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy    ));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy    ));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy + 1));

            return d;
        }

        float Calculate2DFractal(Vector2 point, int level)
        {
            var originalFreq = _freq;
            var originalRepeat = _repeat;

            var sum = 0.0f;
            var w = 0.5f;

            for (var i = 0; i < level; i++)
            {
                sum += Calculate2D(point) * w;
                _freq *= 2;
                _repeat *= 2;
                w *= 0.5f;
            }

            _freq = originalFreq;
            _repeat = originalRepeat;

            return sum;
        }

        #endregion

        #region 3D noise

        int CellID(int cx, int cy, int cz)
        {
            return (Repeat(cz) * _repeat + Repeat(cy)) * _repeat + Repeat(cx);
        }

        Vector3 Feature(int cx, int cy, int cz)
        {
            var id = CellID(cx, cy, cz);
            return new Vector3(
                Hash(id, 0) + cx,
                Hash(id, 1) + cy,
                Hash(id, 2) + cz
            );
        }

        float DistanceToFeature(Vector3 p, int cx, int cy, int cz)
        {
            return Vector3.Distance(p, Feature(cx, cy, cz));
        }

        float Calculate3D(Vector3 point)
        {
            point *= _freq;

            var cx = Mathf.FloorToInt(point.x);
            var cy = Mathf.FloorToInt(point.y);
            var cz = Mathf.FloorToInt(point.z);

            var d = DistanceToFeature(point, cx, cy, cz);

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy - 1, cz - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy - 1, cz - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy - 1, cz - 1));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy    , cz - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy    , cz - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy    , cz - 1));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy + 1, cz - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy + 1, cz - 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy + 1, cz - 1));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy - 1, cz));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy - 1, cz));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy - 1, cz));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy    , cz));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy    , cz));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy + 1, cz));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy + 1, cz));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy + 1, cz));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy - 1, cz + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy - 1, cz + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy - 1, cz + 1));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy    , cz + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy    , cz + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy    , cz + 1));

            d = Mathf.Min(d, DistanceToFeature(point, cx - 1, cy + 1, cz + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx    , cy + 1, cz + 1));
            d = Mathf.Min(d, DistanceToFeature(point, cx + 1, cy + 1, cz + 1));

            return d;
        }

        float Calculate3DFractal(Vector3 point, int level)
        {
            var originalFreq = _freq;
            var originalRepeat = _repeat;

            var sum = 0.0f;
            var w = 0.5f;

            for (var i = 0; i < level; i++)
            {
                sum += Calculate3D(point) * w;
                _freq *= 2;
                _repeat *= 2;
                w *= 0.5f;
            }

            _freq = originalFreq;
            _repeat = originalRepeat;

            return sum;
        }

        #endregion
    }
}
