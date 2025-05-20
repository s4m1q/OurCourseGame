using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class FireballTests
{
    private GameObject fireballObj;
    private Fireball fireball;
    
    [SetUp]
    public void Setup()
    {
        // Создаем мок GameManager
        var gameManagerObj = new GameObject("GameManager");
        gameManagerObj.AddComponent<GameManager>();

        // Создаем Fireball
        fireballObj = new GameObject();
        fireball = fireballObj.AddComponent<Fireball>();
    }

    [TearDown]
    public void Cleanup()
    {
        Object.DestroyImmediate(fireballObj);
    }

    [Test]
    public void Test1_FireballExistsAfterCreation()
    {
        Assert.IsNotNull(fireball); // Просто проверяем что объект создан
    }

}
