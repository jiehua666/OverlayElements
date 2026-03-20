// Skill System - Active and passive skills
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;
using OverlayElements.Card;
using OverlayElements.Game;

namespace OverlayElements.Skills
{
    /// <summary>
    /// Base skill class
    /// </summary>
    [Serializable]
    public abstract class Skill
    {
        public string skillId;
        public string skillName;
        public string description;
        public int cooldown;
        public int currentCooldown;
        public SkillTarget targetType;
        public SkillTiming timing;

        public virtual void Initialize()
        {
            currentCooldown = 0;
        }

        public abstract void Execute(CardInstance caster, CardInstance target, Player casterPlayer, Player opponentPlayer);
        
        public virtual bool CanExecute(CardInstance caster, CardInstance target)
        {
            return currentCooldown == 0;
        }

        public virtual void OnTurnStart(CardInstance caster)
        {
            if (currentCooldown > 0)
                currentCooldown--;
        }

        public virtual void OnTurnEnd(CardInstance caster)
        {
            // Override in subclasses
        }

        public virtual void StartCooldown()
        {
            currentCooldown = cooldown;
        }
    }

    /// <summary>
    /// Attack skill - deal damage
    /// </summary>
    [Serializable]
    public class AttackSkill : Skill
    {
        public int damage;
        public bool ignoreDefense;

        public AttackSkill()
        {
            skillId = "attack_basic";
            skillName = "攻击";
            description = "造成伤害";
            targetType = SkillTarget.EnemyCard;
            timing = SkillTiming.Main;
        }

        public override void Execute(CardInstance caster, CardInstance target, Player casterPlayer, Player opponentPlayer)
        {
            if (target == null) return;

            int finalDamage = ignoreDefense ? damage : damage + target.Template.attack;
            target.TakeDamage(finalDamage);
            StartCooldown();
        }
    }

    /// <summary>
    /// Heal skill - restore health
    /// </summary>
    [Serializable]
    public class HealSkill : Skill
    {
        public int healAmount;
        public bool healSelf;

        public HealSkill()
        {
            skillId = "heal_basic";
            skillName = "治疗";
            description = "恢复生命";
            targetType = SkillTarget.AllyCard;
            timing = SkillTiming.Main;
        }

        public override void Execute(CardInstance caster, CardInstance target, Player casterPlayer, Player opponentPlayer)
        {
            CardInstance targetCard = healSelf ? caster : target;
            if (targetCard != null)
            {
                targetCard.Heal(healAmount);
                StartCooldown();
            }
        }
    }

    /// <summary>
    /// Buff skill - increase stats
    /// </summary>
    [Serializable]
    public class BuffSkill : Skill
    {
        public int attackBoost;
        public int healthBoost;
        public int duration;

        private int currentDuration;

        public BuffSkill()
        {
            skillId = "buff_basic";
            skillName = "强化";
            description = "提升属性";
            targetType = SkillTarget.AllyCard;
            timing = SkillTiming.Main;
        }

        public override void Initialize()
        {
            base.Initialize();
            currentDuration = duration;
        }

        public override void Execute(CardInstance caster, CardInstance target, Player casterPlayer, Player opponentPlayer)
        {
            if (target == null) return;
            
            // Apply buff effect (simplified - in full implementation would modify card stats)
            Debug.Log($"[{skillName}] Applied to {target.CardName}");
            StartCooldown();
        }

        public override void OnTurnEnd(CardInstance caster)
        {
            currentDuration--;
            if (currentDuration <= 0)
            {
                // Remove buff effect
                Debug.Log($"[{skillName}] Buff expired on {caster.CardName}");
            }
        }
    }

    /// <summary>
    /// Draw skill - draw cards
    /// </summary>
    [Serializable]
    public class DrawSkill : Skill
    {
        public int cardsToDraw;

        public DrawSkill()
        {
            skillId = "draw_basic";
            skillName = "抽牌";
            description = "抽取卡牌";
            targetType = SkillTarget.None;
            timing = SkillTiming.Main;
        }

        public override void Execute(CardInstance caster, CardInstance target, Player casterPlayer, Player opponentPlayer)
        {
            casterPlayer.DrawCards(cardsToDraw);
            StartCooldown();
        }
    }

    /// <summary>
    /// Element burst skill - elemental reaction
    /// </summary>
    [Serializable]
    public class ElementBurstSkill : Skill
    {
        public ElementType element;
        public int baseDamage;
        public ElementReaction reaction;

        public ElementBurstSkill()
        {
            skillId = "element_burst";
            skillName = "元素爆发";
            targetType = SkillTarget.AllEnemies;
            timing = SkillTiming.Main;
        }

        public override void Execute(CardInstance caster, CardInstance target, Player casterPlayer, Player opponentPlayer)
        {
            // Deal damage to all enemy cards
            foreach (var card in opponentPlayer.Deck.Field)
            {
                card.TakeDamage(baseDamage);
            }
            
            // Deal damage to opponent player
            opponentPlayer.TakeDamage(baseDamage / 2);
            
            StartCooldown();
        }
    }

    /// <summary>
    /// Skill manager
    /// </summary>
    public class SkillManager
    {
        private Dictionary<string, Skill> skills = new Dictionary<string, Skill>();

        public SkillManager()
        {
            // Register default skills
            RegisterSkill(new AttackSkill());
            RegisterSkill(new HealSkill());
            RegisterSkill(new BuffSkill());
            RegisterSkill(new DrawSkill());
            RegisterSkill(new ElementBurstSkill());
        }

        public void RegisterSkill(Skill skill)
        {
            skill.Initialize();
            skills[skill.skillId] = skill;
        }

        public Skill GetSkill(string skillId)
        {
            return skills.TryGetValue(skillId, out var skill) ? skill : null;
        }

        public List<Skill> GetAvailableSkills(CardInstance card)
        {
            var available = new List<Skill>();
            foreach (var skillId in card.Template.activeSkillIds)
            {
                var skill = GetSkill(skillId);
                if (skill != null && skill.CanExecute(card, null))
                {
                    available.Add(skill);
                }
            }
            return available;
        }
    }

    /// <summary>
    /// Skill target types
    /// </summary>
    public enum SkillTarget
    {
        None,
        Self,
        AllyCard,
        EnemyCard,
        AllEnemies,
        AllAllies,
        RandomEnemy,
        RandomAlly
    }

    /// <summary>
    /// Skill timing
    /// </summary>
    public enum SkillTiming
    {
        Passive,      // Always active
        OnPlay,       // When card is played
        OnAttack,     // When card attacks
        OnDeath,      // When card dies
        Main,         // During main phase
        Start,        // Turn start
        End           // Turn end
    }
}
