using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    private Nut _currentNut;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPosition2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition2D, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                Hole hole = hit.transform.GetComponent<Hole>();
                if (hole != null)
                {
                    hole.SelectThis(this);
                    break;
                }
            }
        }
    }

    public void SelectHole(Hole hole)
    {
        if (hole == null || _currentNut == null) return;
        _currentNut.SelectHole(hole);
    }

    public void SelectNut(Nut nut)
    {
        if (nut == null) return;
        _currentNut = nut;
        _currentNut.SelectThis();
    }
}