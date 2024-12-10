using System;
using System.Collections.Generic;
using System.Linq;

public enum Buff
{
    BulletCapIncrease,
    HPCountIncrease,
    DamageIncrease,
    RecoverAllHP,
    AutoShot,
    AutoReload,
    TimeSlowTime,
    TimeSlowMult
}

public class BuffSelector
{
    private static Random random = new Random();

    public static List<Buff> GetRandomBuffs(int count)
    {
        
        Buff[] allBuffs = (Buff[])Enum.GetValues(typeof(Buff));

        
        return allBuffs.OrderBy(b => random.Next()).Take(count).ToList();
    }
}