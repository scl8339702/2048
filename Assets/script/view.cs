using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;

public class view : MonoBehaviour
{
    public static view instance;
    //===group====
    public GameObject[] gridGroup;
    public Color[] blockBkColor;
    public int[] numberGroup = new int[] { 2,4,8,16,32,64,128,256,512,1024,2048 };
    //============
    public Text best;
    public Text source;
    [HideInInspector]
    public AssetBundle block;
    public int[,] tzfeGridBefor = new int[4, 4];
    void Awake()
    {
        if (instance == null)
        {
            instance = new view();
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        instance.gridGroup = gridGroup;
        instance.blockBkColor = blockBkColor;
        instance.block = loadAssetsBundle("block");
        instance.best = best;
        instance.source = source;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
       
    }
    //设置单个数字块的颜色和数字
    public void setItemColor(GameObject numberBlock,int number)
    {
        int index = Array.IndexOf(numberGroup,number);
        if (index < blockBkColor.Length)
        {
            numberBlock.GetComponent<Image>().color = blockBkColor[index];
        }
        else
        {
            numberBlock.GetComponent<Image>().color = blockBkColor[7];
        }
        Text numberText = numberBlock.transform.GetChild(0).GetComponent<Text>();
        numberText.text = number+"";
        if (number > 4)
        {
            numberText.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            numberText.color = new Color32(120, 111, 102, 255);
        }
    }

    //前后比较，生成对应的数字
    public bool compareTZFZGrid(moveDirection direction)
    {
        bool isChange = false;
        for(int i=0;i<4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                if (tzfeGridBefor[i, j] < countModel.instance.tzfeGrid[i, j])
                {
                    int count = gridGroup[i * 4 + j].transform.childCount;
                    if (gridGroup[i * 4 + j].GetComponent<saveSighleBlock>().singleBlock!=null)
                    {   //----如果前后对比增加了，且原来该位置有数字，直接重新设置数字的值
                        GameObject bl = gridGroup[i * 4 + j].GetComponent<saveSighleBlock>().singleBlock;
                        bl.SetActive(true);
                        setItemColor(bl, countModel.instance.tzfeGrid[i, j]);
                    }
                    else
                    {
                        //----如果前后对比增加了，且原来该位置没有数字，重新在此生成一个数字
                        GameObject bl =loadPreb(block, "singleBlock",gridGroup[i*4+j]);
                        gridGroup[i * 4 + j].GetComponent<saveSighleBlock>().singleBlock = bl;
                        setItemColor(bl, countModel.instance.tzfeGrid[i, j]);
                    }
                    tzfeGridBefor[i, j] = countModel.instance.tzfeGrid[i, j];
                    if (isChange == false)
                    {
                        isChange = true;
                    }
                }
                else if(tzfeGridBefor[i, j] > countModel.instance.tzfeGrid[i, j])
                {
                    GameObject bl = gridGroup[i * 4 + j].GetComponent<saveSighleBlock>().singleBlock;
                    if (countModel.instance.tzfeGrid[i, j] == 0) 
                    {
                        //----如果前后对比减少了，且现在为0，直接隐藏数字
                        bl.SetActive(false);
                    }
                    else
                    {
                        //----如果前后对比减少了，且现在不为0，直接修改数字
                        setItemColor(bl, countModel.instance.tzfeGrid[i, j]);
                    }
                    blockMove(gridGroup[i * 4 + j], direction, i, j, tzfeGridBefor[i,j]);
                    tzfeGridBefor[i, j] = countModel.instance.tzfeGrid[i, j];
                    if (isChange == false)
                    {
                        isChange = true;
                    }
                }
            }
        }
        return isChange;
    }

    /// <summary>
    /// 假装移动动画
    /// </summary>
    /// <param name="parent">初始位置</param>
    /// <param name="direction">移动方向（1。左，2.右，3.上，4.下）</param>
    /// /// <param name="veNumber">行数</param>
    /// /// <param name="hoNumber">列数</param>
    /// <param name="number">移动的数字</param>
    public void blockMove(GameObject parent,moveDirection direction, int veNumber,int hoNumber,int number)
    {
      GameObject bl =  loadPreb(block, "moveAni", parent);
        setItemColor(bl,number);
        switch (direction)
        {
            case moveDirection.left:
                bl.transform.SetParent(gridGroup[veNumber * 4].transform);
                bl.transform.DOLocalMove(Vector3.zero, 0.07f).SetEase(Ease.Linear).OnComplete(()=>Destroy(bl));
                break;
            case moveDirection.right:
                bl.transform.SetParent(gridGroup[veNumber * 4+3].transform);
                bl.transform.DOLocalMove(Vector3.zero, 0.07f).SetEase(Ease.Linear).OnComplete(() => Destroy(bl));
                break;
            case moveDirection.up:
                bl.transform.SetParent(gridGroup[hoNumber].transform);
                bl.transform.DOLocalMove(Vector3.zero, 0.07f).SetEase(Ease.Linear).OnComplete(() => Destroy(bl));
                break;
            case moveDirection.down:
                bl.transform.SetParent(gridGroup[12+hoNumber].transform);
                bl.transform.DOLocalMove(Vector3.zero, 0.07f).SetEase(Ease.Linear).OnComplete(() => Destroy(bl));
                break;
        }
        bl.transform.SetAsFirstSibling();
    }


    //隐藏所有数字
    public void hideAllBlock()
    {
        foreach (GameObject a in gridGroup)
        {
            if (a.transform.childCount != 0)
            {
                for (int i = 0; i < a.transform.childCount; i++)
                {
                    a.GetComponent<saveSighleBlock>().singleBlock.SetActive(false);
                }
            }
        }
    }
    
    //加载assetsBundle资源
    public AssetBundle loadAssetsBundle(string assetsBundleName)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/" + assetsBundleName);
        return ab;
    }
    public GameObject loadPreb(AssetBundle ab, string prefabName, GameObject parent)
    {
        GameObject prefab = ab.LoadAsset<GameObject>(prefabName);
        GameObject initiaPre = Instantiate(prefab);
        initiaPre.transform.SetParent(parent.transform);
        initiaPre.transform.localScale = Vector3.one;
        initiaPre.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        initiaPre.transform.GetComponent<RectTransform>().offsetMax = new Vector2(1, 1);
        return initiaPre;
    }
}
