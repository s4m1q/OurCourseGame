using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using UnityEngine.SceneManagement;
public class DeathHandlerTests
{
    private GameObject handlerObject;
    private PlayerDeathHandler handler;
    private TestablePlayerDeathHandler testableHandler;

    [SetUp]
    public void Setup()
    {
        handlerObject = new GameObject();
        handler = handlerObject.AddComponent<PlayerDeathHandler>();
        testableHandler = handlerObject.AddComponent<TestablePlayerDeathHandler>();

        // Setup player
        var playerObject = new GameObject();
        handler.player = playerObject.AddComponent<PlayerController>();
        testableHandler.player = handler.player;

        // Setup UI
        handler.gameOverUI = new GameObject();
        testableHandler.gameOverUI = handler.gameOverUI;
        handler.gameOverUI.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(handlerObject);
        Object.DestroyImmediate(handler.player.gameObject);
        Object.DestroyImmediate(handler.gameOverUI);
    }

    [Test]
    public void HandleDeath_SetsIsDeadToTrue()
    {
        // Arrange
        var isDeadField = typeof(PlayerDeathHandler).GetField("isDead", BindingFlags.NonPublic | BindingFlags.Instance);
        isDeadField.SetValue(handler, false);

        // Act
        CallPrivateMethod(handler, "HandleDeath");

        // Assert
        Assert.IsTrue((bool)isDeadField.GetValue(handler));
    }

    [UnityTest]
    public IEnumerator Update_WhenHealthIsZero_CallsHandleDeath()
    {
        // Arrange
        handler.player.currentHealth = 0;
        var isDeadField = typeof(PlayerDeathHandler).GetField("isDead", BindingFlags.NonPublic | BindingFlags.Instance);
        isDeadField.SetValue(handler, false);

        // Act
        yield return null; // Wait for Update

        // Assert
        Assert.IsTrue((bool)isDeadField.GetValue(handler));
    }

    [Test]
    public void HandleDeath_InLevel1_CallsRespawn()
    {
        // Arrange
        testableHandler.SetSceneName("Level 1");
        testableHandler.player.currentHealth = 0;

        // Act
        CallPrivateMethod(testableHandler, "HandleDeath");

        // Assert
        Assert.IsTrue(testableHandler.RespawnCalled);
    }

    [Test]
    public void HandleDeath_NotInLevel1_TriggersGameOver()
    {
        // Arrange
        testableHandler.SetSceneName("Level 2");
        testableHandler.player.currentHealth = 0;

        // Act
        CallPrivateMethod(testableHandler, "HandleDeath");

        // Assert
        Assert.IsTrue(testableHandler.GameOverCalled);
        Assert.IsTrue(testableHandler.gameOverUI.activeSelf);
    }

    private void CallPrivateMethod(object obj, string methodName)
    {
        var method = obj.GetType().GetMethod(methodName,
            BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(obj, null);
    }

    // Testable subclass
    public class TestablePlayerDeathHandler : PlayerDeathHandler
    {
        public bool RespawnCalled { get; private set; }
        public bool GameOverCalled { get; private set; }
        private string fakeSceneName;

        public void SetSceneName(string name) => fakeSceneName = name;

        protected override string GetCurrentSceneName() => fakeSceneName;

        protected override void RespawnPlayer()
        {
            RespawnCalled = true;
            // base.RespawnPlayer(); // Skip actual implementation
        }

        protected override void GameOver()
        {
            GameOverCalled = true;
            gameOverUI.SetActive(true);
            // Skip Invoke and call directly
            ReturnToMainMenu();
        }
    }
}
