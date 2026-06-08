using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class LinqExamples : MonoBehaviour
{
    public List<int> numbers = new List<int>() { 2,13,5,1,8,10,4};
    void Start()
    {
        var numberHigherThan5 = numbers.Where(x => x > 5).ToList();
        numbers.ForEach(ctx => Debug.Log(ctx));

      
    }
    [Button]
    public void TestFoD()
    {
        int has4 = numbers.FirstOrDefault(ctx => ctx == 100);
        print(has4);
    }

    [Button]
    public void TestAny()
    {
      bool isTrue= numbers.Any(ctx => ctx >= 10);
        print(isTrue);
    }

    [Button]
    public void TestOrderBy()
    {
        numbers = numbers.OrderBy(ctx => ctx).ToList();
    }
    [Button]
    public void TestOrderByDescending()
    {
        numbers = numbers.OrderByDescending(ctx => ctx).ToList();
    }
    
    public struct Enemy
    {
        public string EnemyName;
        public int EnemyCost;
        public Enemy(string enemyName , int enemyCost)
        {
            EnemyName = enemyName;
            EnemyCost = enemyCost;
        }
    }
    [Button]
    public void TestSelect()
    {
        List<Enemy> list = new List<Enemy>();
        list.Add(new Enemy("Manolo",4 ));
        list.Add(new Enemy("Arturo", 3));
        list.Add(new Enemy("Rigoberto",2));
        list.Add(new Enemy("Sonoro", 1));

        var result = list.Select(ctx => ctx.EnemyName).ToList();

        result.ForEach(ctx => Debug.Log(ctx));

    }

    [Button]
    public void TestTake()
    {
        var takeTest = numbers.Take(2).ToList();
        takeTest.ForEach(ctx => Debug.Log(ctx));
    }
    [Button]
    public void TestSkip()
    {
        var takeTest = numbers.Skip(3).ToList();
        takeTest.ForEach(ctx => Debug.Log(ctx));
    }
    public enum Type
    {
        None,
        Fire,
        Water,
        Earth
    }
    public struct Ability
    {
        public string AbilityName;
        public Type AbilityType;
        public Ability(string abilityName, Type abilityType)
        {
            AbilityName = abilityName;
            AbilityType = abilityType;
        }
    }
    [Button]
    public void TestGruopBy()
    {
        List<Ability> abilitys = new();
        abilitys.Add(new("Shuriken", Type.Earth));
        abilitys.Add(new("bamboo", Type.Earth));

        abilitys.Add(new("waterball", Type.Water));
        abilitys.Add(new("BubblePistol", Type.Water));

        abilitys.Add(new("FireWhip", Type.Fire));
        abilitys.Add(new("FireMoth", Type.Fire));

        var groupAbilitys = abilitys.GroupBy(key => key.AbilityType);

        Dictionary<Type, List<string>> dic = groupAbilitys.ToDictionary
            (
            group => group.Key,
            group => group.Select(ability => ability.AbilityName).ToList()
            );

        Dictionary<Type, List<Ability>> dic2 = groupAbilitys.ToDictionary
           (
           group => group.Key,
           group => group.Select(ability => ability).ToList()
           );


    }
    // public List<int> numbers = new List<int>() { 2,13,5,1,8,10,4};
    [Button]
    public void TestChainLinq()
    {
        var result =
            numbers.Where(x => x != 1)
            .OrderByDescending(x => x)
            .Take(3)
            .Select(x => x.ToString())
            .ToList();
    }

    [Button]
    public void testAll()
    {
        bool result =  numbers.All(x => x != 1);
    }

    public void testContains()
    {
        bool result = numbers.Contains(1);
    }

    public void TestCount()
    {
        int count = numbers.Count(x => x > 1);
    }
}

