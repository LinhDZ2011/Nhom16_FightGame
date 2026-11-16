using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OpponentAI : MonoBehaviour
{
    [Header("Opponent Movement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Opponent Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public int attackCount = 0;
    public int randomNumber;
    public float attackRadius = 2f;
    public FightingController[] fightingController;
    public Transform[] players;
    public bool isTakingDamage;
    private float lastAttackTime;

    [Header("Effects and sounds")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;

    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.GiveFullHealth(currentHealth);
        }
        createRandomNumber();
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (fightingController == null || players == null)
            return;

        for (int i = 0; i < fightingController.Length; i++)
        {
            if (i >= players.Length || players[i] == null || fightingController[i] == null)
                continue;

            if (players[i].gameObject.activeSelf && Vector3.Distance(transform.position, players[i].position) <= attackRadius)
            {
                if (animator != null)
                {
                    animator.SetBool("Walking", false);
                }

                if (Time.time - lastAttackTime > attackCooldown)
                {
                    int randomAttackIndex = Random.Range(0, attackAnimations.Length);

                    if (!isTakingDamage)
                    {
                        PerformAttack(randomAttackIndex);
                    }
                    fightingController[i].StartCoroutine(fightingController[i].PlayHitDamageAnimation(attackDamage));
                }
                
            }
            else
            {
                if (players[i].gameObject.activeSelf)
                {
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    if (characterController != null)
                    {
                        characterController.Move(direction * movementSpeed * Time.deltaTime);
                    }

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    }

                    if (animator != null)
                    {
                        animator.SetBool("Walking", true);
                    }
                }
            }
        }
    }
    void PerformAttack(int attackIndex)
    {
        if (animator != null && attackAnimations != null && attackIndex >= 0 && attackIndex < attackAnimations.Length)
        {
            animator.Play(attackAnimations[attackIndex]);
        }

        int damage = attackDamage;
        Debug.Log("Performed attack" + (attackIndex + 1) + "dealing" + damage + "damage");

        lastAttackTime = Time.time;
    }

    void PerformDodgeFront()
    {
        if (animator != null)
        {
            animator.Play("DodgeFrontAnimation");
        }

        Vector3 dodgeDiretion = -transform.forward * dodgeDistance;

        if (characterController != null)
        {
            characterController.SimpleMove(dodgeDiretion);
        }
    }
    void createRandomNumber()
    {
        randomNumber = Random.Range(1, attackCount + 1);
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        yield return new WaitForSeconds(0.5f);

        if (hitSounds != null && hitSounds.Length > 0)
        {
            int randonIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randonIndex], transform.position);
        }

        currentHealth -= takeDamage;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (animator != null)
        {
            animator.Play("HitDamageAnimation");  
        }
    }

    void Die()
    {
        Debug.Log("Opponent died.");
    }


    public void Attack1Effect()
    {
        if (attack1Effect != null) attack1Effect.Play();
    }
    public void Attack2Effect()
    {
        if (attack2Effect != null) attack2Effect.Play();
    }
    public void Attack3Effect()
    {
        if (attack3Effect != null) attack3Effect.Play();
    }
    public void Attack4Effect()
    {
        if (attack4Effect != null) attack4Effect.Play();
    }

}