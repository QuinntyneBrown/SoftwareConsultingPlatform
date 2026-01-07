# Hero Component Specification

## Overview
The Hero component is a large, prominent section typically placed at the top of the homepage. It captures visitor attention and communicates the primary value proposition.

## Dimensions
- **Width**: 100% (full viewport width)
- **Height**: 600px (desktop), 500px (tablet), 400px (mobile)
- **Container max-width**: 1200px
- **Content padding**: 80px (desktop), 60px (tablet), 40px (mobile)

## Layout Structure
```
┌─────────────────────────────────────────────────┐
│                                                 │
│  ┌─────────────────────────────────────────┐   │
│  │                                         │   │
│  │  Headline (H1)                          │   │
│  │  48-60px font size                      │   │
│  │                                         │   │
│  │  Subheadline (P)                        │   │
│  │  18-20px font size                      │   │
│  │                                         │   │
│  │  [Primary Button] [Secondary Button]    │   │
│  │                                         │   │
│  └─────────────────────────────────────────┘   │
│                                                 │
└─────────────────────────────────────────────────┘
```

## Spacing
- **Headline margin-bottom**: 24px
- **Subheadline margin-bottom**: 32px
- **Button gap**: 16px
- **Vertical alignment**: Center (flexbox)
- **Horizontal alignment**: Left or Center

## Typography
- **Headline**: 
  - Font size: 48px (desktop), 36px (tablet), 32px (mobile)
  - Font weight: 700 (Bold)
  - Line height: 1.2
  - Color: #1A1A1A
  
- **Subheadline**: 
  - Font size: 20px (desktop), 18px (tablet), 16px (mobile)
  - Font weight: 400 (Regular)
  - Line height: 1.6
  - Color: #4A4A4A

## Color Codes
- **Background**: #FFFFFF or #F8F9FA
- **Text (Primary)**: #1A1A1A
- **Text (Secondary)**: #4A4A4A
- **Accent (Optional gradient overlay)**: Linear gradient with brand colors

## States
### Default
- Background visible
- Text clearly readable
- Buttons in default state

### Hover (for background videos/images)
- Subtle scale or parallax effect
- No change to content

### Responsive Breakpoints
- **Desktop**: > 1024px
- **Tablet**: 768px - 1024px
- **Mobile**: < 768px

## Accessibility
- Semantic HTML with `<section>` tag
- Proper heading hierarchy (H1 for main headline)
- Sufficient color contrast (WCAG AA minimum)
- Keyboard navigable buttons
- Screen reader friendly text

## Variants
1. **Text Only**: Simple text-based hero with solid background
2. **With Image**: Background image with overlay
3. **With Video**: Background video (muted, autoplay)
4. **Split Layout**: Text on one side, image/graphic on other

## Animation
- **On load**: Fade in with slight upward movement
- **Duration**: 0.6s
- **Easing**: ease-out
- **Delay**: Stagger headline (0s), subheadline (0.1s), buttons (0.2s)

## Implementation Notes
- Use CSS Grid or Flexbox for layout
- Ensure mobile-first responsive design
- Optimize images/videos for web performance
- Consider lazy loading for background media
- Test across different viewport sizes
