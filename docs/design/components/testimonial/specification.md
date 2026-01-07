# Testimonial Component Specification

## Overview
The Testimonial component displays client quotes, feedback, and reviews. It builds trust and social proof by showcasing positive experiences from past clients.

## Dimensions
- **Width**: Flexible (280-400px typical for single testimonial)
- **Height**: Auto (content-driven)
- **Padding**: 32px (internal)
- **Border radius**: 12px
- **Avatar size**: 64px × 64px (circular)
- **Rating stars**: 20px × 20px each

## Layout Structure
```
┌────────────────────────────────────┐
│                                    │
│  ★★★★★ (optional rating)           │
│                                    │
│  "Quote text goes here. This is    │
│  what the client said about our    │
│  services and experience..."       │
│                                    │
│  ┌────┐                            │
│  │ 👤 │  Client Name               │
│  └────┘  Position, Company         │
│                                    │
└────────────────────────────────────┘
```

## Spacing
- **Stars margin-bottom**: 16px
- **Quote margin-bottom**: 24px
- **Avatar margin-right**: 16px
- **Client name margin-bottom**: 4px
- **Card padding**: 32px
- **Gap between testimonials**: 24px

## Typography
- **Quote Text**: 
  - Font size: 16px (desktop), 15px (mobile)
  - Font weight: 400 (Regular)
  - Line height: 1.7
  - Color: #2D3748
  - Style: Italic (optional)
  
- **Client Name**: 
  - Font size: 16px
  - Font weight: 600 (Semi-bold)
  - Line height: 1.4
  - Color: #1A1A1A
  
- **Client Position/Company**: 
  - Font size: 14px
  - Font weight: 400
  - Line height: 1.4
  - Color: #718096

## Color Codes
- **Background**: #FFFFFF
- **Border**: 1px solid #E2E8F0 or none
- **Shadow**: 0 2px 8px rgba(0, 0, 0, 0.08)
- **Quote marks**: #667eea or #E2E8F0
- **Stars (filled)**: #FFC107 (gold)
- **Stars (empty)**: #E2E8F0
- **Text**: #2D3748
- **Name**: #1A1A1A
- **Position**: #718096

## States

### Default
- Standard background
- Subtle shadow
- Normal text colors

### Hover
- Shadow increases: 0 4px 16px rgba(0, 0, 0, 0.12)
- Slight lift: translateY(-2px)
- Transition: 0.3s ease

## Components Breakdown

### Quote Section
- Main testimonial text
- 100-200 characters recommended
- Quotation marks (decorative, optional)
- Italic styling (optional)

### Rating (Optional)
- 5-star system
- Filled/empty states
- Color: Gold (#FFC107)
- Display above quote

### Client Info
- Avatar/photo (circular)
- Name (bold)
- Position/title
- Company name
- Logo (optional, small)

## Variants

### 1. Card Style (Default)
- White background
- Border or shadow
- Padding around content
- Most common

### 2. Minimal
- No background
- No border
- Simple text display
- Left-aligned

### 3. Featured/Hero
- Larger text (20-24px)
- Center-aligned
- More prominent
- Used for key testimonials

### 4. With Logo
- Company logo included
- Logo size: 100px × 40px
- Grayscale or color
- Placed below client info

### 5. Video Testimonial
- Play button overlay
- Thumbnail image
- Client info overlay
- Duration indicator

### 6. Carousel/Slider
- Multiple testimonials
- Navigation arrows
- Dots indicator
- Auto-rotate (optional)

## Layout Patterns

### Grid Layout
```css
display: grid;
grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
gap: 24px;
```

### Carousel/Slider
- Single testimonial visible
- Navigation controls
- Smooth transitions
- Touch/swipe enabled

### Stacked
- Full-width testimonials
- Vertical stack
- Alternating alignment (optional)

## Responsive Breakpoints
- **Desktop**: > 1024px (3 columns typical)
- **Tablet**: 768px - 1024px (2 columns)
- **Mobile**: < 768px (1 column, full width)

## Accessibility
- Semantic HTML with `<blockquote>` for quotes
- Proper figure/figcaption structure
- Alt text for avatar images
- ARIA labels for rating stars
- Keyboard navigable (if carousel)
- Focus visible states
- Screen reader friendly

## Rating System
- Visual: Stars (★★★★★)
- Filled stars: #FFC107
- Empty stars: #E2E8F0
- Half-star support (optional)
- Alternative: Score out of 5 (e.g., "4.5/5")

## Quote Marks Styling
- Large decorative quotes: 48-64px
- Positioned absolutely
- Color: #667eea (15% opacity)
- Optional: Icon font or SVG

## Avatar Specifications
- Size: 64px × 64px (standard), 48px × 48px (small)
- Shape: Circular (border-radius: 50%)
- Border: 2px solid #FFFFFF (optional)
- Fallback: Initials on colored background
- Image format: WebP or JPG

## Animation
- **On scroll into view**: Fade in with slight scale
- **Duration**: 0.5s ease-out
- **Stagger**: 0.1s delay between cards
- **Carousel transitions**: Slide or fade, 0.4s

## Content Guidelines
- **Quote length**: 80-200 characters ideal
- **Attribution**: Always include name and company
- **Authenticity**: Use real testimonials
- **Permissions**: Obtain client approval
- **Variety**: Show diverse clients/industries
- **Specificity**: Specific praise better than generic

## Common Testimonial Themes
- Problem solved
- Results achieved
- Experience working together
- Quality of service/product
- Recommendation to others

## Implementation Notes
- Use `<blockquote>` for semantic HTML
- Lazy load avatar images
- Consider carousel library (Swiper, Slick)
- Truncate long quotes with "Read more"
- Link company name (optional)
- Schema markup for SEO (Review schema)
- Test with various quote lengths

## Schema Markup Example
```html
<div itemscope itemtype="https://schema.org/Review">
  <blockquote itemprop="reviewBody">Quote text...</blockquote>
  <div itemprop="author" itemscope itemtype="https://schema.org/Person">
    <span itemprop="name">Client Name</span>
  </div>
  <div itemprop="reviewRating" itemscope itemtype="https://schema.org/Rating">
    <meta itemprop="ratingValue" content="5">
  </div>
</div>
```

## Best Practices
- Use high-quality avatar images
- Keep quotes concise and impactful
- Include specific details/metrics when possible
- Rotate testimonials to show variety
- Update regularly with recent feedback
- Verify authenticity
- Link to full case study (optional)
- Match visual style to overall design

## Trust Indicators (Optional Add-ons)
- Client company logos
- Project names/links
- Dates of projects
- Verified badge
- Video testimonials
- LinkedIn profile links
