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


    public static Num Add(List<Element> chain)
    {
        int sum = 0;
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
                    //int ind = currentInd - Array.IndexOf(Dec, element.num.dec);
                    //sum += (int)(element.numVal / MathF.Pow(1000, ind));
                }
            }
            else
            {
                sum = sum % 1000;
                currentInd++;
            }
        }
        while (s < sum)
        {
            s *= 2;
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
        int min = 1;

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
            dec = 'O',
            txt = $"{x}"
        };
    }
}
