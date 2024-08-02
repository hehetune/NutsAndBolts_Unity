using System;
using UnityEngine;
using DG.Tweening;
using ObserverPattern;

public class Nut : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _model;
    [SerializeField] private Sprite _upSprite;
    [SerializeField] private Sprite _downSprite;
    [SerializeField] private Collider2D _col;
    [SerializeField] private SpriteMask _spriteMask;

    private bool _up = false;
    private Hole _curHole = null;

    public Hole CurHole
    {
        get => this._curHole;
        set => this._curHole = value;
    }

    [SerializeField] private float _yUp = 0.1f;
    [SerializeField] private float _yDown = 0.1f;

    public bool IsUp => this._up;

    public void SelectThis()
    {
        this.ToggleNut();
    }

    public void SelectHole(Hole hole)
    {
        if (this._curHole == hole)
        {
            this.ToggleNut();
        }
        else if (this._up)
        {
            this.MoveToHole(hole);
        }
    }

    private void ToggleNut(params Action[] onComplete)
    {
        this._up = !this._up;
        if (this._up)
        {
            this._spriteRenderer.sprite = this._upSprite;
            this._spriteMask.enabled = true;
            _model.DOMoveY(_yUp, 0.1f).OnComplete(() => { this._spriteMask.enabled = false; });
        }
        else
        {
            this._spriteMask.enabled = true;
            _model.DOMoveY(_yDown, 0.1f).OnComplete(() =>
            {
                this._spriteRenderer.sprite = this._downSprite;
                this._spriteMask.enabled = false;
            });
        }
        //
        // transform.DORotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360)
        //     .SetEase(Ease.Linear)
        //     .SetLoops(-1, LoopType.Incremental);
    }

    private void MoveToHole(Hole hole)
    {
        this._col.enabled = false;
        transform.DOMove(hole.transform.position, 0.2f).OnComplete(() =>
        {
            this._curHole.SetNut(null);

            ToggleNut();

            hole.SetNut(this);
            this._curHole = hole;

            this._col.enabled = true;
        });
    }
}