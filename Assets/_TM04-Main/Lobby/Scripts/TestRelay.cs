using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : NetworkBehaviour
{
    public static TestRelay Instance { get; private set; }
    private void Awake() { Instance = this; }
   private async void Start()
   {
       
   }

   public async Task<string> CreateRelay()
   {
       try
       {
           Allocation  allocation = await RelayService.Instance.CreateAllocationAsync(100);

           string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
           
           Debug.Log(joinCode);

           RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
           NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
           NetworkManager.Singleton.StartHost();
           return joinCode;
       }
       catch (RelayServiceException e)
       {
           Debug.Log(e);
           return null;
       }
   }

   public async void JoinRelay(string joinCode)
   {
       try
       {
           Debug.Log("Join in relay with" +  joinCode);
           JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
           
           RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
           NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

           NetworkManager.Singleton.StartClient();
       }
       catch (Exception e)
       {
           Debug.Log(e);
       }
   }
}

