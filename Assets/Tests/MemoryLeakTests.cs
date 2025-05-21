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
        GameObject testObject = new GameObject("TestObject");
        WeakReference weakRef = new WeakReference(testObject);

        GameObject.DestroyImmediate(testObject);
        testObject = null;

        yield return null;

        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();

        Assert.IsFalse(weakRef.IsAlive, "GameObject не был уничтожен, возможна утечка памяти.");
    }
}
