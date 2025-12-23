using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Hearts : MonoBehaviour
{
    [Header("UI & Scoring")]
    public TMP_Text scoreText;
    public int score = 0;
    private bool areTargetsPaired = false;
    public NPCIconCamera npcIconCamera;

    [Header("Heart 1")]
    public GameObject heart1;
    public TMP_Text heart1Name; public RawImage heart1Icon;
    [HideInInspector] public GameObject heart1target;
    public Animator heart1Anim;

    [Header("Heart 2")]
    public GameObject heart2; public RawImage heart2Icon;
    public TMP_Text heart2Name;
    [HideInInspector] public GameObject heart2target;
    public Animator heart2Anim;

    [Header("Attack Rounds")]
    [SerializeField]private float timer = 0f, roundLength = 180f;
    bool RoundOn;

    private List<GameObject> pastTargets; 

    private void Start()
    {
        UpdateScoreDisplay();
        ClearTargets();
    }

    public bool CanShoot(GameObject shotTarget)
    {
        if (shotTarget == null) return false;

        if (heart1target != null && heart2target != null)
            return false;

        if (pastTargets.Contains(shotTarget))
            return false;

        return true;
    }
    
    public void Shoot(GameObject shotTarget)
    {
        pastTargets.Add(shotTarget);
        if (heart1target == null)
        {
            heart1target = shotTarget;
            heart1Name.text = shotTarget.name;

            npcIconCamera.gameObject.SetActive(true);
            heart1Icon.texture = npcIconCamera.CaptureIcon(shotTarget);
            heart1Anim.SetTrigger("Shot");
            Debug.Log($"Target 1 set: {heart1target.name}");

        }
        else
        {
            heart2target = shotTarget;
            heart2Name.text = shotTarget.name;

            npcIconCamera.gameObject.SetActive(true);
            heart2Icon.texture = npcIconCamera.CaptureIcon(shotTarget);
            heart2Anim.SetTrigger("Shot");
            Debug.Log($"Target 2 set: {heart2target.name}");
            
            StartCoroutine(HandleMatchupSequence());
        }
        

        // once both shot, they go out into the void.
    }
    
    public void StartMatchupTimer()
    {
        ClearTargets();
    }

    private void ClearTargets()
    {
        heart1target = null;  heart2target = null;
        heart1Name.text = "???"; heart2Name.text = "???";
        areTargetsPaired = false;
        Debug.Log("Targets data cleared, ready for new pair.");
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"{score}";
    }

    private IEnumerator HandleMatchupSequence()
    {
        areTargetsPaired = true;
        
        yield return new WaitForSeconds(0.5f); 
        heart1Anim.SetTrigger("Score"); heart2Anim.SetTrigger("Score");
    }

    public void NewScore()
    {
        score++; UpdateScoreDisplay();
        ClearTargets();
    }

    public IEnumerator AttackRoundTimer()
    {
        RoundOn = true;
        timer = roundLength;

        while (timer > 0)
        {
            timer -= 3f;
            Debug.Log("Time left: " + Mathf.Ceil(timer));
            
            yield return new WaitForSeconds(3f);
        }

        Debug.Log("Time's up!");
        RoundOn = false;

        //GameManager.Scene End. depending on finalScore, the stuff will b slightly diff but mostly the same

        // reset score? or mabe do something before that idk
    }
}