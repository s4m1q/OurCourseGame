using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class MemoryLeakTests
{
    [UnityTest]
    public IEnumerator GameObject_Destroy_ShouldNotLeaveMemoryLeak()
    {
        // Создаем объект и запоминаем его
        GameObject testObject = new GameObject("TestObject");
        WeakReference weakRef = new WeakReference(testObject);

        // Уничтожаем объект
        GameObject.DestroyImmediate(testObject);
        testObject = null;

        // Ждем завершения кадра
        yield return null;

        // Принудительно запускаем сборщик мусора
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();

        // Проверяем, что объект уничтожен
        Assert.IsFalse(weakRef.IsAlive, "GameObject не был уничтожен, возможна утечка памяти.");
    }
}