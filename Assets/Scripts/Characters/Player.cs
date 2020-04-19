using System.Collections.Generic;
using UnityEngine;

public class Player : MovingCharacter
{
  /* Shadow Follower */
  public ShadowCameraTargetGroup shadowCameraTargetGroupPrefab;
  [HideInInspector] public ShadowCameraTargetGroup shadowCameraTargetGroup;

  /* Weapons */
  public List<Weapon> weapons;
  private Weapon activeWeapon;

  /* Player controls */
  private Controls controls;

  void Awake()
  {
    controls = new Controls();
    Vector3 weaponPosition = new Vector3(transform.position.x, transform.position.y + 0.03125f, transform.position.z);
    activeWeapon = Instantiate(weapons[0], weaponPosition, Quaternion.identity, transform);
    shadowCameraTargetGroup = Instantiate(shadowCameraTargetGroupPrefab, transform.position, Quaternion.identity);
    shadowCameraTargetGroup.player = gameObject;
  }

  void OnEnable()
  {
    controls.Enable();

    controls.Weapon.Attack.performed += ctx =>
    {
      Attack();
      holdAttack = ctx.ReadValue<float>() >= 0.9f;
    };
    controls.Player.Movement.performed += ctx => ChangeDirection(ctx.ReadValue<Vector2>());
  }

  void OnDisable() => controls.Disable();
  private void ChangeDirection(Vector2 newDirection) => Direction = newDirection;
  protected override void Move() => rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);
  protected override void Attack() => activeWeapon.Attack();
  public override void Die()
  {
    enabled = false;
    GameManager.instance.GameOver();
  }

}
