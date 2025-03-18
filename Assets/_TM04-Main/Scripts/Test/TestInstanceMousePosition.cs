using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInstanceMousePosition : MonoBehaviour
{

    private GameObject _gameObject;

    [SerializeField] private Camera yourCam;

    [SerializeField] private List<GameObject> _gameObjects;

    [SerializeField] private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = yourCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycast))
        {
        //            Debug.Log(raycast.point);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                //int index = Random.Range(0, 6);
                //Instantiate(_gameObjects[1], new Vector3(raycastHit.point.x, 0, raycastHit.point.z), Quaternion.identity);
                PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, new Vector3(raycastHit.point.x, 0, raycastHit.point.z));
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Instantiate(_gameObjects[0], new Vector3(_player.transform.position.x, 1f, _player.transform.position.z), _player.transform.rotation);
        }
    }
}
