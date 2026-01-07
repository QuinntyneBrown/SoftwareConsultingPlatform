# Card Component Specification

## Overview
The Card component is a flexible container used to display case studies, services, blog posts, and other content in a structured, visually appealing format.

## Dimensions
- **Width**: Flexible (typically 1/3 or 1/2 of container in grid)
- **Height**: Auto (content-driven) or fixed (360px for uniform grids)
- **Minimum width**: 280px
- **Maximum width**: 400px (in multi-column layouts)
- **Border radius**: 12px
- **Padding**: 24px (internal content padding)

## Layout Structure
```
┌──────────────────────────────────┐
│                                  │
│  ┌────────────────────────────┐  │
│  │                            │  │
│  │    Image / Thumbnail       │  │
│  │    (16:9 or 4:3 ratio)     │  │
│  │                            │  │
│  └────────────────────────────┘  │
│                                  │
│  Category / Tag                  │
│                                  │
│  Headline (H3)                   │
│  Multi-line truncated            │
│                                  │
│  Description text...             │
│  Truncated to 2-3 lines          │
│                                  │
│  [Read More →]                   │
│                                  │
└──────────────────────────────────┘
```

## Spacing
- **Image margin-bottom**: 20px
- **Category margin-bottom**: 12px
- **Headline margin-bottom**: 12px
- **Description margin-bottom**: 20px
- **Card padding**: 24px (for content below image)
- **Card gap in grid**: 24px (desktop), 16px (mobile)

## Typography
- **Category/Tag**: 
  - Font size: 12px
  - Font weight: 600 (Semi-bold)
  - Text transform: uppercase
  - Letter spacing: 1px
  - Color: #667eea
  
- **Headline**: 
  - Font size: 20px (desktop), 18px (mobile)
  - Font weight: 600 (Semi-bold)
  - Line height: 1.3
  - Color: #1A1A1A
  
- **Description**: 
  - Font size: 15px
  - Font weight: 400 (Regular)
  - Line height: 1.6
  - Color: #4A5568
  
- **Link/CTA**: 
  - Font size: 14px
  - Font weight: 600
  - Color: #667eea

## Color Codes
- **Background**: #FFFFFF
- **Border**: #E2E8F0 (1px solid) or none
- **Shadow (default)**: 0 2px 8px rgba(0, 0, 0, 0.08)
- **Shadow (hover)**: 0 8px 24px rgba(0, 0, 0, 0.12)
- **Category background**: #EEF2FF
- **Category text**: #667eea
- **Image overlay**: Linear gradient (0deg, rgba(0,0,0,0.4) 0%, transparent 50%)

## States

### Default
- Subtle shadow
- Normal colors
- Image at 100% opacity

### Hover
- Shadow elevation increases
- Card lifts slightly (translateY(-4px))
- Image scales slightly (scale: 1.05)
- Headline color changes to brand color
- Transition duration: 0.3s ease

### Active/Pressed
- Shadow reduces
- Card moves down (translateY(0))
- Brief transition

## Image Specifications
- **Aspect ratio**: 16:9 (recommended) or 4:3
- **Dimensions**: 400px × 225px (16:9) or 400px × 300px (4:3)
- **Object-fit**: cover
- **Background**: #E2E8F0 (placeholder)
- **Border radius (top)**: 12px 12px 0 0

## Responsive Breakpoints
- **Desktop**: > 1024px (3 columns)
- **Tablet**: 768px - 1024px (2 columns)
- **Mobile**: < 768px (1 column, full width)

## Accessibility
- Semantic HTML with `<article>` tag
- Proper heading hierarchy (H3 for card titles)
- Meaningful alt text for images
- Keyboard focusable (entire card as link or button inside)
- Focus visible state
- Sufficient color contrast
- ARIA labels where necessary

## Variants

### 1. Image Card (Default)
- Image at top
- Content below
- Standard for case studies, blog posts

### 2. Icon Card
- Icon instead of image
- Used for service descriptions
- Icon size: 48px × 48px
- Icon color: Brand color

### 3. Horizontal Card
- Image on left (30-40% width)
- Content on right
- Used for featured items, lists

### 4. Overlay Card
- Content overlaid on image
- Text with background overlay for readability
- Used for visual-heavy content

### 5. Minimal Card
- No border/shadow
- Flat design
- Hover shows border or background

## Animation
- **On appear**: Fade in with slight upward movement
- **On hover**: 
  - Card: translateY(-4px), shadow increase, 0.3s ease
  - Image: scale(1.05), 0.3s ease
  - Headline: color transition, 0.2s ease
- **Stagger effect**: When multiple cards load (0.1s delay between each)

## Grid Layout
```css
display: grid;
grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
gap: 24px;
```

## Implementation Notes
- Use CSS Grid or Flexbox for card layout
- Overflow hidden for image scale effect
- Text truncation with line-clamp for description
- Consider lazy loading images
- Ensure clickable area is entire card (use positioned overlay)
- Test touch interactions on mobile
- Optimize images for web (WebP format recommended)
- Use skeleton loaders for loading states

## Content Guidelines
- **Headline**: 40-60 characters recommended
- **Description**: 80-120 characters (2-3 lines)
- **Image**: High quality, relevant to content
- **Category**: Single word or short phrase
