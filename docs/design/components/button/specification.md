# Button Component Specification

## Overview
The Button component is a fundamental UI element used for user actions throughout the site. It comes in multiple variants for different contexts and hierarchies.

## Dimensions
- **Height**: 48px (large), 40px (medium), 32px (small)
- **Horizontal padding**: 32px (large), 24px (medium), 16px (small)
- **Border radius**: 8px (large), 6px (medium), 4px (small)
- **Border width**: 2px (for outlined variants)
- **Minimum width**: 120px (except icon-only buttons)
- **Icon size**: 20px (for buttons with icons)
- **Icon spacing**: 8px (gap between icon and text)

## Layout Structure
```
┌──────────────────────┐
│  [Icon] Button Text  │
└──────────────────────┘

or

┌──────────────────────┐
│  Button Text [Icon]  │
└──────────────────────┘
```

## Spacing
- **Padding**: 
  - Large: 16px 32px
  - Medium: 12px 24px
  - Small: 8px 16px
- **Gap between buttons**: 16px (horizontal), 12px (vertical when stacked)
- **Icon-text gap**: 8px

## Typography
- **Font size**: 
  - Large: 16px
  - Medium: 15px
  - Small: 14px
- **Font weight**: 600 (Semi-bold)
- **Letter spacing**: 0.3px
- **Text transform**: None (sentence case)

## Color Codes

### Primary Button
- **Background**: #667eea
- **Text**: #FFFFFF
- **Border**: none
- **Hover Background**: #5568d3
- **Active Background**: #4c5fc7
- **Disabled Background**: #E2E8F0
- **Disabled Text**: #A0AEC0

### Secondary Button
- **Background**: #FFFFFF
- **Text**: #667eea
- **Border**: 2px solid #667eea
- **Hover Background**: #EEF2FF
- **Hover Border**: #5568d3
- **Active Background**: #E0E7FF
- **Disabled Background**: #F7FAFC
- **Disabled Text**: #CBD5E0
- **Disabled Border**: #E2E8F0

### Tertiary/Ghost Button
- **Background**: transparent
- **Text**: #667eea
- **Border**: none
- **Hover Background**: #EEF2FF
- **Active Background**: #E0E7FF
- **Disabled Text**: #CBD5E0

### Danger Button
- **Background**: #F56565
- **Text**: #FFFFFF
- **Border**: none
- **Hover Background**: #E53E3E
- **Active Background**: #C53030
- **Disabled Background**: #FED7D7
- **Disabled Text**: #FC8181

### Success Button
- **Background**: #48BB78
- **Text**: #FFFFFF
- **Border**: none
- **Hover Background**: #38A169
- **Active Background**: #2F855A

## States

### Default
- Normal colors as specified
- Cursor: pointer
- Transition: all 0.2s ease

### Hover
- Background color darkens/changes
- Slight elevation (shadow increase)
- Transform: translateY(-2px)
- Box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3)
- Transition duration: 0.2s

### Active/Pressed
- Background color darkens further
- Transform: translateY(0) or scale(0.98)
- Shadow reduces
- Transition duration: 0.1s

### Focus
- Outline: 2px solid #667eea with 2px offset
- Or box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.3)
- Maintains hover state if also hovered

### Disabled
- Cursor: not-allowed
- Opacity: 0.6
- No hover effects
- Muted colors

### Loading
- Cursor: wait
- Display loading spinner
- Text optional or "Loading..."
- Disabled interaction

## Button Variants

### 1. Primary
- Most prominent actions
- "Get Started", "Submit", "Save"
- Solid background with brand color
- Maximum one per section

### 2. Secondary
- Alternative actions
- Less prominent than primary
- Outlined style
- Can have multiple in view

### 3. Tertiary/Ghost
- Least prominent actions
- "Cancel", "Learn More"
- Minimal visual weight
- Transparent background

### 4. Icon Only
- 40px × 40px (square)
- Single icon, no text
- Circular or square
- Tooltips required

### 5. Icon + Text
- Icon on left or right
- Enhances meaning
- Common for "Download", "Share", etc.

### 6. Full Width
- Width: 100%
- Common in mobile layouts
- Forms and modals

### 7. Button Group
- Multiple buttons connected
- Shared borders
- First and last have rounded corners

## Responsive Behavior
- **Desktop**: Standard sizes
- **Tablet**: Slightly reduce padding
- **Mobile**: 
  - Consider full-width for primary actions
  - Increase touch target to minimum 44px height
  - Stack vertically with 12px gap

## Accessibility
- Semantic `<button>` or `<a>` tag (use button for actions, anchor for navigation)
- Keyboard accessible (Tab, Enter, Space)
- Focus visible state
- ARIA labels for icon-only buttons
- Disabled state prevents interaction
- Sufficient color contrast (WCAG AA: 4.5:1 minimum)
- Loading state announced to screen readers

## Animation
- **Hover**: 
  - Transform: translateY(-2px)
  - Box-shadow increase
  - Duration: 0.2s ease
- **Active**: 
  - Transform: scale(0.98)
  - Duration: 0.1s ease
- **Loading**: 
  - Spinner rotation: 1s linear infinite

## Usage Guidelines
- Use primary buttons sparingly (1-2 per view)
- Button text should be action-oriented ("Start Project", not just "Click Here")
- Keep text concise (1-3 words)
- Ensure adequate spacing between buttons
- Match button size to importance
- Don't use all caps (reduces readability)

## Icon Guidelines
- Use 20px icons for medium buttons
- Maintain 8px gap between icon and text
- Icon should enhance, not replace text (except icon-only)
- Common icons: arrow-right, check, x, download, external-link

## Code Example Pattern
```html
<!-- Primary Button -->
<button class="btn btn-primary">Get Started</button>

<!-- Secondary Button -->
<button class="btn btn-secondary">Learn More</button>

<!-- Button with Icon -->
<button class="btn btn-primary">
  <span>Download</span>
  <svg><!-- icon --></svg>
</button>

<!-- Disabled Button -->
<button class="btn btn-primary" disabled>Submit</button>
```

## Testing Checklist
- [ ] All states render correctly
- [ ] Focus state is visible
- [ ] Keyboard navigation works
- [ ] Touch targets meet minimum size (44px)
- [ ] Color contrast passes WCAG standards
- [ ] Disabled state prevents interaction
- [ ] Loading state shows appropriate feedback
- [ ] Works across browsers
- [ ] Responsive at all breakpoints
