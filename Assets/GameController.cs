using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Material[] materials;
    public Text scoreText;
    public bool CanCheck { get; set; }

    private GameObject firstCard, secondCard;
    private int score = 0, guesScore = 10, failScore = -2;
    private CardBechaviour fcb, scb;
    //private float checkTimeout = 0f, timeoutTime = 4f;

    // Use this for initialization
    void Start()
    {
        CanCheck = true;
        //checkTimeout = timeoutTime;
    }

    private void Awake()
    {
        Generation();
    }

    // Update is called once per frame
    void Update()
    {
        if (secondCard != null)// && checkTimeout >= timeoutTime)
        {
            CanCheck = false;
            Compare();
        }
        if (scb == null && Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000);
            if (hit.collider != null)
            {
                CardBechaviour cb = hit.transform.GetComponentInParent<CardBechaviour>();
                if (cb != null)
                {
                    cb.Select();
                }
            }
        }
        //if (checkTimeout < timeoutTime)
        //    checkTimeout += Time.deltaTime;
    }

    private void Generation()
    {
        List<Pair<Transform, Transform>> pairs = new List<Pair<Transform, Transform>>();
        int[] indexes = new int[materials.Length * 2];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = 1;
        }
        int p = 0;
        foreach (Material m in materials)
        {

            int f = Random.Range(0, indexes.Length);
            int s = Random.Range(0, indexes.Length);
            while (indexes[f] != 1)
            {
                f++;
                if (f == indexes.Length) f = 0;
            }
            indexes[f] = 0;
            while (indexes[s] != 1)
            {
                s++;
                if (s == indexes.Length) s = 0;
            }
            indexes[s] = 0;
            pairs.Add(new Pair<Transform, Transform>(transform.GetChild(f), transform.GetChild(s)));
            pairs[p].First.GetChild(1).GetComponent<MeshRenderer>().material = m;
            pairs[p].First.GetComponent<CardBechaviour>().x = p;
            pairs[p].Second.GetChild(1).GetComponent<MeshRenderer>().material = m;
            pairs[p].Second.GetComponent<CardBechaviour>().x = p;
            p++;
        }
    }

    public bool AddCard(GameObject card)
    {
        bool res = false;
        if (firstCard == null)
        {
            firstCard = card;
            fcb = firstCard.GetComponentInParent<CardBechaviour>();
            res = true;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            scb = secondCard.GetComponentInParent<CardBechaviour>();
            //checkTimeout = 2f;
            res = true;
        }
        return res;
    }

    private void Compare()
    {
        Debug.Log("c" + fcb.isReady + " " + scb.isReady);
        if (fcb.isReady == true && scb.isReady == true)
        {
            Debug.Log("Both are ready");
            if (fcb.x == scb.x)
            {
                Debug.Log("Correct");
                score += guesScore;
                Destroy(firstCard);
                Destroy(secondCard);
            }
            else
            {
                Debug.Log("Wrong");
                score += failScore;
                StartCoroutine(DoCompare());
            }
            firstCard = null;
            secondCard = null;
            CanCheck = true;
            scoreText.text = "Score: " + score;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator DoCompare()
    {
        yield return new WaitForSeconds(1f);
        fcb.Unselect();
        scb.Unselect();
        scb = null;
    }

}
