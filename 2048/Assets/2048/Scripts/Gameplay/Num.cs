using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Num
{
    public static char[] Dec = { ' ', 'K', 'M', 'B', 'T', 'q', 'Q', 's', 'S', 'O', 'N', 'D' };

    public int numVal;
    public char dec;
    public string txt;
    public Num(int val = default, char dec = default)
    {
        this.numVal = val;
        this.dec = dec;
        this.txt = $"{val}{dec}";
    }
    public static int CurrentDec(Num num)
    {
        return (Array.IndexOf(Dec, num.dec));
    }
    public static Num Increment(Num Num, int i)
    {
        List<Num> list = new List<Num>();

        for(int j = 0; j <= i; j++)
        {
            list.Add(Num);
        }

        float sum = 0;
        int s = 1;
        char initialDec = list[0].dec;
        int initialInd = Array.IndexOf(Dec, initialDec);
        int currentInd = initialInd;


        foreach (Num _num in list)
        {
            if (sum < 1000)
            {
                if (Array.IndexOf(Dec, _num.dec) == currentInd)
                {
                    sum += _num.numVal;
                }
                else if (Array.IndexOf(Dec, _num.dec) != currentInd)
                {
                    int ind = currentInd - Array.IndexOf(Dec, _num.dec);
                    sum += (_num.numVal / MathF.Pow(1000, ind));
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

        if (s == 1024)
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
    public static Num AddElement(List<Element> chain)
    {
        float sum = 0;
        int s = 1;
        char initialDec = chain[0].num.dec;
        int initialInd = Array.IndexOf(Dec, initialDec);
        int currentInd = initialInd;


        foreach (Element element in chain)
        {
            Num _num = element.num;
            if (sum < 1000)
            {
                if(Array.IndexOf(Dec, _num.dec) == currentInd)
                {
                    sum += _num.numVal;
                }
                else if(Array.IndexOf(Dec, _num.dec) != currentInd)
                {
                    int ind = currentInd - Array.IndexOf(Dec, _num.dec);
                    sum += (_num.numVal / MathF.Pow(1000, ind));
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
        if( s == 2048)
        {
            s = 2;
            currentInd++;
        }
        Num num = new Num();
        num.numVal = s;
        num.dec = Dec[currentInd];
        num.txt = $"{s}{Dec[currentInd]}";
        return num;
    }
    public static Num AddNum(Num num1, Num num2)
    {
        float sum = 0;
        int currentDec = CurrentDec(num1);

        if(num1.dec == num2.dec)
        {
           sum = num1.numVal + num2.numVal;
        }
        else if(num1.dec != num2.dec)
        {
            sum = num1.numVal + num2.numVal / Mathf.Pow(1000, CurrentDec(num1));
        }
        return new Num() { numVal = (int)sum, dec = Dec[currentDec] };
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
    public static Color BlockColor(ref Num num)
    {
        int numVal = num.numVal;
        int dec = CurrentDec(num);

        return new Color(1 /** numVal / 10*/, 1 /** dec */, 1/* * (numVal + dec)*/);
    }
}
