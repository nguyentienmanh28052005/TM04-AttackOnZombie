using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnObjectPrefab;
    private Transform spawnObjectTranform;
    
    private NetworkVariable<MyCustomData> random = new NetworkVariable<MyCustomData>(
        new MyCustomData()
        {
            _int = 1, _bool = false,
        },  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    };
    
    public override void OnNetworkSpawn()
    {
        random.OnValueChanged += (MyCustomData priviousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId  + "; randomNumber :  " + newValue._bool + ";" + newValue._int + ";" + newValue.message);
        };
    }
    
    void Update()
    {
        if(!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.T))
        {
            spawnObjectTranform = Instantiate(spawnObjectPrefab);
            spawnObjectTranform.GetComponent<NetworkObject>().Spawn(true);
            //TestClientRpc();
            // random.Value = new MyCustonData
            // {
            //     _int = 10, _bool = true,message = "hi",
            // };
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnObjectTranform.gameObject);
        }
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc()
    {
        Debug.Log("TestServerRpc" + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc");
    }
}

