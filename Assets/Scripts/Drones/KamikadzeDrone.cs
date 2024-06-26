using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KamikadzeDrone : MonoBehaviour
{
    [SerializeField] private float lightSpeed;
    [SerializeField] private float lightAttackPower;
    [SerializeField] private float droneHeight;
    [SerializeField] private float attackRadius;

    // private float enemyHealth; на будущее, чтобы сконектить с скриптом Матвея
    private PlayerBase _playerBase;
    private Rigidbody _enemyRb;

    void Start()
    {
        droneHeight = transform.position.y;
        _playerBase = GameObject.Find("Player Base").GetComponent<PlayerBase>();
        _enemyRb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        Move();
        // PlayEffect();
    }
    
    private void Move()
    {
        var playerPosition = _playerBase.transform.position;
        var targetPosition = new Vector3(playerPosition.x, droneHeight, playerPosition.z);
        var newPosition = Vector3.MoveTowards(transform.position, targetPosition, lightSpeed * Time.deltaTime);
        _enemyRb.MovePosition(newPosition);
    }
    
    // private void PlayEffect()
    // {
    //     if (enemyHealth.health != 0)
    //     {
    //         GetComponentInChildren<ParticleSystem>().Play();
    //     }
    // }

    private void AttackRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius); // Получаем все коллайдеры в радиусе атаки
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Attack(collider.gameObject);
            }
        }
    }
    
    private void Attack(GameObject target)
    {
        var player = target.GetComponent<PlayerBase>();
        if (player != null)
        {
            player.ReceiveDamage(-lightAttackPower);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Отрисовка радиуса атаки в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}