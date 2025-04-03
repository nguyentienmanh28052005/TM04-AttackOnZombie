using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire1 : MonoBehaviour
{
    
    protected Animator _animator;
    protected GameObject _player;
    [SerializeField] private Camera _cam;
    //[SerializeField] private GameObject _bullet;
    private Vector3 _pos;
    private Rigidbody _rb;
    

    void Start()
    {
        _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _rb = GetComponent<Rigidbody>();
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            _pos = raycastHit.point;
        }
        MoveToGameObject(_pos);

    }

    // Update is called once per frame
    void Update()
    {
    }
    
    protected void MoveToGameObject(Vector3 _gameObject)
    {
        transform.LookAt(new Vector3(_gameObject.x + 20f, 1f, _gameObject.z + 20f));
        Vector3 _vector3 = new Vector3(_gameObject.x, 1f, _gameObject.z) - transform.position;
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(_gameObject.x + 20f, 1.4f, _gameObject.z + 20f), 10f * Time.deltaTime);
        //transform.Translate(_vector3 * Time.deltaTime, Space.World);
        //transform.position += _vector3 * Time.deltaTime * 10f;
        _rb.linearVelocity = _vector3.normalized * 10f;
    }
}
