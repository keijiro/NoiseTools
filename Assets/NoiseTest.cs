using UnityEngine;

[ExecuteInEditMode]
public class NoiseTest : MonoBehaviour
{
    [SerializeField, Range(2, 3)]
    int _dimensions = 2;

    [SerializeField, Range(64, 256)]
    int _resolution = 64;

    [SerializeField, Range(1, 10)]
    int _frequency = 10;

    Texture2D _texture;

    void OnEnable()
    {
        _texture = new Texture2D(_resolution, _resolution);
        _texture.hideFlags = HideFlags.DontSave;

        ResetTexture();
    }

    void OnDisable()
    {
        DestroyImmediate(_texture);
        _texture = null;
    }

    void Update()
    {
        ResetTexture();
    }

    void OnGUI()
    {
        var w = Screen.height / 2;
        GUI.DrawTexture(new Rect(0, 0, w, w), _texture);
        GUI.DrawTexture(new Rect(w, 0, w, w), _texture);
        GUI.DrawTexture(new Rect(0, w, w, w), _texture);
        GUI.DrawTexture(new Rect(w, w, w, w), _texture);
    }

    void ResetTexture()
    {
        if (_texture.width != _resolution)
            _texture.Resize(_resolution, _resolution);

        var noise = new Klak.Math.CellularNoise(_frequency, 1, 0);

        var scale = 1.0f / _resolution;
        var z = Time.time * 0.1f;

        for (var iy = 0; iy < _resolution; iy++)
        {
            var y = scale * iy;

            for (var ix = 0; ix < _resolution; ix++)
            {
                var x = scale * ix;
                var c = _dimensions == 2 ? noise.Get(x, y) : noise.Get(x, y, z);
                _texture.SetPixel(ix, iy, new Color(c, c, c));
            }
        }

        _texture.Apply();
    }
}
