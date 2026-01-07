# Service Listing Component Specification

## Overview
The Service Listing component displays the consulting services offered, typically on a services page or homepage. It presents each service clearly with icons, descriptions, and call-to-action elements.

## Dimensions
- **Container width**: 100% (max-width: 1200px)
- **Item width**: Flexible (typically 1/2 or 1/3 of container)
- **Item height**: Auto (content-driven)
- **Padding**: 40px (per service item)
- **Icon size**: 48px × 48px (standard), 64px × 64px (large)
- **Border radius**: 12px

## Layout Structure
```
┌──────────────────────────────────────────────────┐
│                                                  │
│  ┌─────────┐  ┌─────────┐  ┌─────────┐         │
│  │         │  │         │  │         │         │
│  │  [Icon] │  │  [Icon] │  │  [Icon] │         │
│  │  Title  │  │  Title  │  │  Title  │         │
│  │  Desc   │  │  Desc   │  │  Desc   │         │
│  │  [Link] │  │  [Link] │  │  [Link] │         │
│  │         │  │         │  │         │         │
│  └─────────┘  └─────────┘  └─────────┘         │
│                                                  │
└──────────────────────────────────────────────────┘
```

## Spacing
- **Section padding**: 80px (top/bottom), 40px (sides)
- **Item padding**: 40px (internal)
- **Grid gap**: 32px (desktop), 24px (mobile)
- **Icon margin-bottom**: 24px
- **Title margin-bottom**: 16px
- **Description margin-bottom**: 24px
- **Feature list item gap**: 12px

## Typography

### Service Title
- **Font size**: 24px (desktop), 22px (mobile)
- **Font weight**: 600 (Semi-bold)
- **Line height**: 1.3
- **Color**: #1A1A1A

### Service Description
- **Font size**: 16px
- **Font weight**: 400 (Regular)
- **Line height**: 1.6
- **Color**: #4A5568

### Feature List
- **Font size**: 15px
- **Font weight**: 400
- **Line height**: 1.8
- **Color**: #2D3748

### Link/CTA
- **Font size**: 15px
- **Font weight**: 600 (Semi-bold)
- **Color**: #667eea

## Color Codes
- **Background (item)**: #FFFFFF or #F7FAFC
- **Icon background**: Linear gradient (#667eea to #764ba2) or solid brand color
- **Icon color**: #FFFFFF
- **Title**: #1A1A1A
- **Description**: #4A5568
- **Feature text**: #2D3748
- **Link**: #667eea
- **Link hover**: #5568d3
- **Border**: 1px solid #E2E8F0 (optional)
- **Shadow**: 0 2px 8px rgba(0, 0, 0, 0.08)

## Layout Variants

### 1. Grid Layout (Default)
- 3 columns (desktop)
- 2 columns (tablet)
- 1 column (mobile)
- Equal height cards

### 2. List Layout
- Full-width items
- Alternating layout (icon left/right)
- Stacked vertically

### 3. Minimal List
- Icon + title + description inline
- No cards/backgrounds
- Simple, clean design

## Service Item Components

### Icon
- Size: 48-64px
- Background: Circular or rounded square
- Color: Brand color or gradient
- Position: Top-center or left

### Title
- Clear, concise service name
- 2-5 words recommended
- Prominent typography

### Description
- Brief overview (50-100 words)
- Key benefits or approach
- Clear and accessible language

### Feature List (Optional)
- Bulleted or checkmark list
- 3-5 key features/deliverables
- Brief, scannable items

### CTA Link/Button
- "Learn More"
- "View Details"
- "Get Started"
- Links to service detail page

## States

### Default
- Standard colors
- Subtle shadow
- Static icon

### Hover
- Shadow increases
- Slight lift (translateY(-4px))
- Icon may scale or animate
- Link underline appears
- Transition: 0.3s ease

### Active/Focus
- Border highlight
- Focus outline for accessibility

## Icon Styles

### SVG Icons
- Custom or icon library (Heroicons, Feather, etc.)
- Consistent style across all services
- Single color or duotone

### Icon Background
- Circular: border-radius: 50%
- Rounded square: border-radius: 12px
- Gradient background
- Shadow: 0 4px 12px rgba(102, 126, 234, 0.2)

## Common Services (Examples)
1. MVP Development
2. Product Strategy & Consulting
3. UI/UX Design
4. Full-Stack Engineering
5. Mobile App Development
6. Quality Assurance & Testing
7. DevOps & Cloud Infrastructure
8. Staff Augmentation
9. Technical Consulting
10. Maintenance & Support

## Responsive Breakpoints
- **Desktop**: > 1024px (3 columns)
- **Tablet**: 768px - 1024px (2 columns)
- **Mobile**: < 768px (1 column)

## Accessibility
- Semantic HTML (`<section>`, `<article>`)
- Proper heading hierarchy (H2 for titles)
- Alt text for icons
- Keyboard navigable links
- Focus visible states
- Sufficient color contrast
- ARIA labels where needed

## Animation
- **On scroll into view**: Fade in with upward movement
- **Stagger effect**: 0.1s delay between items
- **Hover**: Scale icon slightly, lift card
- **Duration**: 0.3-0.5s ease-out

## Feature List Styling
- Checkmark icon: ✓ (green) or brand color
- Bullet alternative: Custom icon
- Spacing: 12px between items
- Icon size: 16px × 16px

## Implementation Notes
- Use CSS Grid for layout
- Icon fonts or inline SVG
- Lazy load if many services
- Consider hover effects carefully
- Ensure consistent heights (flexbox or grid)
- Link entire card (optional) or just CTA

## Content Guidelines
- **Title**: Clear, specific service name
- **Description**: Focus on benefits, not just features
- **Features**: Concrete deliverables
- **Length**: Keep description to 1-2 paragraphs
- **Tone**: Professional but approachable

## Grid Code Example
```css
.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 32px;
}
```

## Variants by Design Approach

### 1. Card-Based
- White background
- Shadow/border
- Contained design
- Most common

### 2. Flat/Minimal
- No background
- Simple borders or dividers
- More whitespace
- Modern aesthetic

### 3. Icon-Focused
- Large icons (64-80px)
- Icon as primary element
- Minimal text
- Visual hierarchy

### 4. Detailed
- Longer descriptions
- Multiple feature lists
- Pricing (optional)
- More comprehensive

## Call-to-Action Patterns
- Text link with arrow →
- Ghost button
- Primary button (less common for list items)
- "Learn More" most common text

## Best Practices
- Consistent card heights
- Balance visual weight of icons
- Keep descriptions concise
- Use action-oriented language
- Provide clear next steps
- Maintain visual hierarchy
- Test with real content
- Ensure mobile responsiveness
