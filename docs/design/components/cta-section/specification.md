# CTA Section Component Specification

## Overview
The CTA (Call-to-Action) Section is a prominent content block designed to encourage user action. It's strategically placed throughout the site to drive conversions.

## Dimensions
- **Width**: 100% (full viewport width) or contained (max-width: 1200px)
- **Height**: Variable (300-500px typical)
- **Padding**: 80px 40px (desktop), 60px 20px (mobile)
- **Border radius**: 16px (for contained variant)
- **Content max-width**: 700px (centered)

## Layout Structure
```
┌─────────────────────────────────────────┐
│                                         │
│         ┌─────────────────────┐         │
│         │                     │         │
│         │    Headline (H2)    │         │
│         │                     │         │
│         │    Subheadline      │         │
│         │                     │         │
│         │   [Primary Button]  │         │
│         │   [Secondary Link]  │         │
│         │                     │         │
│         └─────────────────────┘         │
│                                         │
└─────────────────────────────────────────┘
```

## Spacing
- **Headline margin-bottom**: 16px
- **Subheadline margin-bottom**: 32px
- **Button gap**: 16px (between buttons)
- **Section padding**: 80px (top/bottom, desktop), 60px (mobile)
- **Side padding**: 40px (desktop), 20px (mobile)

## Typography
- **Headline**: 
  - Font size: 36px (desktop), 28px (tablet), 24px (mobile)
  - Font weight: 700 (Bold)
  - Line height: 1.2
  - Color: #1A1A1A or #FFFFFF (depending on background)
  
- **Subheadline**: 
  - Font size: 18px (desktop), 16px (mobile)
  - Font weight: 400 (Regular)
  - Line height: 1.6
  - Color: #4A5568 or rgba(255, 255, 255, 0.9)

## Color Codes

### Standard Variant
- **Background**: #F7FAFC
- **Headline**: #1A1A1A
- **Subheadline**: #4A5568

### Brand Variant
- **Background**: Linear gradient (#667eea to #764ba2)
- **Headline**: #FFFFFF
- **Subheadline**: rgba(255, 255, 255, 0.9)

### Dark Variant
- **Background**: #1A202C
- **Headline**: #FFFFFF
- **Subheadline**: #A0AEC0

### Accent Variant
- **Background**: #EEF2FF
- **Border**: 2px solid #667eea
- **Headline**: #1A1A1A
- **Subheadline**: #4A5568

## States

### Default
- Static background
- Standard colors
- Buttons in default state

### With Animation (on scroll into view)
- Fade in with upward movement
- Duration: 0.6s
- Easing: ease-out
- Stagger: Headline (0s), subheadline (0.1s), buttons (0.2s)

## Variants

### 1. Centered (Default)
- Text center-aligned
- Buttons centered horizontally
- Most common placement

### 2. Split Layout
- Text on left (50%)
- Visual/image on right (50%)
- Desktop only (stacks on mobile)

### 3. Full Width
- Edge-to-edge background
- Contained content within max-width
- Used for major conversions

### 4. Contained/Card
- Max-width container
- Border radius applied
- Shadow: 0 4px 16px rgba(0, 0, 0, 0.1)
- Used within page content

### 5. Minimal
- No background color
- Simple border (top/bottom)
- Subtle design
- Lower visual priority

### 6. With Form
- Email input + button inline
- Newsletter signup pattern
- Form below subheadline

## Content Elements

### Required
- Headline (compelling action statement)
- Primary button (clear CTA)

### Optional
- Subheadline (supporting text)
- Secondary button or text link
- Supporting icon or graphic
- Trust indicators (logos, testimonials)
- Background image or pattern

## Responsive Breakpoints
- **Desktop**: > 1024px (full layout)
- **Tablet**: 768px - 1024px (adjusted padding/sizing)
- **Mobile**: < 768px (stack layout, smaller text)

## Accessibility
- Semantic HTML with `<section>` tag
- Proper heading hierarchy
- Sufficient color contrast (WCAG AA: 4.5:1 minimum)
- Keyboard navigable buttons
- Focus visible states
- ARIA labels if needed for context

## Common Use Cases

### 1. Homepage Hero CTA
- "Start Your Project"
- "Get a Free Consultation"
- Primary conversion goal

### 2. Mid-Page CTA
- "See Our Case Studies"
- "Learn About Our Process"
- Engagement driver

### 3. Bottom-of-Page CTA
- "Ready to Get Started?"
- "Contact Us Today"
- Last conversion opportunity

### 4. Newsletter Signup
- "Stay Updated"
- Email capture
- Lead generation

### 5. Downloadable Content
- "Download Our Guide"
- "Get the Whitepaper"
- Content marketing

## Animation
- **On scroll into view**: 
  - Fade in: opacity 0 to 1
  - Slide up: translateY(30px) to 0
  - Duration: 0.6s ease-out
- **Stagger effect**: 
  - Headline: 0s delay
  - Subheadline: 0.1s delay
  - Buttons: 0.2s delay

## Best Practices
- Keep headline short and action-oriented (5-10 words)
- Subheadline adds context, not required
- Use contrasting button colors
- One primary CTA per section
- Test different placements and copy
- Ensure mobile-friendly design
- Make buttons large enough to tap easily

## Content Guidelines
- **Headline**: Clear value proposition or action
- **Subheadline**: Brief explanation or benefit (optional)
- **Button text**: Action verb (Start, Get, Download, Contact)
- **Character limits**: 
  - Headline: 40-60 characters
  - Subheadline: 80-120 characters

## Background Options
- Solid color
- Gradient
- Image with overlay
- Pattern or texture
- Video (muted, autoplay)

## Implementation Notes
- Use flexbox for centering
- Ensure z-index for overlays
- Optimize background images
- Test contrast ratios
- Consider A/B testing different CTAs
- Track conversion metrics
- Use clear, actionable language

## Code Example Pattern
```html
<section class="cta-section">
  <div class="cta-container">
    <h2 class="cta-headline">Ready to Build Something Great?</h2>
    <p class="cta-subheadline">Let's discuss your project and bring your vision to life.</p>
    <div class="cta-buttons">
      <a href="#" class="btn btn-primary">Start Your Project</a>
      <a href="#" class="btn btn-secondary">View Our Work</a>
    </div>
  </div>
</section>
```
