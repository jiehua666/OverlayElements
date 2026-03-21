# 叠印元素 - 卡牌AI生成提示词文档

> **版本**: v1.0  
> **更新日期**: 2026-03-21  
> **用途**: 为每张卡牌生成高质量AI人像图  
> **推荐工具**: ComfyUI + Illustrious/Flux 模型

---

## 卡牌视觉风格指南

### 整体风格
- **艺术风格**: Fantasy card game, detailed illustration, high quality
- **画面比例**: 3:4 (768x960 或 1024x1280)
- **视角**: 半身人像，胸部以上为主
- **背景**: 简洁，元素色渐变背景

### 元素配色
| 元素 | 主色调 | 辅色调 | 点缀色 |
|------|--------|--------|--------|
| 火 (FIRE) | 深红 #DC3C1E | 橙黄 #FF8C00 | 金黄 #FFC832 |
| 水 (WATER) | 深蓝 #1E64C8 | 天蓝 #50A0FF | 浅蓝 #96C8FF |
| 风 (WIND) | 银灰蓝 #B4C8DC | 浅灰 #DCE6F0 | 纯白 #FFFFFF |
| 木 (WOOD) | 深绿 #3C7832 | 草绿 #64A050 | 浅绿 #B4DC78 |

### 卡牌边框
- 已生成四种元素边框模板: `03_美术资源/card_frames/`
- 使用时将人物图放置在上半部分，下半部分放置文字

---

## 卡牌提示词详情

---

### 🔥 FIRE_001 - 火龙战士 (Fire Dragon Warrior)

| 属性 | 值 |
|------|-----|
| 费用 | 3 |
| 攻击 | 4 |
| 生命 | 5 |
| 类型 | 战士 |
| 稀有度 | ★★ (稀有) |
| 描述 | 火籍战士，披拂火焰之力 |

**提示词 (Positive Prompt)**:
```
fire element fantasy warrior, muscular male warrior with burning flame armor, 
molten lava patterns on chest plate, flame wings made of fire, fierce 
determined expression, long red hair flowing with flames, holding flaming 
sword, dramatic fire particles surrounding, dark background with orange fire 
glow, detailed face, sharp eyes, epic fantasy art style, card game illustration, 
intricate armor design, high quality, 8k
```

**负向提示词 (Negative Prompt)**:
```
blurry, low quality, distorted face, extra limbs, bad anatomy, watermark, 
text overlay, mutated hands, deformed, ugly
```

**风格关键词**: `fire warrior, flame armor, epic fantasy, dramatic lighting`

---

### 🔥 FIRE_002 - 火焰射手 (Flame Archer)

| 属性 | 值 |
|------|-----|
| 费用 | 2 |
| 攻击 | 3 |
| 生命 | 2 |
| 类型 | 战士 |
| 稀有度 | ★ (普通) |
| 描述 | 简单的火籍射手 |

**提示词**:
```
fire element fantasy archer, agile young woman archer, crimson red leather 
armor with flame embroidery, flaming bow made of fire essence, quiver with 
flaming arrows, confident smirk expression, flowing red cape, hair tied up 
with flame ornaments, ember particles floating, dark orange background with 
fire glow, dynamic pose aiming bow, fantasy card illustration style, 
detailed costume, high quality, 8k
```

**负向提示词**:
```
blurry, low quality, bad anatomy, extra fingers, deformed, watermark, 
text overlay, ugly, duplicate
```

**风格关键词**: `fire archer, flame bow, agile warrior, dynamic pose`

---

### 💧 WATER_001 - 水之女王 (Water Queen)

| 属性 | 值 |
|------|-----|
| 费用 | 4 |
| 攻击 | 2 |
| 生命 | 7 |
| 类型 | 法师 |
| 稀有度 | ★★ (稀有) |
| 描述 | 水籍法师，擅长治癌和控制 |

**提示词**:
```
water element fantasy queen, elegant mature woman in flowing royal water 
dress, crown made of crystallized water, translucent blue dress with fish 
scale patterns, serene yet powerful expression, long silver-blue hair 
floating underwater, holding ornate water orb staff, water spirits 
surrounding, deep ocean blue background with light rays, majestic posture, 
fantasy card illustration, detailed dress design, ethereal beauty, 
high quality, 8k
```

**负向提示词**:
```
blurry, low quality, bad anatomy, deformed, watermark, text overlay, 
mutated hands, extra limbs, ugly
```

**风格关键词**: `water queen, royal mage, ethereal beauty, ocean theme`

---

### 💧 WATER_002 - 海蛇 (Sea Serpent)

| 属性 | 值 |
|------|-----|
| 费用 | 3 |
| 攻击 | 3 |
| 生命 | 5 |
| 类型 | 生物 |
| 稀有度 | ★ (普通) |
| 描述 | 海籍蛇形生物 |

**提示词**:
```
water element fantasy sea serpent creature, beautiful mermaid-like upper 
body with serpent lower body, blue-green iridescent scales, long flowing 
hair like kelp, sharp intelligent eyes, tail with fin decorations, 
coral and shell ornaments, surrounded by small fish and bubbles, 
underwater cave background with light filtering through, fantasy 
creature illustration, semi-humanoid pose, mysterious atmosphere, 
high quality, 8k
```

**负向提示词**:
```
blurry, low quality, distorted, extra limbs, bad anatomy, watermark, 
text overlay, deformed, ugly, mutated
```

**风格关键词**: `sea serpent, mermaid creature, underwater fantasy, serpent`

---

### 🌪️ WIND_001 - 风之精灵 (Wind Spirit)

| 属性 | 值 |
|------|-----|
| 费用 | 2 |
| 攻击 | 3 |
| 生命 | 3 |
| 类型 | 法师 |
| 稀有度 | ★★ (稀有) |
| 描述 | 风籍纪灵，逋廸风中的利灵 |

**提示词**:
```
wind element fantasy wind spirit, ethereal androgynous being made of 
glowing wind wisps, translucent flowing robes in white and pale blue, 
no solid form visible through body, long hair swirling like wind 
currents, serene floating expression, holding swirling wind orb, small 
feather-like particles around, dreamy white and silver gradient 
background with wind swirls, delicate fantasy spirit illustration, 
graceful pose, high quality, 8k
```

**负向提示词**:
```
blurry, low quality, dark, solid form, heavy body, watermark, 
text overlay, deformed, ugly
```

**风格关键词**: `wind spirit, ethereal being, wind wisp, delicate fantasy`

---

### 🌪️ WIND_002 - 风暴之鸟 (Storm Bird)

| 属性 | 值 |
|------|-----|
| 费用 | 2 |
| 攻击 | 2 |
| 生命 | 4 |
| 类型 | 生物 |
| 稀有度 | ★ (普通) |
| 描述 | 风籍鸟类，速度极快 |

**提示词**:
```
wind element fantasy storm bird, majestic large bird with lightning 
patterns on feathers, silver-white plumage with electric blue streaks, 
sharp determined eyes, wings spread wide creating wind currents, 
feathers crackling with static electricity, perched on cloud or wind 
stream pose, dramatic stormy sky background with lightning in distance, 
fantasy creature card illustration, powerful and swift appearance, 
high quality, 8k
```

**负向提示词**:
```
blurry, low quality, bad anatomy, extra limbs, deformed, watermark, 
text overlay, ugly, broken wings
```

**风格关键词**: `storm bird, thunder bird, lightning feathers, swift creature`

---

### 🌿 WOOD_001 - 森林守护 (Forest Guardian)

| 属性 | 值 |
|------|-----|
| 费用 | 3 |
| 攻击 | 2 |
| 生命 | 8 |
| 类型 | 战士 |
| 稀有度 | ★★ (稀有) |
| 描述 | 树籍守护者，固定的防线 |

**提示词**:
```
wood element fantasy forest guardian, massive armored treant humanoid, 
bark-like armor with moss and small plants growing on it, ancient wise 
face with wooden features, glowing green eyes, vines wrapping around 
arms as weapons, flowers blooming on shoulders, solid grounded stance, 
enchanted forest background with large ancient trees, fantasy guardian 
illustration, protective solid presence, high quality, 8k
```

**负向提示词**:
```
blurry, low quality, bad anatomy, deformed, watermark, text overlay, 
ugly, extra limbs, mutated
```

**风格关键词**: `forest guardian, treant warrior, ancient protector, wooden armor`

---

### 🌿 WOOD_002 - 树精 (Treant)

| 属性 | 值 |
|------|-----|
| 费用 | 4 |
| 攻击 | 2 |
| 生命 | 10 |
| 类型 | 战士 |
| 稀有度 | ★ (普通) |
| 描述 | 树籍树人，壮键的嘉構 |

**提示词**:
```
wood element fantasy treant, massive ancient humanoid tree creature, 
thick bark skin with deep wood grain patterns, moss and mushroom 
growths on body, solid muscular wooden torso, small glowing life essence 
orbs scattered, serene ancient expression, sturdy rooted stance, 
mystical forest clearing background with floating leaves, fantasy 
creature card illustration, massive imposing figure, high quality, 8k
```

**负向提示词**:
```
blurry, low quality, bad anatomy, deformed, watermark, text overlay, 
ugly, thin, malnourished appearance
```

**风格关键词**: `treant, ancient tree creature, massive guardian, forest giant`

---

## 使用说明

### ComfyUI 工作流建议

1. **模型选择**: 推荐使用 `Illustrious` 或 `Flux` 模型
2. **采样器**: DPM++ 2M Karras / Euler a
3. **步数**: 25-35步
4. **CFG**: 7-8
5. **分辨率**: 768x960 (3:4)

### 提示词模板

```json
{
  "positive": "[角色描述], fantasy card game illustration, high quality, 8k, detailed, dramatic lighting",
  "negative": "blurry, low quality, bad anatomy, deformed, watermark, text overlay, ugly",
  "style": "element_color_palette"
}
```

### 生成的图片处理流程

1. 人物图生成后，裁剪到合适尺寸
2. 使用对应元素的边框模板 (768x960 PNG)
3. 将人物图放置在上半部分 (0-480px)
4. 下半部分 (480-960px) 留给文字区域
5. 统一导出为 PNG 格式

---

## 元素边框模板位置

```
C:\Users\PC\Desktop\项目文件夹\03_美术资源\card_frames\
├── card_frame_fire.png   (火元素边框)
├── card_frame_water.png  (水元素边框)
├── card_frame_wind.png   (风元素边框)
└── card_frame_wood.png   (木元素边框)
```

---

## 更新记录

| 版本 | 日期 | 更新内容 |
|------|------|----------|
| v1.0 | 2026-03-21 | 初始版本，包含8张卡牌提示词 |

