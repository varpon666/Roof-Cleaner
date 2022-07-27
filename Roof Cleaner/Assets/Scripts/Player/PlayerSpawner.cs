using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPosition = new List<Transform>();

    [SerializeField] private PlayerX _player;

    private PlayerX _spawnedPlayer;

    public void Spawn()
    {
        if (_spawnPosition.Count == 0)
            return;

        _spawnedPlayer = Instantiate(_player, _spawnPosition[0].position, _player.transform.rotation);

        _spawnPosition.RemoveAt(0);

    }

    public void DestroyPlayer()
    {
        if (_spawnedPlayer == null)
            return;

        Destroy(_spawnedPlayer.gameObject); 
    }
}
