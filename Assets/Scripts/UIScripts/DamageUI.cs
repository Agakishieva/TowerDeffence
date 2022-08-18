using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    public static DamageUI sharedInstance;

    private class ActiveText
    {
        public TextMeshProUGUI UIText;
        public float maxTime;
        public float timer;
        public Vector3 unitPosition;

        public void MoveText(Camera camera)
        {
            float delta = 1.0f - (timer / maxTime);
            Vector3 position = unitPosition + new Vector3(delta, delta, 0.0f);
            position = camera.WorldToScreenPoint(position);
            position.z = 0.0f;

            UIText.transform.position = position;
        }
    }

    [SerializeField] private TextMeshProUGUI _textPrefab;
    private const int poolSize = 64;

    private Queue<TextMeshProUGUI> _textPool = new Queue<TextMeshProUGUI>();
    private List<ActiveText> _activeText = new List<ActiveText>();

    private Camera _camera;
    private Transform _transform;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        _camera = Camera.main;
        _transform = transform;

        for (int i = 0; i < poolSize; i++)
        {
            var tempText = Instantiate(_textPrefab, _transform);
            tempText.gameObject.SetActive(false);
            _textPool.Enqueue(tempText);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _activeText.Count; i++)
        {
            ActiveText at = _activeText[i];
            at.timer -= Time.deltaTime;

            if(at.timer <= 0.0f)
            {
                at.UIText.gameObject.SetActive(false);
                _textPool.Enqueue(at.UIText);
                _activeText.RemoveAt(i);
                --i;
            }
            else
            {
                var color = at.UIText.color;
                color.a = at.timer / at.maxTime;
                at.UIText.color = color;
                at.MoveText(_camera);
            }
        }
    }

    public void AddText(int amount, Vector3 unitPosition)
    {
        if (_textPool.Count == 0)
        {
            return;
        }

        var text = _textPool.Dequeue();
        text.text = amount.ToString();
        text.gameObject.SetActive(true);

        ActiveText at = new ActiveText() { maxTime = 1.0f };
        at.timer = at.maxTime;
        at.UIText = text;
        at.unitPosition = unitPosition + Vector3.up;

        at.MoveText(_camera);
        _activeText.Add(at);
    }
}
