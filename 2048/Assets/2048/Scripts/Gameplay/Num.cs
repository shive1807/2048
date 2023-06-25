using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Num
{
    public static char[] Dec = { ' ', 'K', 'M', 'B', 'T', 'q', 'Q', 's', 'S', 'O', 'N', 'D' };

    public int numVal;
    public char dec;
    public string txt;

    public static int CurrentDec(Num num)
    {
        return (Array.IndexOf(Dec, num.dec));
    }

    public static Num Max(Num num1, Num num2)
    {
        if(CurrentDec(num1) == CurrentDec(num2))
        {
            if(num1.numVal >= num2.numVal)
            {
                return num1;
            }
            else
            {
                return num2;
            }
        }
        else
        {
            if (CurrentDec(num1) > CurrentDec(num2))
            {
                return num1;
            }
            else
            {
                return num2;
            }
        }
    }
    public static Num Min(Num num1, Num num2)
    {
        if (CurrentDec(num1) == CurrentDec(num2))
        {
            if (num1.numVal >= num2.numVal)
            {
                return num2;
            }
            else
            {
                return num1;
            }
        }
        else
        {
            if (CurrentDec(num1) > CurrentDec(num2))
            {
                return num2;
            }
            else
            {
                return num1;
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

        int x = 2;
        int y = 1;
        int i = DependencyManager.Instance.gridController.DecInd;

        int min  = DependencyManager.Instance.gridController.ElementMinLimit;
        int max  = DependencyManager.Instance.gridController.ElementMaxLimit;

        if (numeric == 0)
        {
            y = random.Next(min, max);

            // if y > 10 means the max has exceeded the 1024 therefore we are setting it to our num system

            if (y >= 10)
            {
                y -= 10;
                i++;
            }

            x = (int)Mathf.Pow(2, y);
        }
        else
        {
            x = numeric;
        }

        char dec = Dec[i];

        num = new Num()
        {
            numVal = x,
            dec = dec,
            txt = $"{x}{dec}"
        };
    }
}
