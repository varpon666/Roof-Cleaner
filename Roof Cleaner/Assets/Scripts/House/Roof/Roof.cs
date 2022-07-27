using System;
using System.Collections.Generic;
using UnityEngine;

public class Roof : MonoBehaviour
{
    public Action<Roof> OnCheckRoofClean;

    [SerializeField] private List<Dirt> _dirts = new List<Dirt>();

    [SerializeField] private float _turnAngle;

    public float TurnAngle => _turnAngle;

    private void OnEnable()
    {
        foreach(var dirt in _dirts)
        {
            dirt.OnRemoveDirt += RemoveDirt;
        }
    }

    private void OnDisable()
    {
        foreach (var dirt in _dirts)
        {
            dirt.OnRemoveDirt -= RemoveDirt;
        }
    }

    public void MoveToNextRoof(Roof roof)
    {
        OnCheckRoofClean?.Invoke(roof);
    }

    public void RemoveDirt(Dirt dirt)
    {
        if (_dirts.Contains(dirt))
        {
            _dirts.Remove(dirt);
            Destroy(dirt.gameObject);
        }

        if (_dirts.Count == 0)
        {
            MoveToNextRoof(this);
            return;
        }
    }
}
