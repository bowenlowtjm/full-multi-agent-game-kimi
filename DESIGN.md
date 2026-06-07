# DESIGN.md — Arcade Gesture Game Art Style

## Visual Identity

*Last updated: 2025-06-08*
*Theme: "Critters" - Each shape is a little creature with personality*

### Design Rationale
The "Critters" theme was chosen to create instant emotional connection with players. Each shape isn't just a geometric form—it's a living creature that reacts to player input:
- **Green Circle**: A bouncy, friendly blob. Simple and approachable—perfect for single-tap learning.
- **Red Circle**: The "anxious" version. Suggests danger with its worried expression—this is the trap/long-press target.
- **Blue Square**: Solid and weighty. Reinforces the need for double-tap through its blocky, metallic appearance.
- **Yellow Triangle**: Directional and energetic. Points players toward the swipe gesture with its alert expression.
- **Purple Star**: Rare and valuable. Stars in its eyes and a celebratory burst on hit make this high-value target feel special.

Faces are universally readable: dot eyes, simple mouths, and clear expressions (worry, happiness, panic) transcend language barriers. High contrast silhouette shapes ensure accessibility for colorblind players.

### Color Palette (from PRD)
| Element | Color | Usage |
|---------|-------|-------|
| Pop Target | Bright Blue (#2196F3) | TG-01 - approachable, friendly |
| Heavy Target | Gold/Amber (#FFC107) | TG-02 - solid, weighty |
| Charge Target | Deep Green (#4CAF50) | TG-03 - energy, growth |
| Trash Target | Alert Red (#F44336) | TG-04 - danger, warning |
| Background | Dark Navy (#0D1B2A) | Contrast for targets |
| UI Accents | Cyan/Teal (#00BCD4) | HUD, highlights |

### Art Style
- **Flat Vector** — clean, modern mobile aesthetic
- **Consistent line weight** across all sprites
- **Slight gradients** for depth (subtle)
- **High contrast** for readability

### Target Visuals

#### TG-01 Pop (Blue Sphere)
- Simple circle with subtle inner glow
- Face: two dots + small smile
- Squash animation on hit
- Particle burst on success

#### TG-02 Heavy (Gold Square)
- Solid square with rounded corners
- Thicker border to suggest weight
- Impact shake on correct double-tap
- Metallic shine effect

#### TG-03 Charge (Green Anchor)
- Anchor shape with energy ring
- Hold-progress indicator (fills over 1.5s)
- Pulsing glow while held
- Release burst on success

#### TG-04 Trash (Red Jagged Line)
- Randomly generated path (segments)
- Dotted line showing drag path
- Trash Bin Zone at bottom (bin icon)
- Line fades after drag completion

### UI Elements
- **Score Display**: Large, monospace-style digits
- **Combo Multiplier**: Orb with rotating glow
- **Lives**: Heart icons (3 max)
- **Pause Button**: Standard || icon
- **Trash Bin Zone**: Trash can icon with drop zone indicator

### FX (Juice)
- Hit pop: Scale bounce + particle burst + SFX
- Miss flash: Screen edge flash + shake
- Combo escalation: Rising pitch + glow intensity
- Level up: Full-screen pulse (if implemented)

## Animation Principles
- **60 FPS target** — all animations must be smooth
- **Quick feedback** — < 100ms from input to visual response
- **Juice, not clutter** — every effect serves feedback
