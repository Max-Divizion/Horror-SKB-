using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent AI_Monster;
    private GameObject Player;

    public Transform[] WayPoints;
    public Transform target;
    public int Current_Patch;

    public enum AI_State { Patrol, Stay, Chase };
    public AI_State AI_Enemy;

    private Transform Last_Point;
    private bool Check_LastPoint;

    float i_stay;
    float DistPlayer;
    float Patch_dist;
    float PointDist;


    private void Start()
    {
        AI_Monster = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        OnFunctionCheckLastPoint();
    }

    /// <summary>
    /// Метод вызова ходьбы по чекпоинтам для пассивного состояние
    /// Также для обнаружения игрока и его преследования
    /// </summary>
    private void OnFunctionCheckLastPoint()
    {
        if (Check_LastPoint == false)
        {
            OnActionPatrol();
            OnActionChase();
        }
        else
        {
            AI_Monster.isStopped = false;
            i_stay += 1 * Time.deltaTime;
            PointDist = Vector3.Distance(Last_Point.transform.position, gameObject.transform.position);
            if (PointDist < 1 || i_stay >= 7)
            {
                Check_LastPoint = false;
                AI_Enemy = AI_State.Patrol;
                i_stay = 0;
            }
            else
                StartCoroutine(AnimationLookAroundAndPatrol());


        }
        DistPlayer = Vector3.Distance(Player.transform.position, gameObject.transform.position);
        if (DistPlayer < 2)
        {
            Player.SetActive(false);
            // Panel_GameOver.SetActive(true); В случае смерти ГГ появляется меню 
        }

    }

    /// <summary>
    /// Метод пассивной ходьбы по чекпоинтам 
    /// </summary>
    private void OnActionPatrol()
    {
        if (AI_Enemy == AI_State.Patrol)
        {
            AI_Monster.isStopped = false;
            gameObject.GetComponent<Animator>().SetBool("isPatrolling", true);
            AI_Monster.SetDestination(WayPoints[Current_Patch].transform.position);
            Patch_dist = Vector3.Distance(WayPoints[Current_Patch].transform.position, gameObject.transform.position);
            if (Patch_dist < 2)
            {
                Current_Patch++;
                Current_Patch = Current_Patch % WayPoints.Length;
            }
        }
    }

    /// <summary>
    /// Метод преследования и крика при обнаружении игрока
    /// </summary>
    private void OnActionChase()
    {
        if (AI_Enemy == AI_State.Chase)
        {
            StartCoroutine(AnimationScreamAndChase());
        }
    }

    /// <summary>
    /// Задержка на 4 секунды для работы анимации поиска после потери игрока из виду
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationLookAroundAndPatrol()
    {
        LookAround();
        yield return new WaitForSeconds(4f);
        OnPatrollingAnimation();
    }

    /// <summary>
    /// Задержки анимации преследования на 1.5 секунды для анимации крика при обнаружении игрока
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationScreamAndChase()
    {
        WaitWorkAnimationScream();
        yield return new WaitForSeconds(1.5f);
        WaitWorkAnimationChase();
    }
  
    /// <summary>
    /// Запуск анимации осмотра территории
    /// </summary>
    private void LookAround()
    {
        AI_Monster.isStopped = true;
        gameObject.GetComponent<Animator>().SetBool("isScream", false);
        gameObject.GetComponent<Animator>().SetBool("isLookAround", true);
    }

    /// <summary>
    /// Запуск анимации патруля по чекпоинтам(пассивное состояние)
    /// </summary>
    private void OnPatrollingAnimation()
    {
        AI_Monster.isStopped = false;
        gameObject.GetComponent<Animator>().SetBool("isLookAround", false);
        gameObject.GetComponent<Animator>().SetBool("isPatrolling", true);
    }

    /// <summary>
    /// Запуск анимации крика в сторону игрока
    /// </summary>
    private void WaitWorkAnimationScream()
    {
        AI_Monster.isStopped = true;
        transform.LookAt(target);
        gameObject.GetComponent<Animator>().SetBool("isScream", true);
    }

    /// <summary>
    /// Запуск анимации преследования игрока
    /// </summary>
    private void WaitWorkAnimationChase()
    {
        AI_Monster.isStopped = false;
        AI_Monster.speed = 5f;
        gameObject.GetComponent<Animator>().SetBool("isChasing", true);
        StateOfRest();
    }

    /// <summary>
    /// Метод для перехода от анимации преследования к анимации патруля
    /// </summary>
    private void StateOfRest()
    {
        if (gameObject.GetComponent<FieldOfView>().canSeePlayer == false)
        {
            Last_Point = Player.transform;
            Check_LastPoint = true;
            gameObject.GetComponent<Animator>().SetBool("isScream", false);
            gameObject.GetComponent<Animator>().SetBool("isChasing", false);
            AI_Monster.speed = 1f;
            AI_Monster.isStopped = false;
        }
        else
            AI_Monster.SetDestination(Player.transform.position);
    }
}
