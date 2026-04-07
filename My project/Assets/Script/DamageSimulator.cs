using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDispay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    // --- [새로 추가된 통계 변수] ---
    private int weakPointCount = 0;   // 약점 공격 횟수
    private int missCount = 0;        // 명중 실패 횟수
    private int totalCritCount = 0;   // 전체 크리티컬 횟수
    private float maxDamage = 0f;     // 최대 데미지 기록
    // ------------------------------

    void Start()
    {
        SetWeapon(0); // 시작 시 단검 장착
    }

    private void Resetdata()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;

        // 통계 데이터도 초기화
        weakPointCount = 0;
        missCount = 0;
        totalCritCount = 0;
        maxDamage = 0f;
    }

    public void SetWeapon(int id)
    {
        Resetdata();
        if (id == 0) SetStats("단검", 0.1f, 0.4f, 1.5f);
        else if (id == 1) SetStats("장검", 0.2f, 0.3f, 2.0f);
        else SetStats("도끼", 0.3f, 0.2f, 3.0f);

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        Resetdata(); // 레벨업 시 통계 리셋 (필요에 따라 유지 가능)
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    // --- [공격 로직 통합 및 분리] ---

    // 버튼 연결용: 1회 공격
    public void OnAttack()
    {
        ProcessSingleAttack();
        UpdateUI();
    }

    // 버튼 연결용: 1000회 연속 공격
    public void OnAttack1000()
    {
        for (int i = 0; i < 1000; i++)
        {
            ProcessSingleAttack();
        }
        logDisplay.text = "<color=yellow>1,000회 연속 공격 완료!</color>";
        UpdateUI();
    }

    // 핵심 판정 로직 (명중 실패, 약점 공격, 크리티컬)
    private void ProcessSingleAttack()
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        float finalDamage = 0;
        bool isCrit = false;
        bool isWeakPoint = false;
        bool isMiss = false;

        // 1. 명중 실패 판정 (평균 - 2σ 미만)
        if (normalDamage < baseDamage - (2 * sd))
        {
            isMiss = true;
            finalDamage = 0;
            missCount++;
        }
        // 2. 약점 공격 판정 (평균 + 2σ 초과)
        else if (normalDamage > baseDamage + (2 * sd))
        {
            isWeakPoint = true;
            float weakDamage = normalDamage * 2f; // 약점 2배 적용

            isCrit = Random.value < critRate;
            finalDamage = isCrit ? weakDamage * critMult : weakDamage;

            weakPointCount++;
            if (isCrit) totalCritCount++;
        }
        // 3. 일반 공격 판정
        else
        {
            isCrit = Random.value < critRate;
            finalDamage = isCrit ? normalDamage * critMult : normalDamage;

            if (isCrit) totalCritCount++;
        }

        // 결과 누적
        attackCount++;
        totalDamage += finalDamage;
        if (finalDamage > maxDamage) maxDamage = finalDamage;

        // 단일 공격일 때만 로그 업데이트 (1000회 시에는 마지막 공격 정보 표시)
        if (!logDisplay.text.Contains("1,000회"))
        {
            string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
            string weakMark = isWeakPoint ? "<color=blue>[약점!]</color> " : "";
            if (isMiss) logDisplay.text = "<color=gray>명중 실패</color>";
            else logDisplay.text = string.Format("{0}{1}데미지: {2:F1}", critMark, weakMark, finalDamage);
        }
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        // 2σ 기준 범위 표시
        rangeDispay.text = string.Format("미스 범위: {0:F1}↓ / 약점 범위: {1:F1}↑",
            baseDamage - (2 * baseDamage * stdDevMult),
            baseDamage + (2 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format(
            "누적 데미지: {0:F1} (최대: {1:F1})\n" +
            "공격 횟수: {2} / 평균 DPA: {3:F2}\n" +
            "<color=blue>약점: {4}</color> / <color=black>미스: {5}</color> / <color=red>치명타: {6}</color>",
            totalDamage, maxDamage, attackCount, dpa, weakPointCount, missCount, totalCritCount);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}