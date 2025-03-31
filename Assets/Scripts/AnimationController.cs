using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimationController : MonoBehaviour
{
    private Image _image;
    private Sprite[] _frames;
    private float _speed;
    private float _timer;
    private int _currentIndex;
    private bool _isAnimating;

    public void Initialize(Sprite[] frames, float speed)
    {
        _image = GetComponent<Image>();
        _frames = frames;
        _speed = speed;
        
        if (frames != null && frames.Length > 0)
        {
            _image.sprite = frames[0];
            _isAnimating = frames.Length > 1;
        }
    }

    private void Update()
    {
        if (!_isAnimating || _frames == null || _frames.Length <= 1) return;
        
        _timer += Time.deltaTime;
        
        if (_timer >= _speed)
        {
            _timer = 0;
            _currentIndex = (_currentIndex + 1) % _frames.Length;
            _image.sprite = _frames[_currentIndex];
        }
    }
}
