using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingCharacter
{
  /* Shadow Follower */
  public ShadowCameraTargetGroup shadowCameraTargetGroupPrefab;
  [HideInInspector] public ShadowCameraTargetGroup shadowCameraTargetGroup;

  /* Weapons */
  public List<Weapon> weapons;
  private Weapon activeWeapon;

  /* Dash */
  [SerializeField] private float dashSpeed = 50f;
  [SerializeField] private float dashTime = 0.05f;
  private bool isDashing = false;
  private bool dashOnCooldown = false;
  [SerializeField] private float dashCooldownTimer = 1f;
  public GameObject echo;
  [SerializeField] private float echoLifeSpan = 0.3f;

  /* Player controls */
  private Controls controls;

  /* Dash */
  public static event Action<float> OnDash;

  void Awake()
  {
    controls = new Controls();
    Vector3 weaponPosition = new Vector3(transform.position.x, transform.position.y + 0.03125f, transform.position.z);
    activeWeapon = Instantiate(weapons[0], weaponPosition, Quaternion.identity, transform);
    shadowCameraTargetGroup = Instantiate(shadowCameraTargetGroupPrefab, transform.position, Quaternion.identity);
    shadowCameraTargetGroup.player = gameObject;
  }

  public void EnablePlayerControls()
  {
    controls.Enable();

    controls.Weapon.Attack.performed += ctx =>
    {
      Attack();
      holdAttack = ctx.ReadValue<float>() >= 0.9f;
    };
    controls.Player.Movement.performed += ctx => ChangeDirection(ctx.ReadValue<Vector2>());

    controls.Player.Dash.performed += _ => Dash();
  }

  public void DisablePlayerControls() => controls.Disable();
  void OnDisable() => DisablePlayerControls();

  private void ChangeDirection(Vector2 newDirection) => Direction = newDirection.normalized;
  protected override void Move()
  {
    animator.SetFloat("Speed", Direction.sqrMagnitude);
    rb.velocity = Direction * (!isDashing ? moveSpeed : dashSpeed);
  }
  private void Dash()
  {
    if (!dashOnCooldown && Direction.sqrMagnitude > 0)
    {
      StartCoroutine(DashTimer());
      StartCoroutine(CreateEcho());
    }
  }

  private IEnumerator DashTimer()
  {
    isDashing = true;
    yield return new WaitForSeconds(dashTime);
    isDashing = false;
    StartCoroutine(DashCooldown());
  }

  private IEnumerator DashCooldown()
  {
    dashOnCooldown = true;
    OnDash?.Invoke(dashCooldownTimer);
    yield return new WaitForSeconds(dashCooldownTimer);
    dashOnCooldown = false;
  }

  private IEnumerator CreateEcho()
  {
    while (isDashing)
    {
      Destroy(Instantiate(echo, transform.position, transform.rotation), echoLifeSpan);
      yield return null;
    }
  }

  // public void SayName(string name) => floatingText.CreateFloatingText(transform, $"{name} reporting for duty!");

  protected override void Attack() => activeWeapon.Attack();
  public override void Die(Quaternion rotation)
  {
    Instantiate(bloodParticleEffect, transform.position, rotation);
    Instantiate(bloodStain, transform.position, rotation);
    AudioManager.instance.Play("Death");
    enabled = false;
    GameManager.instance.GameOver();
  }
}
