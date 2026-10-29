using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using EFT.Quests;
using Terkoiz.Skipper;
using UnityEngine.Events;

[BepInPlugin("com.jpeckham.skipper.tests", "Skipper Runtime Tests", "1.0.0")]
[BepInDependency("com.terkoiz.skipper")]
public class RuntimeTests : BasePlugin
{
    public override void Load()
    {
        try
        {
            if (QuestObjectiveViewPatch.TargetMethod() == null) throw new Exception("Missing target");
            var condition = new ConditionCounterCreator();
            condition.value = 7f;
            var checker = new ConditionProgressChecker(condition);
            SetGetter(checker, condition);
            var clicked = new[] { false };
            var evt = new UnityEvent();
            AddListener(evt, clicked);
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            if (checker.CurrentValue != 7d) throw new Exception("Native progress getter failed");
            evt.Invoke();
            if (!clicked[0]) throw new Exception("Native button callback failed");
            QuestObjectiveViewPatch.Postfix(null, null, null, null);
            Log.LogInfo("PASS: target resolution, native progress getter and UnityEvent callback after GC, null objective guard; no profile modified by harness");
        }
        catch (Exception ex) { Log.LogError("FAIL: " + ex); }
    }
    private static void SetGetter(ConditionProgressChecker checker, Condition condition)
    {
        Func<ConditionProgressChecker, double> getter = _ => condition.value;
        checker.SetCurrentValueGetter((Il2CppSystem.Func<ConditionProgressChecker, double>)getter);
    }
    private static void AddListener(UnityEvent evt, bool[] clicked)
    {
        Action callback = () => clicked[0] = true;
        evt.AddListener((UnityAction)callback);
    }
}
