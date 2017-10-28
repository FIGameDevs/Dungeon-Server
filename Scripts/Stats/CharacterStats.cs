using System;
using System.Collections;
using System.Collections.Generic;

namespace Stats
{
    public class UniqueId
    {
        int id;
        static int counter = 0;

        public UniqueId()
        {
            id = counter;
            counter++;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
    //Use Add to add your effects
    //Use ApplyStats to transform your number through the inheritance system
    //Additive numbers just add to the base stat
    //Multiplicative numbers add a percentage to the stat after additive is added. 
    //READ: multiplicative: -0.15 means substract 15%. 2 means add 200%
    public class CharacterStats
    {

        Dictionary<PerkNode, Dictionary<UniqueId, float>> additive = new Dictionary<PerkNode, Dictionary<UniqueId, float>>();
        Dictionary<PerkNode, Dictionary<UniqueId, float>> multiplicative = new Dictionary<PerkNode, Dictionary<UniqueId, float>>();



        public UniqueId AddAdditive(PerkNode node, float amount)
        {
            Dictionary<UniqueId, float> dic;
            if (!additive.TryGetValue(node, out dic))
            {
                dic = new Dictionary<UniqueId, float>();
                additive.Add(node, dic);
            }
            var id = new UniqueId();
            dic[id] = amount;
            return id;
        }

        public void SetAdditive(PerkNode node, UniqueId id, float amount)
        {
            Dictionary<UniqueId, float> dic;
            if (!additive.TryGetValue(node, out dic))
            {
                dic = new Dictionary<UniqueId, float>();
                additive.Add(node, dic);
            }
            dic[id] = amount;
        }
        public void RemoveAdditive(PerkNode node, UniqueId id)
        {
            Dictionary<UniqueId, float> dic;
            if (additive.TryGetValue(node, out dic))
            {
                dic.Remove(id);
            }
        }
        public void RemoveMultiplicative(PerkNode node, UniqueId id)
        {
            Dictionary<UniqueId, float> dic;
            if (multiplicative.TryGetValue(node, out dic))
            {
                dic.Remove(id);
            }
        }
        public void RemoveBoth(PerkNode node, UniqueId id)
        {
            Dictionary<UniqueId, float> dic;
            if (additive.TryGetValue(node, out dic))
            {
                dic.Remove(id);
            }
            if (multiplicative.TryGetValue(node, out dic))
            {
                dic.Remove(id);
            }
        }

        public UniqueId AddMultiplicative(PerkNode node, float amount)
        {
            Dictionary<UniqueId, float> dic;
            if (!multiplicative.TryGetValue(node, out dic))
            {
                dic = new Dictionary<UniqueId, float>();
                multiplicative.Add(node, dic);
            }
            var id = new UniqueId();
            dic[id] = amount;
            return id;
        }

        public void SetMultiplicative(PerkNode node, UniqueId id, float amount)
        {
            Dictionary<UniqueId, float> dic;
            if (!multiplicative.TryGetValue(node, out dic))
            {
                dic = new Dictionary<UniqueId, float>();
                multiplicative.Add(node, dic);
            }
            dic[id] = amount;
        }

        public float ApplyStats(PerkNode node, float baseAmount)
        {
            float add = 0;
            float mul = 0;
            Queue<PerkNode> nodes = new Queue<PerkNode>();
            nodes.Enqueue(node);

            do
            {
                var n = nodes.Dequeue();

                add += GetAdd(n);
                mul += GetMul(n);

                var p = n.Parents;
                if (p == null)
                    continue;
                for (int i = 0; i < p.Length; i++)
                {
                    nodes.Enqueue(p[i]);
                }
            }
            while (nodes.Count != 0);

            return (baseAmount + add) + (baseAmount + add) * mul;
        }


        float GetAdd(PerkNode node)
        {
            Dictionary<UniqueId, float> dic;
            if (additive.TryGetValue(node, out dic))
            {
                float add = 0;
                foreach (var item in dic.Values)
                {
                    add += item;
                }
                return add;
            }
            return 0;
        }

        float GetMul(PerkNode node)
        {
            Dictionary<UniqueId, float> dic;
            if (multiplicative.TryGetValue(node, out dic))
            {
                float mul = 0;
                foreach (var item in dic.Values)
                {
                    mul += item;
                }
                return mul;
            }
            return 0;
        }

    }
}