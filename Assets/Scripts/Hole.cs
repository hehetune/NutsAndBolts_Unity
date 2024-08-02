using System;
using ObserverPattern;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private Nut _curNut = null;

    private void Start()
    {
        DetectNutInside();
    }

    private void DetectNutInside()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y); // Vị trí của game object
        Vector2 direction = Vector2.zero; // Hướng của ray

        // Cast một ray từ vị trí của game object theo hướng
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction);

        foreach (RaycastHit2D hit in hits)
        {
            Nut nut = hit.transform.GetComponent<Nut>();
            if (nut != null)
            {
                this._curNut = nut;
                this._curNut.CurHole = this;
                break; // Dừng vòng lặp nếu tìm thấy đối tượng
            }
        }
    }

    public void SelectThis(PlayerController playerController)
    {
        if (this._curNut && !this._curNut.IsUp) playerController.SelectNut(this._curNut);
        else playerController.SelectHole(this);
    }

    public void SetNut(Nut nut)
    {
        this._curNut = nut;
    }
}