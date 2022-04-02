using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KardanAdamManager : MonoBehaviour
{
    public static KardanAdamManager instance;

    public List<GameObject> kardanAdamlar = new List<GameObject>();

    public Vector3 startPos;
    public int columns;
    public float space;

    public Transform karakter; //sets on inspector

    public Transform testEngel;





    public float minDistance = 0.25f;

    public GameObject bodyprefabs;

    private float dis;
    private Transform curBodyPart;
    private Transform PrevBodyPart;

    private float targetSpread;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        targetSpread = GameManager.instance.kardanAdamTargetSpread;
        //UpdateKardanAdams();
    }

    private void Update()
    {

    }
    public void UpdateKardanAdams()
    {
        int index = 0;
        for (int i = 0; i < kardanAdamlar.Count; i++)
        {
            GameObject adam = kardanAdamlar[i];
            adam.transform.SetParent(karakter.transform);
            adam.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            adam.transform.localRotation = karakter.rotation;
            adam.transform.localPosition = startPos + CalcPosition(i);
            if (index < 4)
                adam.GetComponent<Adam>().toFollow = karakter;

            if (index >= 4)
                adam.GetComponent<Adam>().toFollow = kardanAdamlar[i - 4].transform;

            index++;
        }
    }

    public void ChangeKardanAdamAnimationToJump(bool jump)
    {
        foreach (GameObject adam in kardanAdamlar)
        {
            Animator anim = adam.transform.GetChild(0).GetComponent<Animator>();
            anim.SetBool("jump", jump);
            adam.GetComponent<Adam>().ResetAnim();
        }
    }

    public void KardanAdamFirlat(Transform engel)
    {
        int howMany = kardanAdamlar.Count - GameManager.instance.countOfFirlatilacakKardanAdam;
        Debug.Log("firlatilacak adam: " + howMany);
        if (AreThereEnoughKardanAdams(howMany))
        {
            //engel.GetComponent<Obstract>().kirilmayaHazir = true;
            for (int i = kardanAdamlar.Count; i-- > howMany;)
            {
                kardanAdamlar[i].GetComponent<Adam>().Firlat(engel.position + RandomCoordinates(), GameManager.instance.kardanAdamFirlatmaGucu);
                kardanAdamlar.RemoveAt(i);
            }
        }
        else
            Debug.Log("Not Enough Adams");
    }
    private Vector3 RandomCoordinates()
    {
        float x = Random.Range(-targetSpread, targetSpread);
        float y = Random.Range(-targetSpread, targetSpread);

        return new Vector3(x, y, 0);
    }
    Vector3 CalcPosition(int index) // call this func for all your objects
    {
        //float posX;
        //if (index % columns == 0)
        //    posX = -0.5f;
        //else if (index % columns == 1)
        //    posX = 0.5f;

        //else
        //    posX = (index % columns) * space;
        //float posZ = (index / columns) * space;
        //return new Vector3(posX, 0, posZ);

        //float posX;
        //if (index % columns == 0)
        //{
        //    posX = -0.5f + (-0.15f * index / columns);

        //}
        //else
        //{
        //    posX = 0.5f + (0.15f * index / columns);

        //}
        //float posZ = (index / columns) * space;
        //return new Vector3(posX, 0, posZ);

        //float posX;
        //if (index % columns == 0)
        //{
        //    posX = -0.5f + (-0.15f * index / columns);
        //    if(index > 6)
        //        posX = -0.5f + (-0.15f * 3);
        //}
        //else
        //{
        //    posX = 0.5f + (0.15f * index / columns);
        //    if (index > 6)
        //        posX = 0.5f + (0.15f * 3);
        //}
        //float posZ = (index / columns) * space;
        //return new Vector3(posX, 0, posZ);

        float posX;
        if (index % columns == 0)
            posX = -0.5f;
        else if (index % columns == 1)
            posX = 0.5f;
        else if (index % columns == 2)
            posX = -1f;
        else if (index % columns == 3)
            posX = 1f;
        else
            posX = (index % columns) * space;
        float posZ = (index / columns) * space;
        return new Vector3(posX, 0, posZ);
    }

    private bool AreThereEnoughKardanAdams(int many)
    {
        if (many >= 0)
            return true;
        else
            return false;
    }


    public void PlayEmoji(Emoji emoji)
    {
        foreach (GameObject adam in kardanAdamlar)
        {
            adam.GetComponent<Adam>().ChangeEmoji(emoji);
        }
    }

    public void OnPlayerDied()
    {

        for (int i = 0; i < kardanAdamlar.Count; i++)
        {
            kardanAdamlar[i].GetComponent<Adam>().HangAround();
        }
        kardanAdamlar.Clear();
    }

    public void FinalKardanAdamFirlat(int index, Vector3 target)
    {
        kardanAdamlar[index].GetComponent<Adam>().FinalMove(target);
    }
}
