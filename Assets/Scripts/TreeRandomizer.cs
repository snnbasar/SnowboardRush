using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;

    public GameObject treePrefab;

    public float offset;

    public List<GameObject> agaclar = new List<GameObject>();



    public void Generate()
    {
        int length = (int)((endPos.z - startPos.z) / offset);
        Debug.Log("agac to generate" + length);
        int index = 0;
        for (int i = 0; i < length; i++)
        {
            GameObject tree = Instantiate(treePrefab);
            tree.transform.parent = this.transform;
            agaclar.Add(tree);
            Vector3 calculatedPos = (Vector3.forward * (index * offset)) + startPos + pickRandomPoint();
            tree.transform.position = calculatedPos;
            tree.transform.rotation = Quaternion.Euler(pickRandomRotatiton());
            index++;
            Debug.Log(index);
        }
    }

    public void ClearList()
    {
        if(this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                DestroyImmediate(this.transform.GetChild(i).gameObject);
            }
        }
        agaclar.Clear();
    }
    public Vector3 pickRandomPoint()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = 0f;
        float z = Random.Range(-5, 5);
        return new Vector3(x, y, z);
    }
    public Vector3 pickRandomRotatiton()
    {
        float x = 0;
        float y = Random.Range(0, 360);
        float z = 0;
        return new Vector3(x, y, z);
    }
}
