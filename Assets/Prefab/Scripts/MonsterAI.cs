using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Theme.Primitives;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent AI_Monster;
    private GameObject Player;
      
    public Transform[] WayPoints;
    public int Current_Patch;

    public enum AI_State {Patrol, Stay, Chase};
    public AI_State AI_Enemy;

    private Transform Last_Point;
    private bool Check_LastPoint;
    float i_stay;

    private void Start()
    {
        AI_Monster = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (Check_LastPoint == false)
        {
            if (AI_Enemy == AI_State.Patrol)
            {
                AI_Monster.Resume();
                gameObject.GetComponent<Animator>().SetBool("isPatrolling", true);
                AI_Monster.SetDestination(WayPoints[Current_Patch].transform.position);
                float Patch_dist = Vector3.Distance(WayPoints[Current_Patch].transform.position, gameObject.transform.position);
                if (Patch_dist < 2)
                {
                    Current_Patch++;
                    Current_Patch = Current_Patch % WayPoints.Length;
                }
            }
            if (AI_Enemy == AI_State.Stay)
            {
                gameObject.GetComponent<Animator>().SetBool("isPatrolling", false);
                AI_Monster.Stop();
            }
            if (AI_Enemy == AI_State.Chase)
            {
                AI_Monster.Resume();
                gameObject.GetComponent<Animator>().SetBool("isPatrolling", false);
                gameObject.GetComponent<Animator>().SetBool("isChasing", true);

                if (gameObject.GetComponent<FieldOfView>().canSeePlayer == false)
                {
                    Last_Point = Player.transform;
                    Check_LastPoint = true;
                }
                else
                    AI_Monster.SetDestination(Player.transform.position);
            }
        }
        else
        {
            AI_Monster.Resume();
            i_stay += 1 * Time.deltaTime;
            float PointDist = Vector3.Distance(Last_Point.transform.position, gameObject.transform.position);
            if (PointDist < 1 || i_stay >= 7)
            {
                Check_LastPoint = false;
                AI_Enemy = AI_State.Patrol;
                i_stay = 0;
            }
            else            
                gameObject.GetComponent<Animator>().SetBool("isPatrolling", true);
        }
        float DistPlayer = Vector3.Distance(Player.transform.position, gameObject.transform.position);
        if (DistPlayer < 2)
        {
            Player.SetActive(false);
           // Panel_GameOver.SetActive(true); В случае смерти ГГ появляется меню 
        }
    }
}
