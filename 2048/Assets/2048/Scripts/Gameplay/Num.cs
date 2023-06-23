using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Num
{
    public int numVal;
    public char dec;
    public string txt;
    public static char[] Dec = { ' ', 'K', 'M', 'B', 'Q' };

    public static Num Compare(Num num1, Num num2)
    {
        if(Array.IndexOf(Dec, num1.dec) == Array.IndexOf(Dec, num2.dec))
        {
            if(num1.numVal >= num2.numVal)
            {
                return num1;
            }
            else
            {
                Debug.Log("normal num2");
                return num2;
            }
        }
        else
        {
            Debug.Log("run");

            if (Array.IndexOf(Dec, num1.dec) > Array.IndexOf(Dec, num2.dec))
            {
                return num1;
            }
            else
            {
                return num2;
            }
        }
    }
    public static Num Add(List<Element> chain)
    {
        float sum = 0;
        int s = 1;
        char initialDec = chain[0].num.dec;
        int initialInd = Array.IndexOf(Dec, initialDec);
        int currentInd = initialInd;


        foreach (Element element in chain)
        {
            if (sum < 1000)
            {
                if(Array.IndexOf(Dec, element.num.dec) == currentInd)
                {
                    sum += element.numVal;
                }
                else if(Array.IndexOf(Dec, element.num.dec) != currentInd)
                {
                    int ind = currentInd - Array.IndexOf(Dec, element.num.dec);
                    sum += (element.numVal / MathF.Pow(1000, ind));
                }
            }
            else
            {
                sum = 1;
                currentInd++;
            }
        }
        do
        {
            s *= 2;
        } while (s < sum);

        if( s == 1024)
        {
            s = 1; // for the bug when sum = 1024 then it isn't setting itself to 1k
            currentInd++;
        }

        Num num = new Num();
        num.numVal = s;
        num.dec = Dec[currentInd];
        num.txt = $"{s}{Dec[currentInd]}";
        return num;
    }
    public static void NumSetup(ref Num num, int numeric = default)
    {
        System.Random random = new System.Random();

        int x;
        int max = 10;
        int min = 9;

        if (numeric == 0)
        {
            x = (int)Mathf.Pow(2, random.Next(min, max));
        }
        else
        {
            x = numeric;
        }

        num = new Num()
        {
            numVal = x,
            dec = ' ',
            txt = $"{x}"
        };
    }
}
