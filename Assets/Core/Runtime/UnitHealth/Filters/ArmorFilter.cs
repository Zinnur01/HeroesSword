public class ArmorFilter : HealthFilter
{
    // Stored required properties.
    private int useCount = 0;

    public override void FilterDamage(ref int damage)
    {
        useCount++;

        switch (useCount)
        {
            case 1:
                damage = (int)(damage * 0.01f);
                break;
            case 2:
                damage = (int)(damage * 0.33f);
                break;
            case 3:
                damage = (int)(damage * 0.66f);
                break;
            default:
                break;
        }
    }
}
