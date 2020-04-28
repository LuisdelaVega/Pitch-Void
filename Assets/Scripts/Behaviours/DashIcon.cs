using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashIcon : MonoBehaviour
{
  private Image dashIcon;

  private void Start() => dashIcon = GetComponent<Image>();
  private void OnEnable() => Player.OnDash += Cooldown;
  private void OnDisable() => Player.OnDash -= Cooldown;
  private void Cooldown(float cooldownTimer) => StartCoroutine(PerformCooldown(cooldownTimer));

  private IEnumerator PerformCooldown(float cooldownTimer)
  {
    dashIcon.color = new Color(255, 0, 0);
    yield return new WaitForSeconds(cooldownTimer);
    dashIcon.color = new Color(255, 255, 255);
  }
}
