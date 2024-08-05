using System;
using ObserverPattern;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private Nut _currentNut = null;

    private void Start()
    {
        CreateJointForNuts();
    }

    private void CreateJointForNuts()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Nut nut = hit.transform.GetComponent<Nut>();
            if (nut != null)
            {
                AssignNut(nut);
                break;
            }
        }
    }

    public void AssignNut(Nut nut)
    {
        _currentNut = nut;
    }

    public void SelectThis(PlayerController playerController)
    {
        if (_currentNut && !_currentNut.IsUp)
            playerController.SelectNut(_currentNut);
        else
            playerController.SelectHole(this);
    }

    

    public void RemoveNut()
    {
        _currentNut = null;
    }
}