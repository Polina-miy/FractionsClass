using System;

class Fractions
{
    public long IntegerPart { get; set; }
    public ushort FractionPart { get; set; } // 0..9999

    private const int SCALE = 10000;

    public Fractions(long integerPart, int fractionPart)
    {
        IntegerPart = integerPart;
        // Нормализация дробной части при создании (перенос избытка)
        int totalFraction = fractionPart;
        if (totalFraction >= SCALE)
        {
            IntegerPart += totalFraction / SCALE;
            totalFraction %= SCALE;
        }
        else if (totalFraction < 0)
        {
            // Отрицательная дробная часть – заём из целой
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

        // Унификация знака: если целая часть отрицательна, а дробная положительна – корректируем
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

    // Сложение (исправлено)
    public static Fractions operator +(Fractions a, Fractions b)
    {
        long res = a.ToScaled() + b.ToScaled();
        return FromScaled(res);
    }

    // Вычитание (исправлено)
    public static Fractions operator -(Fractions a, Fractions b)
    {
        long res = a.ToScaled() - b.ToScaled();
        return FromScaled(res);
    }

    // Умножение (НАМЕРЕННО ОСТАВЛЕНО НЕВЕРНЫМ – отсутствует деление на SCALE)
    public static Fractions operator *(Fractions a, Fractions b)
    {
        long res = a.ToScaled() * b.ToScaled(); // ОШИБКА: нужно делить на SCALE
        return FromScaled(res);
    }

    private static Fractions FromScaled(long value)
    {
        long intPart = value / SCALE;
        int fracPart = (int)Math.Abs(value % SCALE);
        return new Fractions(intPart, fracPart);
    }

    // Операторы сравнения (реализованы только >, <, ==, !=; >= и <= ОТСУТСТВУЮТ)
    public static bool operator >(Fractions a, Fractions b) => a.ToScaled() > b.ToScaled();
    public static bool operator <(Fractions a, Fractions b) => a.ToScaled() < b.ToScaled();
    public static bool operator ==(Fractions a, Fractions b) => a.ToScaled() == b.ToScaled();
    public static bool operator !=(Fractions a, Fractions b) => !(a == b);

    // Отсутствуют операторы >= и <= (это второй баг)

    public override bool Equals(object obj)
    {
        if (obj is Fractions f) return this == f;
        return false;
    }

    public override int GetHashCode() => ToScaled().GetHashCode();

    // ToString() исправлен – всегда 4 цифры дробной части
    public override string ToString() => $"{IntegerPart},{FractionPart:D4}";
}