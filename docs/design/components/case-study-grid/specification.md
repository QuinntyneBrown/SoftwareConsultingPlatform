# Case Study Grid Component Specification

## Overview
The Case Study Grid displays portfolio projects and client work in an organized, visually appealing format. It showcases successful projects with images, descriptions, and key metrics.

## Dimensions
- **Container width**: 100% (max-width: 1200px)
- **Grid item width**: Flexible (1/2 or 1/3 of container)
- **Item height**: Auto or fixed (400-500px for uniform grid)
- **Image height**: 250-300px (16:9 or 3:2 ratio)
- **Content padding**: 32px
- **Border radius**: 12px
- **Overlay height**: 100% (full cover on hover)

## Layout Structure
```
┌────────────────┐  ┌────────────────┐  ┌────────────────┐
│                │  │                │  │                │
│  ┌──────────┐  │  │  ┌──────────┐  │  │  ┌──────────┐  │
│  │  Image   │  │  │  │  Image   │  │  │  │  Image   │  │
│  └──────────┘  │  │  └──────────┘  │  │  └──────────┘  │
│                │  │                │  │                │
│  Tags          │  │  Tags          │  │  Tags          │
│  Project Title │  │  Project Title │  │  Project Title │
│  Description   │  │  Description   │  │  Description   │
│  [View Case]   │  │  [View Case]   │  │  [View Case]   │
│                │  │                │  │                │
└────────────────┘  └────────────────┘  └────────────────┘
```

## Spacing
- **Grid gap**: 32px (desktop), 24px (tablet), 20px (mobile)
- **Image margin-bottom**: 20px
- **Tags margin-bottom**: 12px
- **Title margin-bottom**: 12px
- **Description margin-bottom**: 20px
- **Card padding**: 0 (image edge-to-edge) or 32px (with padding)
- **Content padding**: 24px (below image)

## Typography

### Project Title
- **Font size**: 22px (desktop), 20px (mobile)
- **Font weight**: 600 (Semi-bold)
- **Line height**: 1.3
- **Color**: #1A1A1A

### Description
- **Font size**: 15px
- **Font weight**: 400 (Regular)
- **Line height**: 1.6
- **Color**: #4A5568
- **Max lines**: 2-3 (truncated with ellipsis)

### Tags/Categories
- **Font size**: 12px
- **Font weight**: 600 (Semi-bold)
- **Text transform**: Uppercase
- **Letter spacing**: 0.5px
- **Color**: #667eea

### Metrics/Stats
- **Font size**: 24px (number), 14px (label)
- **Font weight**: 700 (number), 400 (label)
- **Color**: #1A1A1A (number), #718096 (label)

### Link/CTA
- **Font size**: 15px
- **Font weight**: 600
- **Color**: #667eea

## Color Codes
- **Card background**: #FFFFFF
- **Image overlay**: rgba(0, 0, 0, 0.6) or gradient
- **Tag background**: #EEF2FF
- **Tag text**: #667eea
- **Title**: #1A1A1A
- **Title (hover)**: #667eea
- **Description**: #4A5568
- **Link**: #667eea
- **Border**: 1px solid #E2E8F0 (optional)
- **Shadow**: 0 2px 8px rgba(0, 0, 0, 0.08)
- **Shadow (hover)**: 0 8px 24px rgba(0, 0, 0, 0.12)

## States

### Default
- Standard colors
- Subtle shadow
- Image at 100% opacity
- No overlay

### Hover
- Shadow increases
- Card lifts (translateY(-4px))
- Image scales (scale: 1.05)
- Overlay appears with "View Project" text
- Title color changes to brand color
- Transition: 0.3s ease

### Focus
- Focus outline for accessibility
- Similar to hover state

## Image Specifications
- **Aspect ratio**: 16:9 (recommended) or 3:2
- **Dimensions**: 600px × 338px (16:9) or 600px × 400px (3:2)
- **Object-fit**: cover
- **Format**: WebP or JPG
- **Optimization**: Compress for web
- **Lazy loading**: Enabled

## Layout Patterns

### 1. Standard Grid (Default)
- 3 columns (desktop)
- 2 columns (tablet)
- 1 column (mobile)
- Equal heights

### 2. Masonry Layout
- Variable heights
- Packed layout
- More dynamic appearance

### 3. Featured + Grid
- Large featured case (full width)
- Smaller cases in grid below

### 4. List View
- Full-width rows
- Image on left, content on right
- Alternative to grid

## Components Breakdown

### Image/Thumbnail
- High-quality project screenshot
- Overflow hidden for scale effect
- Background color during load

### Tags/Categories
- Industry (Healthcare, FinTech, etc.)
- Service type (MVP, Full-Stack, etc.)
- Technology (React, Node.js, etc.)
- 1-3 tags maximum

### Project Title
- Clear, descriptive name
- 3-8 words recommended
- Clickable (links to case study)

### Description
- Brief project overview
- 1-2 sentences
- Focus on problem solved or outcome
- Truncated if too long

### Metrics/Results (Optional)
- Key statistics
- "200% increase in conversions"
- "10K daily active users"
- "50% faster performance"

### Client Logo (Optional)
- Company logo
- Grayscale or color
- Size: 100px × 40px

### CTA Link
- "View Case Study"
- "Read More"
- "See Project"

## Responsive Breakpoints
- **Desktop**: > 1024px (3 columns)
- **Tablet**: 768px - 1024px (2 columns)
- **Mobile**: < 768px (1 column)

## Accessibility
- Semantic HTML (`<article>` for each case)
- Proper heading hierarchy (H3 for titles)
- Alt text for images (descriptive)
- Keyboard navigable
- Focus visible states
- Sufficient color contrast
- ARIA labels where needed

## Overlay Styles

### On Hover
- Background: rgba(0, 0, 0, 0.7)
- Text: "View Project" centered
- Icon: Arrow or eye icon
- Fade in transition

## Filter/Sort Options (Optional)

### Filters
- By industry
- By service type
- By technology
- By year

### Sort
- Most recent
- Featured first
- Alphabetical

### UI Pattern
- Dropdown or buttons
- Above grid
- Mobile: Slide-out panel

## Animation
- **On scroll into view**: Fade in with upward movement
- **Stagger effect**: 0.1s delay between cards
- **Image hover**: Scale 1.05, duration 0.4s
- **Card hover**: Lift and shadow, duration 0.3s

## Featured Case Study Variant
- Larger size (full row or 2 columns)
- More prominent placement
- Additional details visible
- Larger image

## Metrics Display (Optional)
```
┌──────────────┐
│   200%       │  ← Number (large, bold)
│   Growth     │  ← Label (small)
└──────────────┘
```

## Implementation Notes
- Use CSS Grid for layout
- Lazy load images
- Consider pagination or infinite scroll
- Optimize images (WebP format)
- Link entire card or just CTA
- Ensure consistent image quality
- Test with various content lengths

## Content Guidelines
- **Title**: Project name, not generic
- **Description**: Focus on impact/results
- **Tags**: Relevant, limited to 2-3
- **Images**: Professional quality
- **Metrics**: Real data when possible
- **Update regularly**: Keep portfolio current

## Grid Code Example
```css
.case-studies-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 32px;
}
```

## Best Practices
- Use high-quality project images
- Keep descriptions concise
- Highlight measurable results
- Maintain consistent card heights
- Provide good image alt text
- Link to full case study page
- Test with real content
- Ensure fast image loading
- Update portfolio regularly
- Show diverse projects

## Empty State
- Message: "More case studies coming soon"
- CTA: "Contact us about your project"
- Alternative: Show placeholder cards

## Loading State
- Skeleton screens
- Fade-in animation
- Smooth transitions

## Common Industries
- Healthcare
- FinTech
- E-Commerce
- SaaS
- Education
- Media & Entertainment
- Real Estate
- IoT

## Technology Tags
- React, Vue, Angular
- Node.js, Python, .NET
- Mobile (iOS, Android)
- Cloud (AWS, Azure, GCP)
- Database (PostgreSQL, MongoDB)
