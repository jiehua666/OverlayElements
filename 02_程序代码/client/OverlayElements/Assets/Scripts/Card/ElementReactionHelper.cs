// Element Reaction System
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// Element reaction definitions
    /// </summary>
    public static class ElementReactions
    {
        /// <summary>
        /// Get reaction info
        /// </summary>
        public static ReactionInfo GetReactionInfo(ElementReaction reaction)
        {
            return reaction switch
            {
                ElementReaction.Evaporate => new ReactionInfo(
                    "蒸发", "火+水", "互相抵消，造成范围伤害",
                    new Color(0.8f, 0.9f, 1f), 15
                ),
                ElementReaction.Blight => new ReactionInfo(
                    "枯萎", "火+木", "火焰燃烧木属性，造成持续伤害",
                    new Color(0.8f, 0.4f, 0f), 8
                ),
                ElementReaction.Mud => new ReactionInfo(
                    "泥泞", "水+木", "减速敌人，降低行动速度",
                    new Color(0.4f, 0.3f, 0.2f), 0
                ),
                ElementReaction.Storm => new ReactionInfo(
                    "风暴", "水+风", "召唤风暴，造成雷电伤害",
                    new Color(0.3f, 0.5f, 0.9f), 20
                ),
                ElementReaction.Wildfire => new ReactionInfo(
                    "野火", "火+风", "火势蔓延，伤害扩散",
                    new Color(1f, 0.5f, 0f), 12
                ),
                ElementReaction.Photosynthesis => new ReactionInfo(
                    "光合", "风+木", "回复生命，获得增益",
                    new Color(0.5f, 0.9f, 0.4f), -10
                ),
                _ => new ReactionInfo("无", "-", "无反应", Color.gray, 0)
            };
        }

        /// <summary>
        /// Check if two elements create a reaction
        /// </summary>
        public static bool HasReaction(ElementType a, ElementType b)
        {
            if (a == ElementType.None || b == ElementType.None) return false;
            if (a == b) return false; // Same element = no reaction
            
            return (a, b) switch
            {
                (ElementType.Fire, ElementType.Water) => true,
                (ElementType.Water, ElementType.Fire) => true,
                (ElementType.Fire, ElementType.Wood) => true,
                (ElementType.Wood, ElementType.Fire) => true,
                (ElementType.Water, ElementType.Wood) => true,
                (ElementType.Wood, ElementType.Water) => true,
                (ElementType.Water, ElementType.Wind) => true,
                (ElementType.Wind, ElementType.Water) => true,
                (ElementType.Fire, ElementType.Wind) => true,
                (ElementType.Wind, ElementType.Fire) => true,
                (ElementType.Wind, ElementType.Wood) => true,
                (ElementType.Wood, ElementType.Wind) => true,
                _ => false
            };
        }

        /// <summary>
        /// Get all reactions for an element
        /// </summary>
        public static List<ElementReaction> GetReactionsFor(ElementType element)
        {
            List<ElementReaction> reactions = new List<ElementReaction>();
            
            foreach (ElementReaction reaction in Enum.GetValues(typeof(ElementReaction)))
            {
                if (reaction == ElementReaction.None) continue;
                
                var info = GetReactionInfo(reaction);
                if (info.Elements.Contains(GetElementName(element)))
                {
                    reactions.Add(reaction);
                }
            }
            
            return reactions;
        }

        private static string GetElementName(ElementType element)
        {
            return element switch
            {
                ElementType.Fire => "火",
                ElementType.Water => "水",
                ElementType.Wind => "风",
                ElementType.Wood => "木",
                _ => ""
            };
        }
    }

    /// <summary>
    /// Reaction information
    /// </summary>
    public struct ReactionInfo
    {
        public string Name;
        public string Elements;
        public string Description;
        public Color EffectColor;
        public int BaseDamage;

        public ReactionInfo(string name, string elements, string desc, Color color, int damage)
        {
            Name = name;
            Elements = elements;
            Description = desc;
            EffectColor = color;
            BaseDamage = damage;
        }
    }
}
