using ObserverPattern;
using Unity.VisualScripting;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    private Nut _curNut;

    private void Awake()
    {
        // Subject.Register(this, EventKey.HoleSelect);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Kiểm tra click chuột trái
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorldPosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition2D, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                Hole hole = hit.transform.GetComponent<Hole>();
                if (!hole) continue;

                Debug.Log("???");
                hole.SelectThis(this);
                break; // Dừng vòng lặp sau khi tìm thấy đối tượng cần xử lý
            }
        }
    }

    // public void OnNotify(EventKey key, params object[] args)
    // {
    //     switch (key)
    //     {
    //         case EventKey.HoleSelect:
    //             this.SelectHole((Hole)args[0]);
    //             break;
    //         case EventKey.NutSelect:
    //             this.SelectHole((Hole)args[0]);
    //             break;
    //     }
    // }

    public void SelectHole(Hole hole)
    {
        if (!hole) return;
        if (!this._curNut) return;

        this._curNut.SelectHole(hole);
    }

    public void SelectNut(Nut nut)
    {
        if (!nut) return;

        this._curNut = nut;
        this._curNut.SelectThis();
    }
}