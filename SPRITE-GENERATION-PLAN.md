# Pully - Sprite Generation Plan
## Game Art Agent (game-art)

### Project Context
- **Game**: Pully - One-thumb arcade gesture game
- **Theme**: "Critters" - cute creatures that react
- **Target Platform**: Unity 6 LTS, Android (Portrait), 2D sprites in 3D project
- **Style**: Flat vector with subtle gradients, high contrast, mobile-friendly

---

## Phase 1: Asset Inventory & Requirements

### Target Shapes (4 shapes, 5 color variants each)
From RULESET.md:

| Shape | Primary Color | Hex Code | Required Gesture | Critter Personality |
|-------|--------------|----------|------------------|---------------------|
| Circle | Green | #4CAF50 | Single tap | Friendly, bouncy blob |
| Circle | Red | #F44336 | Long press | Anxious, ticking bomb-like |
| Square | Blue | #2196F3 | Double tap | Solid, weighty block |
| Triangle | Yellow | #FFEB3B | Swipe-tap | Fast, directional pointer |
| Star | Purple | #9C27B0 | Two-finger tap | Special, sparkly rarer |

**Note**: There are 4 base shapes (Circle, Square, Triangle, Star) but Circle appears in 2 colors (Green=normal, Red=trap/long-press).

### Animation States Per Target
Each critter needs sprites for:
1. **Idle** - idle blink (2-frame animation: eyes open, eyes closed)
2. **Panic** - as expiry approaches (shaking/wobble frames)
3. **Hit/Happy** - success reaction (smile widens, stars in eyes)
4. **Squash** - when tapped (squash frame)

---

## Phase 2: Sprite Asset List

### A. Target Sprites (Primary Assets)

#### Folder: `Assets/_Game/Sprites/Targets/`

**Circle Critters:**
| Filename | Size | Description |
|----------|------|-------------|
| `circle_green_idle_01.png` | 128x128 | Green circle, normal eyes |
| `circle_green_idle_02.png` | 128x128 | Green circle, blinking |
| `circle_green_panic.png` | 128x128 | Green circle, X_X eyes, sweat drops |
| `circle_green_hit.png` | 128x128 | Green circle, big smile, sparkles |
| `circle_red_idle_01.png` | 128x128 | Red circle, worried eyes |
| `circle_red_idle_02.png` | 128x128 | Red circle, blinking worried |
| `circle_red_panic.png` | 128x128 | Red circle, ticking clock face |
| `circle_red_hit.png` | 128x128 | Red circle, relieved expression |

**Square Critters:**
| Filename | Size | Description |
|----------|------|-------------|
| `square_blue_idle_01.png` | 128x128 | Blue square, neutral face |
| `square_blue_idle_02.png` | 128x128 | Blue square, blinking |
| `square_blue_panic.png` | 128x128 | Blue square, cracks appearing |
| `square_blue_hit.png` | 128x128 | Blue square, metallic shine, happy |

**Triangle Critters:**
| Filename | Size | Description |
|----------|------|-------------|
| `triangle_yellow_idle_01.png` | 128x128 | Yellow triangle, alert face |
| `triangle_yellow_idle_02.png` | 128x128 | Yellow triangle, blinking |
| `triangle_yellow_panic.png` | 128x128 | Yellow triangle, vibration lines |
| `triangle_yellow_hit.png` | 128x128 | Yellow triangle, proud expression |

**Star Critters:**
| Filename | Size | Description |
|----------|------|-------------|
| `star_purple_idle_01.png` | 128x128 | Purple star, starry eyes |
| `star_purple_idle_02.png` | 128x128 | Purple star, blinking |
| `star_purple_panic.png` | 128x128 | Purple star, fading/pulse |
| `star_purple_hit.png` | 128x128 | Purple star, rainbow burst happy |

**Squash Variants:**
Each shape needs a squash frame (tall-to-short deformation on interaction):
- `circle_green_squash.png` (stretched horizontally, compressed vertically)
- `circle_red_squash.png`
- `square_blue_squash.png`
- `triangle_yellow_squash.png`
- `star_purple_squash.png`

**Total Target Sprites: 25 files**

---

### B. UI Sprites

#### Folder: `Assets/_Game/Sprites/UI/`

| Filename | Size | Description |
|----------|------|-------------|
| `button_play.png` | 256x96 | Play button with rounded corners |
| `button_settings.png` | 64x64 | Settings gear icon |
| `button_pause.png` | 64x64 | Pause || icon |
| `button_restart.png` | 64x64 | Circular restart arrow |
| `button_home.png` | 64x64 | Home house icon |
| `frame_menu.png` | 512x384 | Menu background panel |
| `frame_hud.png` | 480x96 | HUD background bar |
| `icon_heart_full.png` | 64x64 | Full heart (life) |
| `icon_heart_empty.png` | 64x64 | Empty heart outline |
| `icon_star.png` | 64x64 | Star for score/high score |
| `icon_combo_orb.png` | 96x96 | Combo multiplier orb background |
| `slider_track.png` | 192x32 | Settings slider track |
| `slider_handle.png` | 48x48 | Settings slider handle |

**Total UI Sprites: 13 files**

---

### C. FX / Particle Sprites

#### Folder: `Assets/_Game/Sprites/FX/`

| Filename | Size | Description |
|----------|------|-------------|
| `particle_hit.png` | 32x32 | Circular burst particle |
| `particle_star.png` | 32x32 | Star shaped sparkle |
| `particle_line.png` | 64x8 | Streak/line particle for trails |
| `flash_hit.png` | 128x128 | Radial flash burst |
| `flash_miss.png` | 128x128 | Red X or flash for miss |
| `combo_glow_1.png` | 256x256 | Faint glow for x2 combo |
| `combo_glow_2.png` | 256x256 | Brighter glow for x3 combo |
| `combo_glow_3.png` | 256x256 | Intense glow for x5 combo |
| `text_pop_combo.png` | 128x64 | "COMBO!" text popup |
| `text_pop_score.png` | 128x64 | "+10" etc text backdrop |

**Total FX Sprites: 10 files**

---

### D. Splash/App Icons

#### Folder: `Assets/_Game/Sprites/`

| Filename | Size | Description |
|----------|------|-------------|
| `icon_app.png` | 512x512 | App icon - stylized P with critter face |
| `splash_logo.png` | 1024x512 | Splash screen logo |
| `splash_background.png` | 1080x1920 | Portrait splash bg (dark navy) |

**Total Icon/Splash: 3 files**

---

## Phase 3: Technical Specifications

### File Format & Settings
- **Format**: PNG with transparency (RGBA)
- **Dimensions**: Power-of-two preferred (64x64, 128x128, 256x256)
- **PPU (Pixels Per Unit)**: 100 (matches Unity default)
- **Filter Mode**: Bilinear (for smooth vector art scaling)
- **Compression**: RGBA Compressed (for mobile memory)

### Color Palette (Strict)
| Usage | Hex | RGB |
|-------|-----|-----|
| Green (Circle) | #4CAF50 | 76,175,80 |
| Red (Circle-trap) | #F44336 | 244,67,54 |
| Blue (Square) | #2196F3 | 33,150,243 |
| Yellow (Triangle) | #FFEB3B | 255,235,59 |
| Purple (Star) | #9C27B0 | 156,39,176 |
| Background | #0D1B2A | 13,27,42 |
| UI Accent | #00BCD4 | 0,188,212 |
| White | #FFFFFF | 255,255,255 |
| Shadow/Outline | #000000 | 0,0,0 (30% opacity) |

### Accessibility Requirements
- Each shape must be distinguishable by silhouette alone
- Colors must have 4.5:1 contrast ratio against #0D1B2A background
- Animations must not flash >3 times per second

---

## Phase 4: Sprite Generation Strategy

### Generation Method
Since we don't have access to AI image generation tools in this context, we will:

1. **Procedural Generation via Python/Pillow**: Generate clean vector-style sprites programmatically
2. **SVG-based**: Create SVG templates and rasterize to PNG
3. **Unity-based creation**: Use Unity's built-in sprite tools

**Chosen approach**: Python script using Pillow to generate clean, consistent sprites programmatically.

### Tools Required
1. **Python 3** with Pillow library for image generation
2. **Unity 6 LTS** for sprite import and atlas packing
3. **Optional**: TexturePacker if Unity atlas has issues

### Generation Script Components

```python
# Key functions needed:
- draw_critter_circle(color, expression, size) 
- draw_critter_square(color, expression, size)
- draw_critter_triangle(color, expression, size)
- draw_critter_star(color, expression, size)
- draw_ui_button(label, style)
- draw_ui_icon(type, state)
- draw_fx_particle(type)
```

---

## Phase 5: Delivery Checklist

### M2-006 Acceptance Criteria
- [ ] 4 target shapes rendered (Circle, Square, Triangle, Star)
- [ ] 5 color variants per ruleset
- [ ] Multiple expressions per shape (idle, panic, hit, squash)
- [ ] UI elements complete (buttons, hearts, stars, frames)
- [ ] FX elements complete (particles, flashes, glows)
- [ ] All sprites in `Assets/_Game/Sprites/` with proper subfolders

### M2-007 Sprite Atlas
- [ ] Unity Sprite Atlas created at `Assets/_Game/Sprites/Atlas.spriteatlas`
- [ ] All target sprites packed
- [ ] UI sprites packed
- [ ] FX sprites packed
- [ ] Atlas settings: 4px padding, RGBA compressed, PPU 100

### File Structure
```
Assets/_Game/Sprites/
├── Targets/
│   ├── circle_green_*.png (5 files)
│   ├── circle_red_*.png (5 files)
│   ├── square_blue_*.png (4 files)
│   ├── triangle_yellow_*.png (4 files)
│   └── star_purple_*.png (4 files)
├── UI/
│   ├── button_*.png (5 files)
│   ├── frame_*.png (2 files)
│   ├── icon_*.png (6 files)
│   ├── slider_*.png (2 files)
│   └── text_*.png (optional)
├── FX/
│   ├── particle_*.png (3 files)
│   ├── flash_*.png (2 files)
│   ├── combo_glow_*.png (3 files)
│   └── text_pop_*.png (2 files)
├── icon_app.png
├── splash_logo.png
├── splash_background.png
└── Atlas.spriteatlas
```

---

## Phase 6: Estimated Timeline

| Task | Estimate |
|------|----------|
| Set up sprite generation script | 1h |
| Generate target critters | 3h |
| Generate UI elements | 1h |
| Generate FX sprites | 1h |
| Import to Unity + configure | 1h |
| Create Sprite Atlas + test | 1h |
| **Total M2-006 + M2-007** | **~8h** |

---

## Tools & Resources Required

### Software
1. **Python 3.10+** with:
   - Pillow (PIL) for image generation
   - numpy (optional, for gradients)
2. **Unity 6 LTS** (already in project)
3. **TexturePacker** (optional, if Unity atlas insufficient)

### Assets/Fonts
- Clean sans-serif font for UI text (Unity default Arial acceptable)
- No external assets required - all sprites generated procedurally

### Knowledge Resources
- Unity Sprite Atlas documentation
- Mobile sprite optimization guidelines
- Color contrast accessibility guidelines (WCAG 2.1)

---

## Next Steps

1. **Create sprite generation script** (`tools/generate-sprites.py`)
2. **Run generation** to create all PNG files
3. **Import into Unity** and configure import settings
4. **Create Sprite Atlas** and pack all sprites
5. **Update DESIGN.md** with final style rationale
6. **Move tasks to In Progress** and begin execution

---

*Plan created: 2025-06-08*  
*Agent: game-art*  
*Tasks: M2-006, M2-007*
