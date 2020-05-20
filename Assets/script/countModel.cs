using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class countModel : MonoBehaviour
{
    public static countModel instance;
    [HideInInspector]
    public int[,] tzfeGrid = new int[4, 4];
    public int source = 0;
    public int bestSource;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = new countModel();
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("bestSource"))
        {
            instance.bestSource = PlayerPrefs.GetInt("bestSource");
            view.instance.best.text = instance.bestSource + "";
        }
    }

    //moveToLeft
    public void moveLeft()
    {
        for(int ve = 0; ve < tzfeGrid.GetLongLength(0); ve++)
        {
            int len = int.Parse(tzfeGrid.GetLongLength(1).ToString());
            int[] arry = new int[len];
            for(int hor = 0; hor < len; hor++)
            {
                arry[hor] = tzfeGrid[ve, hor];
            }
            removeZero(arry);
            addSame(arry);
            removeZero(arry);
            for (int hor = 0; hor < len; hor++)
            {
                tzfeGrid[ve, hor] = arry[hor];
            }
        }
    }
    //moveToRight
    public void moveRight()
    {
        for (int ve = 0; ve < tzfeGrid.GetLongLength(0); ve++)
        {
            int len = int.Parse(tzfeGrid.GetLongLength(1).ToString());
            int[] arry = new int[len];
            for (int hor = 0; hor < len; hor++)
            {
                arry[hor] = tzfeGrid[ve, hor];
            }
            turnAroundArry(arry);
            removeZero(arry);
            addSame(arry);
            removeZero(arry);
            turnAroundArry(arry);
            for (int hor = 0; hor < len; hor++)
            {
                tzfeGrid[ve, hor] = arry[hor];
            }
        }
    }
    //moveToUp
    public void moveUp()
    {
        for (int hor = 0; hor < tzfeGrid.GetLongLength(1); hor++)
        {
            int len = int.Parse(tzfeGrid.GetLongLength(0).ToString());
            int[] arry = new int[len];
            for (int ve = 0; ve < len; ve++)
            {
                arry[ve] = tzfeGrid[ve, hor];
            }
            removeZero(arry);
            addSame(arry);
            removeZero(arry);
            for (int ve = 0; ve < len; ve++)
            {
                tzfeGrid[ve, hor] = arry[ve];
            }
        }
    }
    //moveToDown
    public void moveDown()
    {
        for (int hor = 0; hor < tzfeGrid.GetLongLength(1); hor++)
        {
            int len = int.Parse(tzfeGrid.GetLongLength(0).ToString());
            int[] arry = new int[len];
            for (int ve = 0; ve < len; ve++)
            {
                arry[ve] = tzfeGrid[ve, hor];
            }
            turnAroundArry(arry);
            removeZero(arry);
            addSame(arry);
            removeZero(arry);
            turnAroundArry(arry);
            for (int ve = 0; ve < len; ve++)
            {
                tzfeGrid[ve, hor] = arry[ve];
            }
        }
    }

    //将数组中的0后移
    public void removeZero(int[] arry)
    {
        int len = arry.Length;
        int lastPos = len;
        for (int i = 0; i < len; i++)
        {
            if (arry[i] == 0 && lastPos == len)
            {
                lastPos = i;
            }
            else if (lastPos != len&&arry[i]!=0)
            {
                arry[lastPos] = arry[i];
                arry[i] = 0;
                i = lastPos;
                lastPos = len;
            }
        }
    }
    //将相邻的相同项相加
     public void addSame(int[] arry)
    {
        for (int i=0;i<arry.Length-1;i++)
        {
            if (arry[i] == arry[i + 1]) 
            {
                arry[i] =arry[i+1]+arry[i];
                setSource(arry[i]);
                arry[i + 1] = 0;
            }
        }
    }
    //设置最高得分和当前得分
    public void setSource(int number)
    {
        source = source + number;
        view.instance.source.text = source + "";
        if (source > bestSource)
        {
            PlayerPrefs.SetInt("bestSource", source);
            view.instance.best.text = source + "";
        }
    }

    //反转数组
    public void turnAroundArry(int[] arry)
    {
        int len = arry.Length;
        for (int i=0;i<len/2;i++)
        {
            int temp = arry[i];
            arry[i] = arry[len-i-1];
            arry[len-i-1] = temp;
        }
    }
    
    //在棋盘空白位置随机生成一个数字
    public void randomIni()
    {
        Dictionary<int, int[]> zeroDic = getZero();
        if (zeroDic.Count != 0)
        {
            int number = Random.Range(0, zeroDic.Count);
            int[] position = zeroDic[number];
            tzfeGrid[position[0], position[1]] = Random.Range(0, 10) < 9 ? 2 : 4;
            view.instance.tzfeGridBefor[position[0], position[1]] = tzfeGrid[position[0], position[1]];
            //--生成方块
            GameObject parent = view.instance.gridGroup[position[0] * 4 + position[1]];
            if (parent.GetComponent<saveSighleBlock>().singleBlock == null)
            {
                GameObject block = view.instance.loadPreb(view.instance.block, "singleBlock", parent);
                parent.GetComponent<saveSighleBlock>().singleBlock = block;
                view.instance.setItemColor(block, tzfeGrid[position[0], position[1]]);
            }
            else
            {
               GameObject block= parent.GetComponent<saveSighleBlock>().singleBlock;
               block.SetActive(true);
               view.instance.setItemColor(block, tzfeGrid[position[0], position[1]]);
            }
        }
    }
    //获取棋盘中为0的位置坐标
     public Dictionary<int,int[]> getZero()
    {
        Dictionary<int, int[]> tempStoreSpace = new Dictionary<int, int[]>();
        int keyNumber = 0;
        for (int i = 0; i < tzfeGrid.GetLongLength(0); i++)
        {
            for (int j = 0; j < tzfeGrid.GetLongLength(1); j++)
            {
                if (tzfeGrid[i, j] == 0)
                {
                    int[] value = new int[2] { 0, 0 };
                    value[0] = i;
                    value[1] = j;
                    tempStoreSpace.Add(keyNumber, value);
                    keyNumber = keyNumber + 1;
                }
            }
        }
        return tempStoreSpace;
    }

    //public void RightLeftCount()
    //{
    //    int lenght = tzfeGrid.GetLength(1);
    //    for (int ve = 0; ve < tzfeGrid.GetLength(0);ve++) 
    //    {
    //        int lastPo = lenght;
    //        for (int hor = 0; hor < lenght; hor++)
    //        {
    //            if (tzfeGrid[ve, hor] == 0 )
    //            {
    //                if (lastPo == lenght)
    //                {
    //                    lastPo = hor;
    //                }
    //                continue;
    //            }
    //            for (int j = hor + 1; j < tzfeGrid.GetLength(1); j++)
    //            {
    //                if (tzfeGrid[ve,j]==0)
    //                {
    //                    continue;
    //                }
    //                else if (tzfeGrid[ve, hor] == tzfeGrid[ve, j])
    //                {
    //                    tzfeGrid[ve, hor] = tzfeGrid[ve, hor] + tzfeGrid[ve, j];
    //                    tzfeGrid[ve, j] = 0;

    //                    break;
    //                }
    //                else
    //                {
    //                    break;
    //                }
    //            }
    //            if (lastPo != lenght)
    //            {
    //                tzfeGrid[ve, lastPo] = tzfeGrid[ve, hor];
    //                tzfeGrid[ve, hor] = 0;

    //                hor = lastPo ;
    //                lastPo = lenght;
    //            }
    //        }
    //    }
    //}
}
