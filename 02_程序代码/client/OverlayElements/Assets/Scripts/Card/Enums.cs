// Card Enums
// Version: 0.1.0
// Created: 2026-03-20

namespace OverlayElements.Card
{
    /// <summary>
    /// Card element types (四象元素)
    /// </summary>
    public enum ElementType
    {
        None = 0,   // 无属性
        Fire = 1,   // 火
        Water = 2,  // 水
        Wind = 3,   // 风
        Wood = 4    // 木
    }

    /// <summary>
    /// Card rarity levels
    /// </summary>
    public enum CardRarity
    {
        Common = 1,     // 普通 (白)
        Rare = 2,       // 稀有 (蓝)
        Epic = 3,       // 史诗 (紫)
        Legendary = 4,  // 传说 (橙)
        Mythic = 5      // 神话 (红)
    }

    /// <summary>
    /// Card type/category
    /// </summary>
    public enum CardType
    {
        Warrior,        // 战士
        Mage,           // 法师
        Support,        // 辅助
        Spell,          // 法术
        Trap,           // 陷阱
        Token           // 衍生物
    }

    /// <summary>
    /// Elemental reaction types
    /// </summary>
    public enum ElementReaction
    {
        None,           // 无反应
        Evaporate,      // 火+水=蒸发
        Blight,         // 火+木=枯萎
        Mud,            // 水+木=泥泞
        Storm,          // 水+风=风暴
        Wildfire,       // 火+风=野火
        Photosynthesis  // 风+木=光合
    }

    /// <summary>
    /// Card zone/location
    /// </summary>
    public enum CardZone
    {
        Deck,           // 牌库
        Hand,           // 手牌
        Field,          // 场地
        Grave,          // 墓地
        Exile           // 放逐
    }

    /// <summary>
    /// Card state
    /// </summary>
    public enum CardState
    {
        Normal,         // 正常
        Exhausted,      // 疲劳（已行动）
        Sealed,         // 封印
        Destroyed       // 破坏中
    }
}
