# Navigation Component Specification

## Overview
The Navigation component is the main site-wide menu bar, typically positioned at the top of all pages. It provides primary navigation links and branding.

## Dimensions
- **Width**: 100% (full viewport width)
- **Height**: 80px (desktop), 70px (tablet), 60px (mobile)
- **Container max-width**: 1200px
- **Logo area**: 150px × 40px
- **Horizontal padding**: 40px (desktop), 20px (mobile)

## Layout Structure
```
┌─────────────────────────────────────────────────┐
│  [Logo]           [Nav Links]      [CTA Button] │
│                                                 │
└─────────────────────────────────────────────────┘
```

## Spacing
- **Logo margin-right**: auto
- **Nav links gap**: 32px (desktop), 24px (tablet)
- **CTA button margin-left**: 24px
- **Vertical padding**: 20px
- **Horizontal padding**: 40px (desktop), 20px (mobile)

## Typography
- **Nav Links**: 
  - Font size: 16px
  - Font weight: 500 (Medium)
  - Line height: 1.5
  - Color: #2D3748
  - Letter spacing: 0.3px
  
- **CTA Button**: 
  - Font size: 15px
  - Font weight: 600 (Semi-bold)

## Color Codes
- **Background**: #FFFFFF
- **Border Bottom**: #E2E8F0 (1px solid)
- **Link Default**: #2D3748
- **Link Hover**: #667eea
- **Link Active**: #667eea
- **CTA Button Background**: #667eea
- **CTA Button Text**: #FFFFFF
- **Mobile Menu Background**: #FFFFFF
- **Mobile Menu Overlay**: rgba(0, 0, 0, 0.5)

## States

### Desktop Navigation
#### Default
- Links displayed horizontally
- Normal link colors
- CTA button visible

#### Link Hover
- Color transitions to brand color
- Underline appears (2px, animated)
- Transition duration: 0.2s

#### Scrolled State
- Add box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1)
- Optional: Reduce height to 70px
- Background remains solid

### Mobile Navigation
#### Hamburger Menu
- Displayed at < 768px
- Icon: 24px × 24px
- Three horizontal lines (3px height, 20px width, 5px gap)
- Animated to X when open

#### Mobile Menu Panel
- Full-screen overlay
- Slide in from right
- Animation duration: 0.3s
- Links stacked vertically (48px height each)

## Responsive Breakpoints
- **Desktop**: > 1024px (horizontal layout)
- **Tablet**: 768px - 1024px (horizontal, condensed spacing)
- **Mobile**: < 768px (hamburger menu)

## Accessibility
- Semantic HTML with `<nav>` tag
- ARIA labels for mobile menu toggle
- Keyboard navigation support (Tab, Enter, Esc)
- Focus visible styles
- Skip to main content link
- Current page indicated with aria-current="page"

## Sticky Behavior
- Position: sticky or fixed
- Top: 0
- Z-index: 1000
- Smooth show/hide on scroll (optional)

## Components Breakdown

### Logo
- SVG or image format
- Height: 32-40px
- Linked to homepage
- Alt text: "Company Name"

### Navigation Links
- Typical items: Home, Services, Case Studies, About, Blog, Contact
- Active state indication
- Dropdown support (optional for sub-menus)

### CTA Button
- "Get Started" or "Contact Us"
- Prominent positioning
- Sticky in mobile menu

## Animation
- **Link hover**: Color transition 0.2s, underline slide-in
- **Mobile menu**: Slide-in from right 0.3s ease-out
- **Hamburger to X**: Transform 0.3s ease
- **Scroll appearance**: Fade-in box-shadow 0.2s

## Variants
1. **Transparent**: Background transparent over hero (becomes solid on scroll)
2. **Solid**: Always solid background
3. **With dropdown**: Sub-menus for complex navigation
4. **Centered**: Logo and links centered (less common for consulting sites)

## Implementation Notes
- Use flexbox for alignment
- Consider CSS position: sticky for scroll behavior
- Ensure z-index hierarchy for dropdowns and mobile menu
- Test touch targets (minimum 44px × 44px for mobile)
- Optimize for performance (avoid layout shifts)
- Consider mega-menu for extensive service offerings
