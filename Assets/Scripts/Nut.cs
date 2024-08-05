using UnityEngine;
using DG.Tweening;

public class Nut : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _model;
    [SerializeField] private Sprite _upSprite;
    [SerializeField] private Sprite _downSprite;
    [SerializeField] private SpriteMask _spriteMask;
    [SerializeField] private LayerMask _boltLayerMask;
    [SerializeField] private LayerMask _holeLayerMask;

    [SerializeField] private GameObject _hingeJointPrefab;

    private bool _isUp = false;
    private Hole _currentHole = null;

    public Hole CurrentHole
    {
        get => _currentHole;
        set => _currentHole = value;
    }

    [SerializeField] private float _yUp = 0.1f;
    [SerializeField] private float _yDown = 0.1f;

    public bool IsUp => _isUp;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        DetectHole(GetRaycastHits(_holeLayerMask));
        DetectBolts(GetRaycastHits(_boltLayerMask));
    }

    private void DetectBolts(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (!hit.transform.CompareTag($"Bolt")) continue;
            Debug.Log("???");

            Rigidbody2D rb = hit.transform.GetComponent<Rigidbody2D>();
            Instantiate(_hingeJointPrefab, transform);
            _hingeJointPrefab.GetComponent<HingeJoint2D>().connectedBody = rb;
        }
    }

    private void DetectHole(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (!hit.transform.CompareTag($"Hole")) continue;
            Hole hole = hit.transform.GetComponent<Hole>();
            if (hole != null)
            {
                hole.AssignNut(this);
                _currentHole = hole;
                break;
            }
        }
    }

    private RaycastHit2D[] GetRaycastHits(LayerMask layerMask)
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        return Physics2D.RaycastAll(position, Vector2.zero, layerMask);
    }

    public void SelectThis()
    {
        ToggleNutState();
    }

    public void SelectHole(Hole hole)
    {
        if (_currentHole == hole)
        {
            ToggleNutState();
        }
        else if (_isUp)
        {
            MoveToHole(hole);
        }
    }

    private void ToggleNutState()
    {
        _isUp = !_isUp;
        if (_isUp)
        {
            SetNutSprite(_upSprite, _yUp);
        }
        else
        {
            SetNutSprite(_downSprite, _yDown);
        }
    }

    private void SetNutSprite(Sprite sprite, float positionY)
    {
        _spriteRenderer.sprite = sprite;
        _spriteMask.enabled = true;
        _model.DOLocalMoveY(positionY, 0.1f).OnComplete(() => { _spriteMask.enabled = false; });
    }

    private void MoveToHole(Hole hole)
    {
        _currentHole.RemoveNut();
        RemoveJoints();
        transform.DOMove(hole.transform.position, 0.2f).OnComplete(() =>
        {
            ToggleNutState();
            hole.AssignNut(this);
            DetectBolts(GetRaycastHits(_boltLayerMask));
            _currentHole = hole;
        });
    }

    private void RemoveJoints()
    {
        foreach (Transform t in this.transform)
        {
            if (!t.GetComponent<HingeJoint2D>()) continue;
            Destroy(t.gameObject);
        }
    }
}