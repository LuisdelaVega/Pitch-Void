﻿using System;
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
  public Weapon activeWeapon;

  /* Dash */
  [SerializeField] private float dashSpeed = 50f;
  [SerializeField] private float dashTime = 0.05f;
  private bool isDashing = false;
  private bool dashOnCooldown = false;
  [SerializeField] private float dashCooldownTimer = 1f;
  public GameObject echo;
  [SerializeField] private float echoLifeSpan = 0.3f;
  public static event Action<float> OnDash;

  /* Player controls */
  public Controls controls;

  /* AFK */
  private float AFKTimer = 0;
  [SerializeField] private float AFKTime = 15f;

  void Awake()
  {
    controls = new Controls();
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
    controls.Player.Movement.canceled += _ => ChangeDirection(Vector2.zero);

    controls.Player.Dash.performed += _ => Dash();
  }

  void OnDisable() => controls.Disable();

  private void ChangeDirection(Vector2 newDirection) => Direction = newDirection.normalized;
  protected override void Move()
  {
    if (Direction.sqrMagnitude == 0)
      AFKTimer += Time.fixedDeltaTime;
    else
      AFKTimer = 0;

    if (AFKTimer >= AFKTime)
    {
      animator.SetTrigger("AFK");
      AFKTimer = 0;
    }

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
      GameObject echoObj = Instantiate(echo, transform.position, transform.rotation);
      SpriteRenderer echoObjSR = echoObj.GetComponent<SpriteRenderer>();
      echoObjSR.sprite = spriteRenderer.sprite;
      echoObjSR.flipX = spriteRenderer.flipX;

      Destroy(echoObj, echoLifeSpan);
      yield return null;
    }
  }

  protected override void Attack() => activeWeapon.Attack();

  public override void Bleed(Quaternion rotation)
  {
    Instantiate(bloodParticleEffect, transform.position, rotation);
    Instantiate(bloodStain, transform.position, rotation);
  }

  public override void Die(Quaternion rotation)
  {
    AudioManager.instance.Play("Death");
    enabled = false;
    GameManager.instance.GameOver();
    Destroy(gameObject);
  }
}
