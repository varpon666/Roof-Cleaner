using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private List<Roof> _roofs = new List<Roof>();

    [SerializeField] private PlayerSpawner _playerSpawner;

    [SerializeField] private float _speedRotation;

    private void OnEnable()
    {
        foreach (var roof in _roofs)
        {
            roof.OnCheckRoofClean += CheckAvailabilityRoofs;
        }
    }

    private void OnDisable()
    {
        foreach (var roof in _roofs)
        {
            roof.OnCheckRoofClean -= CheckAvailabilityRoofs;
        }
    }

    private void Start()
    {
        _playerSpawner.Spawn();
    }

    public void CheckAvailabilityRoofs(Roof roof)
    {
        if (_roofs.Count > 1)
        {
            MoveToNextRoof();
            _roofs.RemoveAt(0);
            return;
        }

        Win();
    }

    private void MoveToNextRoof()
    {
        Debug.Log("MoveToNextRoof");

        StartCoroutine(RotateHouse());
    }

    private void Win()
    {
        Debug.Log("Win");

        _playerSpawner.DestroyPlayer();
    }

    private IEnumerator RotateHouse()
    {
        _playerSpawner.DestroyPlayer();

        yield return new WaitForSeconds(0.5f);

        while(transform.eulerAngles.y != _roofs[0].TurnAngle)
        {
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles,
                new Vector3(transform.eulerAngles.x, _roofs[0].TurnAngle, transform.eulerAngles.z),
                Time.deltaTime * _speedRotation);

            yield return null;
        }

        _playerSpawner.Spawn();
    }
}
