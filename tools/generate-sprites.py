#!/usr/bin/env python3
"""
Pully Sprite Generator
Generates 2D sprites for the Pully arcade game.
Theme: "Critters" - cute creatures with expressions.
"""

from PIL import Image, ImageDraw, ImageFont
import math
import os

# Configuration
OUTPUT_DIR = os.path.dirname(os.path.abspath(__file__))
SIZE_TARGET = 128
SIZE_UI = 64
SIZE_FX = 64
PPU = 100  # Pixels per unit (for Unity)

# Color Palette (from DESIGN.md)
COLORS = {
    'green': (76, 175, 80),    # #4CAF50 - Circle normal
    'red': (244, 67, 54),      # #F44336 - Circle trap
    'blue': (33, 150, 243),    # #2196F3 - Square
    'yellow': (255, 235, 59),  # #FFEB3B - Triangle
    'purple': (156, 39, 176),  # #9C27B0 - Star
    'white': (255, 255, 255),
    'black': (0, 0, 0),
    'shadow': (0, 0, 0, 77),   # 30% opacity black
    'ui_bg': (13, 27, 42),    # #0D1B2A - Dark navy
    'ui_accent': (0, 188, 212), # #00BCD4 - Cyan
}

# Eye positions for different expressions
EXPRESSIONS = {
    'idle_open': {'eyes': 'dots', 'mouth': 'small_smile'},
    'idle_blink': {'eyes': 'lines', 'mouth': 'small_smile'},
    'panic': {'eyes': 'x_x', 'mouth': 'worry'},
    'hit': {'eyes': 'stars', 'mouth': 'big_smile'},
    'squash': {'eyes': 'squashed_dots', 'mouth': 'squashed'},
}

def create_image(size, color=(0, 0, 0, 0)):
    """Create a new transparent image."""
    return Image.new('RGBA', (size, size), color)

def add_shadow(draw, shape_fn, offset_x=4, offset_y=4, alpha=77):
    """Add a shadow beneath a shape."""
    # Shift shadow by offset
    shape_fn(offset_x, offset_y, fill=(0, 0, 0, alpha))

def draw_eyes(draw, cx, cy, eye_type, eye_size=8):
    """Draw different eye styles."""
    eye_offset = 15
    
    if eye_type == 'dots':
        # Normal dot eyes
        draw.ellipse([cx-eye_offset-eye_size, cy-eye_size, 
                      cx-eye_offset+eye_size, cy+eye_size], fill=COLORS['black'])
        draw.ellipse([cx+eye_offset-eye_size, cy-eye_size, 
                      cx+eye_offset+eye_size, cy+eye_size], fill=COLORS['black'])
    elif eye_type == 'lines':
        # Blinking eyes (lines)
        draw.line([cx-eye_offset-10, cy, cx-eye_offset+10, cy], fill=COLORS['black'], width=3)
        draw.line([cx+eye_offset-10, cy, cx+eye_offset+10, cy], fill=COLORS['black'], width=3)
    elif eye_type == 'x_x':
        # Panic/shocked eyes (X shapes)
        draw.line([cx-eye_offset-8, cy-8, cx-eye_offset+8, cy+8], fill=COLORS['black'], width=3)
        draw.line([cx-eye_offset-8, cy+8, cx-eye_offset+8, cy-8], fill=COLORS['black'], width=3)
        draw.line([cx+eye_offset-8, cy-8, cx+eye_offset+8, cy+8], fill=COLORS['black'], width=3)
        draw.line([cx+eye_offset-8, cy+8, cx+eye_offset+8, cy-8], fill=COLORS['black'], width=3)
    elif eye_type == 'stars':
        # Star/sparkly eyes
        draw.polygon(star_points(cx-eye_offset, cy, 8, 4), fill=COLORS['black'])
        draw.polygon(star_points(cx+eye_offset, cy, 8, 4), fill=COLORS['black'])

def draw_mouth(draw, cx, cy, mouth_type):
    """Draw different mouth styles."""
    if mouth_type == 'small_smile':
        draw.arc([cx-12, cy+5, cx+12, cy+25], start=0, end=180, fill=COLORS['black'], width=3)
    elif mouth_type == 'big_smile':
        draw.arc([cx-15, cy+5, cx+15, cy+30], start=0, end=180, fill=COLORS['black'], width=4)
    elif mouth_type == 'worry':
        draw.arc([cx-10, cy+15, cx+10, cy+25], start=180, end=360, fill=COLORS['black'], width=3)

def star_points(cx, cy, outer_r, inner_r, points=5):
    """Generate star polygon points."""
    pts = []
    for i in range(points * 2):
        angle = math.pi / 2 + i * math.pi / points
        radius = outer_r if i % 2 == 0 else inner_r
        x = cx + radius * math.cos(angle)
        y = cy + radius * math.sin(angle)
        pts.append((x, y))
    return pts

def generate_circle(base_color, expression, size=SIZE_TARGET):
    """Generate a circle critter sprite."""
    img = create_image(size)
    draw = ImageDraw.Draw(img)
    cx, cy = size // 2, size // 2
    radius = size // 2 - 10
    
    # Shadow
    shadow_offset = 6
    draw.ellipse([cx-radius+shadow_offset, cy-radius+shadow_offset, 
                  cx+radius+shadow_offset, cy+radius+shadow_offset], 
                 fill=COLORS['shadow'])
    
    # Main body (gradient effect via overlay)
    draw.ellipse([cx-radius, cy-radius, cx+radius, cy+radius], fill=base_color)
    
    # Inner highlight (subtle gradient)
    h_radius = radius - 15
    highlight_color = tuple(min(255, int(c * 1.3)) for c in base_color[:3]) + (100,)
    draw.ellipse([cx-h_radius//2, cy-h_radius, cx+h_radius//2, cy], fill=highlight_color)
    
    # Draw face based on expression
    if expression == 'idle_open':
        draw_eyes(draw, cx, cy-8, 'dots')
        draw_mouth(draw, cx, cy+2, 'small_smile')
    elif expression == 'idle_blink':
        draw_eyes(draw, cx, cy-8, 'lines')
        draw_mouth(draw, cx, cy+2, 'small_smile')
    elif expression == 'panic':
        draw_eyes(draw, cx, cy-8, 'x_x')
        draw_mouth(draw, cx, cy+8, 'worry')
        # Sweat drops
        draw.ellipse([cx+radius-25, cy-radius+15, cx+radius-15, cy-radius+35], 
                     fill=(100, 200, 255, 200))
    elif expression == 'hit':
        draw_eyes(draw, cx, cy-8, 'stars')
        draw_mouth(draw, cx, cy+2, 'big_smile')
    elif expression == 'squash':
        img = create_image(size)
        draw = ImageDraw.Draw(img)
        # Squashed ellipse
        draw.ellipse([cx-radius-10, cy-radius//2+15, cx+radius+10, cy+radius-5], fill=base_color)
        draw_eyes(draw, cx, cy-8, 'dots')
        draw_mouth(draw, cx, cy+5, 'small_smile')
    
    return img

def generate_square(base_color, expression, size=SIZE_TARGET):
    """Generate a square critter sprite."""
    img = create_image(size)
    draw = ImageDraw.Draw(img)
    cx, cy = size // 2, size // 2
    half = size // 2 - 10
    corner_radius = 15
    
    # Shadow
    shadow_offset = 6
    draw.rounded_rectangle([cx-half+shadow_offset, cy-half+shadow_offset,
                            cx+half+shadow_offset, cy+half+shadow_offset], 
                           radius=corner_radius, fill=COLORS['shadow'])
    
    # Main body
    draw.rounded_rectangle([cx-half, cy-half, cx+half, cy+half], 
                           radius=corner_radius, fill=base_color)
    
    # Border (shows weight/heaviness)
    border_color = tuple(int(c * 0.7) for c in base_color[:3])
    draw.rounded_rectangle([cx-half, cy-half, cx+half, cy+half], 
                           radius=corner_radius, outline=border_color, width=4)
    
    # Metallic shine effect (diagonal gradient strip)
    shine_color = (255, 255, 255, 60)
    for i in range(10):
        draw.line([(cx-half+10+i, cy-half+5+i), 
                   (cx-half+30+i, cy-half+5+i)], fill=shine_color, width=1)
    
    # Draw face based on expression
    if expression == 'idle_open':
        draw_eyes(draw, cx, cy-8, 'dots')
        draw_mouth(draw, cx, cy+5, 'small_smile')
    elif expression == 'idle_blink':
        draw_eyes(draw, cx, cy-8, 'lines')
        draw_mouth(draw, cx, cy+5, 'small_smile')
    elif expression == 'panic':
        draw_eyes(draw, cx, cy-8, 'x_x')
        draw_mouth(draw, cx, cy+10, 'worry')
        # Cracks
        draw.line([cx-half+15, cy-half+20, cx-half+35, cy-half+40], fill=COLORS['black'], width=2)
        draw.line([cx+half-20, cy-half+35, cx+half-30, cy-half+50], fill=COLORS['black'], width=2)
    elif expression == 'hit':
        draw_eyes(draw, cx, cy-8, 'stars')
        draw_mouth(draw, cx, cy+5, 'big_smile')
    elif expression == 'squash':
        img = create_image(size)
        draw = ImageDraw.Draw(img)
        # Squashed rectangle
        draw.rounded_rectangle([cx-half-15, cy-10, cx+half+15, cy+half-5],
                               radius=corner_radius, fill=base_color)
        draw_eyes(draw, cx, cy-5, 'dots')
        draw_mouth(draw, cx, cy+10, 'small_smile')
    
    return img

def generate_triangle(base_color, expression, size=SIZE_TARGET):
    """Generate a triangle critter sprite."""
    img = create_image(size)
    draw = ImageDraw.Draw(img)
    cx, cy = size // 2, size // 2
    height = size - 20
    width = size - 20
    
    # Triangle points (pointing up)
    points = [
        (cx, cy - height // 2 + 10),  # Top
        (cx - width // 2 + 10, cy + height // 2 - 5),  # Bottom left
        (cx + width // 2 - 10, cy + height // 2 - 5),  # Bottom right
    ]
    
    # Shadow
    shadow_offset = 6
    shadow_points = [(p[0]+shadow_offset, p[1]+shadow_offset) for p in points]
    draw.polygon(shadow_points, fill=COLORS['shadow'])
    
    # Main body
    draw.polygon(points, fill=base_color)
    
    # Border
    border_color = tuple(int(c * 0.8) for c in base_color[:3])
    draw.line(points + [points[0]], fill=border_color, width=4)
    
    # Draw face
    if expression == 'idle_open':
        draw_eyes(draw, cx, cy-5, 'dots')
        draw_mouth(draw, cx, cy+15, 'small_smile')
    elif expression == 'idle_blink':
        draw_eyes(draw, cx, cy-5, 'lines')
        draw_mouth(draw, cx, cy+15, 'small_smile')
    elif expression == 'panic':
        draw_eyes(draw, cx, cy-5, 'x_x')
        draw_mouth(draw, cx, cy+20, 'worry')
        # Vibration lines
        draw.line([cx-30, cy-30, cx-25, cy-25], fill=COLORS['black'], width=2)
        draw.line([cx+30, cy-30, cx+35, cy-25], fill=COLORS['black'], width=2)
    elif expression == 'hit':
        draw_eyes(draw, cx, cy-5, 'stars')
        draw_mouth(draw, cx, cy+15, 'big_smile')
    elif expression == 'squash':
        img = create_image(size)
        draw = ImageDraw.Draw(img)
        # Squashed triangle (shortened)
        short_points = [
            (cx, cy - height // 4 + 10),
            (cx - width // 2 - 5, cy + height // 2 - 25),
            (cx + width // 2 + 5, cy + height // 2 - 25),
        ]
        draw.polygon(short_points, fill=base_color)
        draw_eyes(draw, cx, cy, 'dots')
        draw_mouth(draw, cx, cy+15, 'small_smile')
    
    return img

def generate_star(base_color, expression, size=SIZE_TARGET):
    """Generate a star critter sprite."""
    img = create_image(size)
    draw = ImageDraw.Draw(img)
    cx, cy = size // 2, size // 2
    
    # Star points
    outer_r = size // 2 - 10
    inner_r = size // 4
    points = star_points(cx, cy, outer_r, inner_r, 5)
    
    # Shadow
    shadow_points = [(p[0]+6, p[1]+6) for p in points]
    draw.polygon(shadow_points, fill=COLORS['shadow'])
    
    # Main body
    draw.polygon(points, fill=base_color)
    
    # Border
    border_color = tuple(int(c * 0.8) for c in base_color[:3])
    draw.polygon(points, outline=border_color, width=3)
    
    # Sparkle effect in body
    sparkle_color = (255, 255, 255, 100)
    draw.ellipse([cx-5, cy-outer_r+15, cx+5, cy-outer_r+25], fill=sparkle_color)
    
    # Draw face
    if expression == 'idle_open':
        draw_eyes(draw, cx, cy-3, 'dots')
        draw_mouth(draw, cx, cy+8, 'small_smile')
    elif expression == 'idle_blink':
        draw_eyes(draw, cx, cy-3, 'lines')
        draw_mouth(draw, cx, cy+8, 'small_smile')
    elif expression == 'panic':
        draw_eyes(draw, cx, cy-3, 'x_x')
        draw_mouth(draw, cx, cy+15, 'worry')
    elif expression == 'hit':
        draw_eyes(draw, cx, cy-3, 'stars')
        draw_mouth(draw, cx, cy+8, 'big_smile')
        # Rainbow burst around star
        for i in range(8):
            angle = i * math.pi / 4
            x1 = cx + (outer_r + 5) * math.cos(angle)
            y1 = cy + (outer_r + 5) * math.sin(angle)
            x2 = cx + (outer_r + 15) * math.cos(angle)
            y2 = cy + (outer_r + 15) * math.sin(angle)
            draw.line([(x1, y1), (x2, y2)], fill=COLORS['white'], width=3)
    elif expression == 'squash':
        # Star squashes to a circle (transform)
        img = create_image(size)
        draw = ImageDraw.Draw(img)
        draw.ellipse([cx-outer_r-5, cy-10, cx+outer_r+5, cy+25], fill=base_color)
        draw_eyes(draw, cx, cy-3, 'dots')
        draw_mouth(draw, cx, cy+8, 'small_smile')
    
    return img

def generate_ui_button(label="PLAY", size=256):
    """Generate a UI button sprite."""
    img = create_image(size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    
    # Button shape
    rect_size = size - 20
    corner_radius = 20
    cx = size // 2
    
    # Shadow
    draw.rounded_rectangle([20, 20, 20+rect_size, 20+rect_size],
                           radius=corner_radius, fill=COLORS['shadow'])
    
    # Main button
    draw.rounded_rectangle([10, 10, 10+rect_size, 10+rect_size],
                           radius=corner_radius, fill=COLORS['ui_accent'])
    
    # Border
    border_color = tuple(int(c * 1.2) for c in COLORS['ui_accent'][:3])
    draw.rounded_rectangle([10, 10, 10+rect_size, 10+rect_size],
                           radius=corner_radius, outline=border_color, width=4)
    
    return img

def generate_ui_icon_hearts(full=True, size=64):
    """Generate heart icon for lives."""
    img = create_image(size)
    draw = ImageDraw.Draw(img)
    cx, cy = size // 2, size // 2
    
    # Heart shape (two circles + triangle)
    color = COLORS['red'] if full else COLORS['white']
    if not full:
        # Empty heart outline
        draw.ellipse([cx-16, cy-16, cx-4, cy-4], outline=COLORS['red'], width=3)
        draw.ellipse([cx+4, cy-16, cx+16, cy-4], outline=COLORS['red'], width=3)
        draw.polygon([(cx, cy+8), (cx-16-5, cy-4), (cx+16+5, cy-8)], outline=COLORS['red'], width=3)
    else:
        # Filled heart
        draw.ellipse([cx-16, cy-16, cx-4, cy-4], fill=COLORS['red'])
        draw.ellipse([cx+4, cy-16, cx+16, cy-4], fill=COLORS['red'])
        draw.polygon([(cx, cy+10), (cx-20, cy-6), (cx+20, cy-6)], fill=COLORS['red'])
    
    return img

def generate_ui_frame(size=512):
    """Generate a UI frame/panel."""
    img = create_image(size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    
    # Panel background
    corner_radius = 25
    border_width = 4
    
    # Semi-transparent background
    bg_color = (13, 27, 42, 220)
    draw.rounded_rectangle([10, 10, size-10, size-10],
                           radius=corner_radius, fill=bg_color)
    
    # Border
    draw.rounded_rectangle([10, 10, size-10, size-10],
                           radius=corner_radius, outline=COLORS['ui_accent'], width=border_width)
    
    return img

def generate_fx_particle(type='burst', size=64):
    """Generate FX particle sprites."""
    img = create_image(size)
    draw = ImageDraw.Draw(img)
    cx, cy = size // 2, size // 2
    
    if type == 'burst':
        # Circular burst
        draw.ellipse([10, 10, size-10, size-10], fill=COLORS['white'])
        # Radial lines
        for i in range(4):
            angle = i * math.pi / 2
            x1 = cx + 15 * math.cos(angle)
            y1 = cy + 15 * math.sin(angle)
            x2 = cx + 25 * math.cos(angle)
            y2 = cy + 25 * math.sin(angle)
            draw.line([(x1, y1), (x2, y2)], fill=COLORS['white'], width=4)
    elif type == 'star':
        pts = star_points(cx, cy, 20, 8, 4)
        draw.polygon(pts, fill=COLORS['yellow'])
    elif type == 'glow':
        # Radial gradient (simplified)
        for r in range(size//2, 0, -2):
            alpha = int(255 * (1 - r / (size//2)))
            color = (255, 255, 255, alpha)
            draw.ellipse([cx-r, cy-r, cx+r, cy+r], fill=color)
    
    return img

def save_sprite(img, filename, output_dir):
    """Save a generated sprite to file."""
    filepath = os.path.join(output_dir, filename)
    img.save(filepath, 'PNG')
    print(f"  Generated: {filename}")

def main():
    print("=" * 60)
    print("PULLY SPRITE GENERATOR")
    print("Theme: Critters - Cute Creatures")
    print("=" * 60)
    
    # Create output directories
    base_dir = os.path.dirname(os.path.abspath(__file__))
    targets_dir = os.path.join(base_dir, 'Targets')
    ui_dir = os.path.join(base_dir, 'UI')
    fx_dir = os.path.join(base_dir, 'FX')
    
    os.makedirs(targets_dir, exist_ok=True)
    os.makedirs(ui_dir, exist_ok=True)
    os.makedirs(fx_dir, exist_ok=True)
    
    total_generated = 0
    
    # Generate Target Sprites (Critters)
    print("\n--- GENERATING TARGET SPRITES (Critters) ---")
    
    shapes_config = [
        ('circle', 'green', COLORS['green'], ['idle_open', 'idle_blink', 'panic', 'hit', 'squash']),
        ('circle', 'red', COLORS['red'], ['idle_open', 'idle_blink', 'panic', 'hit', 'squash']),
        ('square', 'blue', COLORS['blue'], ['idle_open', 'idle_blink', 'panic', 'hit', 'squash']),
        ('triangle', 'yellow', COLORS['yellow'], ['idle_open', 'idle_blink', 'panic', 'hit', 'squash']),
        ('star', 'purple', COLORS['purple'], ['idle_open', 'idle_blink', 'panic', 'hit', 'squash']),
    ]
    
    generators = {
        'circle': generate_circle,
        'square': generate_square,
        'triangle': generate_triangle,
        'star': generate_star,
    }
    
    for shape, color_name, color_val, expressions in shapes_config:
        print(f"\nGenerating {color_name} {shape}...")
        for expr in expressions:
            if expr == 'idle_open' or expr == 'idle_blink':
                variant = 'idle_01' if expr == 'idle_open' else 'idle_02'
            else:
                variant = expr
            filename = f"{shape}_{color_name}_{variant}.png"
            sprite = generators[shape](color_val, expr)
            save_sprite(sprite, filename, targets_dir)
            total_generated += 1
    
    # Generate UI Sprites
    print("\n--- GENERATING UI SPRITES ---")
    
    print("\nGenerating buttons...")
    save_sprite(generate_ui_button("PLAY"), "button_play.png", ui_dir)
    save_sprite(generate_ui_button("SETTINGS"), "button_settings.png", ui_dir)
    total_generated += 2
    
    print("\nGenerating icons...")
    save_sprite(generate_ui_icon_hearts(True), "icon_heart_full.png", ui_dir)
    save_sprite(generate_ui_icon_hearts(False), "icon_heart_empty.png", ui_dir)
    save_sprite(generate_ui_frame(512), "frame_menu.png", ui_dir)
    total_generated += 3
    
    # Generate FX Sprites
    print("\n--- GENERATING FX SPRITES ---")
    
    print("\nGenerating particles...")
    save_sprite(generate_fx_particle('burst'), "particle_hit.png", fx_dir)
    save_sprite(generate_fx_particle('star'), "particle_star.png", fx_dir)
    save_sprite(generate_fx_particle('glow'), "flash_hit.png", fx_dir)
    total_generated += 3
    
    print("\n" + "=" * 60)
    print(f"SPRITE GENERATION COMPLETE")
    print(f"Total sprites generated: {total_generated}")
    print("=" * 60)
    print(f"\nAssets created in:")
    print(f"  Targets/: {len([f for f in os.listdir(targets_dir) if f.endswith('.png')])} files")
    print(f"  UI/: {len([f for f in os.listdir(ui_dir) if f.endswith('.png')])} files")
    print(f"  FX/: {len([f for f in os.listdir(fx_dir) if f.endswith('.png')])} files")
    print("\nNext steps:")
    print("  1. Import into Unity")
    print("  2. Configure PPU = 100, Filter = Bilinear")
    print("  3. Create Sprite Atlas")
    print("  4. Update DESIGN.md with final rationale")

if __name__ == '__main__':
    main()
