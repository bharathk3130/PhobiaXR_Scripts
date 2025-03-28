using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircuitBoard : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Child] Rigidbody _rb;
    [SerializeField, Scene] CircuitBoardDemo _circuitBoardDemo;
    [field: SerializeField, Scene] public LiftSFXManager LiftSoundManager {get; private set; }
    [SerializeField] Connection _connectionPrefab;
    [SerializeField] Transform _graphicsTransform;
    [SerializeField] Transform _connectionSlots;
    [SerializeField] Transform _socketSlotsParent;

    [Header("Colours")]
    [SerializeField] ConnectionMaterials[] _connectionMaterials;

    int _wireCount = 4;
    Connection[] _connections;

    public Action OnConnectionChange = delegate { };
    [SerializeField] float _clearSFXStartingDelay = 0.15f;

    [Serializable]
    class ConnectionMaterials
    {
        public Material WireMat;
        public Material SocketMat;
    }

    void Awake()
    {
        _connections = new Connection[4];

        OnConnectionChange += ConnectionsChanged;
    }

    void Start()
    {
        if (_circuitBoardDemo.IsCompleted)
        {
            SpawnAllConnections();
        } else
        {
            _circuitBoardDemo.OnComplete += SpawnAllConnections;
        }
    }

    public void SpawnAllConnections()
    {
        List<int> socketSpawnIndices = new();
        for (int i = 0; i < _wireCount; i++)
        {
            socketSpawnIndices.Add(i);
        }

        List<ConnectionMaterials> connectionMaterialsList = _connectionMaterials.ToList();
        
        for (int i = 0; i < _wireCount; i++)
        {
            int socketSpawnIndex = socketSpawnIndices[Random.Range(0, socketSpawnIndices.Count)];
            socketSpawnIndices.Remove(socketSpawnIndex);

            ConnectionMaterials connectionMaterial;
            if (connectionMaterialsList.Count > 0)
            {
                connectionMaterial = connectionMaterialsList[Random.Range(0, connectionMaterialsList.Count)];
                connectionMaterialsList.Remove(connectionMaterial);
            } else
            {
                connectionMaterial = _connectionMaterials[Random.Range(0, _connectionMaterials.Length)];
            }
            
            _connections[i] = SpawnConnection(i, socketSpawnIndex, connectionMaterial);
        }
    }
    
    Connection SpawnConnection(int connectionSpawnLocationIndex, int socketSpawnLocationIndex, ConnectionMaterials connectionMaterial)
    {
        Transform spawnPoint = _connectionSlots.GetChild(connectionSpawnLocationIndex);
        Vector3 socketPos = _socketSlotsParent.GetChild(socketSpawnLocationIndex).position;
        Connection connection = Instantiate(_connectionPrefab, spawnPoint.position, spawnPoint.rotation, _graphicsTransform);
        
        connection.SetCircuitBoard(this);
        connection.Initialize(socketPos, connectionMaterial.WireMat, connectionMaterial.SocketMat);

        return connection;
    }

    void ConnectionsChanged()
    {
        bool connectionsComplete = true;
        foreach (Connection connection in _connections)
        {
            if (!connection.IsConnected)
            {
                connectionsComplete = false;
            }
        }

        if (connectionsComplete)
        {
            Invoke(nameof(PlayClearBoardSFX), 2 - _clearSFXStartingDelay);
            Invoke(nameof(Reset), 2);
        }
    }
    
    void PlayClearBoardSFX() => LiftSoundManager.CircuitBoardClearingSFX(transform.position);

    void Reset()
    {
        foreach (Connection connection in _connections)
        {
            Destroy(connection.gameObject);
        }
        
        _connections = new Connection[4];
        SpawnAllConnections();
    }

    public void OnLetGo() => StartCoroutine(ResetMovementDelayed());

    void OnCollisionExit(Collision other)
    {
        StartCoroutine(ResetMovementDelayed());
    }

    IEnumerator ResetMovementDelayed()
    {
        yield return new WaitForEndOfFrame();
        ResetMovement();
    }

    void ResetMovement()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.Sleep();
    }
}