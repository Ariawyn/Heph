using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Heph.Scripts.Character
{
    [Serializable]
    public class Stat
    {
        // This used to be float values, changed it to int since simpler, but maybe casting from applying percentage mod is not great?
        public int baseValue;
        protected readonly List<StatModifier> Modifiers;
        public readonly ReadOnlyCollection<StatModifier> ViewableModifiers;

        public virtual int Value
        {
            get 
            {
                if (!HasChanged && baseValue == MostRecentBaseValue) return MostRecentValue;
                MostRecentBaseValue = baseValue;
                MostRecentValue = CalculateFinalValue();
                HasChanged = false;
                return MostRecentValue;
            }
            set
            {
                baseValue = value;
                HasChanged = true;
            }
        }

        protected bool HasChanged = true;
        protected int MostRecentBaseValue;
        protected int MostRecentValue;

        public Stat()
        {
            Modifiers = new List<StatModifier>();
            ViewableModifiers = Modifiers.AsReadOnly();
        }

        public Stat(int baseValue) : this()
        {
            this.baseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            HasChanged = true;

            Modifiers.Add(mod);
            Modifiers.Sort(CompareModifierOrder);
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            var hasRemovedModifier = Modifiers.Remove(mod);
            return hasRemovedModifier;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            var hasRemovedModifiers = false;

            for(var i = Modifiers.Count - 1; i >= 0; i--)
            {
                if (Modifiers[i].Source != source) continue;
                HasChanged = true;
                hasRemovedModifiers = true;
                Modifiers.RemoveAt(i);
            }

            return hasRemovedModifiers;
        }

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if(a.Order < b.Order)
                return -1;
            else if(a.Order > b.Order)
                return 1;
            return 0;
        }

        protected virtual int CalculateFinalValue()
        {
            var finalValue = baseValue;
            var additivePercentageModifierValue = 0;

            for(var i = 0; i < Modifiers.Count; i++)
            {
                var modifier = Modifiers[i];

                switch(modifier.Type)
                {
                    case StatModType.Flat:
                        finalValue += modifier.Value;
                        break;
                    case StatModType.AdditivePercent:
                        additivePercentageModifierValue += modifier.Value;
                        var shouldFinishApplying = (i + 1 >= Modifiers.Count || Modifiers[i + 1].Type != StatModType.AdditivePercent);
                        if(shouldFinishApplying)
                        {
                            finalValue  = (int)(finalValue * (1 + additivePercentageModifierValue));
                        }
                        break;
                    case StatModType.MultiplicativePercent:
                        finalValue  = (int)(finalValue * (1 + modifier.Value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return finalValue;
        }
    }
}
