using NUnit.Framework;

public class EnemyLogicTests
{
    private float currentHealth;
    private float maxHealth = 100f;

    [SetUp]
    public void Setup()
    {
        currentHealth = maxHealth;
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = currentHealth < 0 ? 0 : currentHealth; // clamp to 0
    }

    [Test]
    public void TakeDamage_ReducesHealthCorrectly()
    {
        TakeDamage(30f);
        Assert.AreEqual(70f, currentHealth, 0.001f);
    }

    [Test]
    public void TakeDamage_DoesNotGoBelowZero()
    {
        TakeDamage(150f);
        Assert.AreEqual(0f, currentHealth, 0.001f);
    }

    [Test]
    public void TakeDamage_ExactZero()
    {
        TakeDamage(100f);
        Assert.AreEqual(0f, currentHealth, 0.001f);
    }

    [Test]
    public void TakeDamage_NoDamage()
    {
        TakeDamage(0f);
        Assert.AreEqual(100f, currentHealth, 0.001f);
    }
}
