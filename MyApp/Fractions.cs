using System;

class Fractions
{
    public long IntegerPart { get; set; }
    public ushort FractionPart { get; set; } // 0..9999

    private const int SCALE = 10000;

    public Fractions(long integerPart, int fractionPart)
    {
        IntegerPart = integerPart;
        
        int totalFraction = fractionPart;
        if (totalFraction >= SCALE)
        {
            IntegerPart += totalFraction / SCALE;
            totalFraction %= SCALE;
        }
        else if (totalFraction < 0)
        {
            
            long needed = (-totalFraction + SCALE - 1) / SCALE;
            IntegerPart -= needed;
            totalFraction += (int)(needed * SCALE);
        }
        FractionPart = (ushort)Math.Abs(totalFraction);
        Normalize();
    }

    private void Normalize()
    {
        if (FractionPart >= SCALE)
        {
            IntegerPart += FractionPart / SCALE;
            FractionPart = (ushort)(FractionPart % SCALE);
        }

        if (FractionPart < 0)
        {
            IntegerPart -= 1;
            FractionPart = (ushort)(SCALE + FractionPart);
        }

        if (IntegerPart < 0 && FractionPart > 0)
        {
            IntegerPart += 1;
            FractionPart = (ushort)(SCALE - FractionPart);
        }
    }

    private long ToScaled()
    {
        return IntegerPart * SCALE + FractionPart;
    }

    // Сложение 
    public static Fractions operator +(Fractions a, Fractions b)
    {
        long res = a.ToScaled() + b.ToScaled();
        return FromScaled(res);
    }

    // Вычитание 
    public static Fractions operator -(Fractions a, Fractions b)
    {
        long res = a.ToScaled() - b.ToScaled();
        return FromScaled(res);
    }

    // Умножение
    public static Fractions operator *(Fractions a, Fractions b)
    {
        long res = a.ToScaled() * b.ToScaled(); 
        return FromScaled(res);
    }

    private static Fractions FromScaled(long value)
    {
        long intPart = value / SCALE;
        int fracPart = (int)Math.Abs(value % SCALE);
        return new Fractions(intPart, fracPart);
    }

    // Операторы сравнения
    public static bool operator >(Fractions a, Fractions b) => a.ToScaled() > b.ToScaled();
    public static bool operator <(Fractions a, Fractions b) => a.ToScaled() < b.ToScaled();
    public static bool operator ==(Fractions a, Fractions b) => a.ToScaled() == b.ToScaled();
    public static bool operator !=(Fractions a, Fractions b) => !(a == b);


    public override bool Equals(object obj)
    {
        if (obj is Fractions f) return this == f;
        return false;
    }

    public override int GetHashCode() => ToScaled().GetHashCode();

    public override string ToString() => $"{IntegerPart},{FractionPart:D4}";
}
