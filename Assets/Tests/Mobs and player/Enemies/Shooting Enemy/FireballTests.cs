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

    [UnityTest]
    public IEnumerator Test3_FireballSelfDestruct()
    {
        // Устанавливаем время жизни и инициализируем направление
        fireball.lifeTime = 0.1f;
        fireball.Initialize(Vector2.right, Vector3.zero);
        
        // Вручную запускаем Start() для активации таймера
        fireball.Start();
        
        // Ждем 2 кадра для инициализации
        yield return null;
        yield return null;
        
        // Ждем полное время жизни + небольшой запас
        yield return new WaitForSeconds(fireball.lifeTime + 0.05f);
        
        // Проверяем уничтожение именно GameObject
        Assert.IsTrue(fireballObj == null); 
    }
}
