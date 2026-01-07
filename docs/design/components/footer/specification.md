# Footer Component Specification

## Overview
The Footer component is the site-wide bottom section containing navigation links, contact information, social media links, and legal information. It appears on every page.

## Dimensions
- **Width**: 100% (full viewport width)
- **Height**: Variable (300-400px typical)
- **Container max-width**: 1200px
- **Padding**: 60px 40px 40px (desktop), 40px 20px 20px (mobile)
- **Column gap**: 80px (desktop), 40px (tablet), 24px (mobile)

## Layout Structure
```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌──────────┐  │
│  │ Company │  │ Services│  │ Company │  │ Connect  │  │
│  │         │  │         │  │         │  │          │  │
│  │ Logo    │  │ Link 1  │  │ About   │  │ Email    │  │
│  │ Tagline │  │ Link 2  │  │ Careers │  │ Phone    │  │
│  │         │  │ Link 3  │  │ Contact │  │ Socials  │  │
│  └─────────┘  └─────────┘  └─────────┘  └──────────┘  │
│                                                         │
│  ────────────────────────────────────────────────────  │
│                                                         │
│  © 2024 Company    |    Privacy    |    Terms          │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

## Spacing
- **Top padding**: 60px (desktop), 40px (mobile)
- **Bottom padding**: 40px (desktop), 20px (mobile)
- **Column gap**: 80px (desktop), 40px (tablet)
- **Link spacing**: 12px (vertical gap between links)
- **Divider margin**: 32px (top and bottom)
- **Copyright row top margin**: 32px

## Typography

### Footer Heading (Column Titles)
- **Font size**: 16px
- **Font weight**: 600 (Semi-bold)
- **Line height**: 1.5
- **Color**: #1A1A1A
- **Margin-bottom**: 16px

### Footer Links
- **Font size**: 15px
- **Font weight**: 400 (Regular)
- **Line height**: 2
- **Color**: #4A5568
- **Hover color**: #667eea

### Company Tagline
- **Font size**: 14px
- **Font weight**: 400
- **Line height**: 1.6
- **Color**: #718096

### Copyright Text
- **Font size**: 14px
- **Font weight**: 400
- **Color**: #718096

### Contact Info
- **Font size**: 14px
- **Font weight**: 400
- **Color**: #4A5568

## Color Codes
- **Background**: #F7FAFC or #1A202C (dark variant)
- **Text (Primary)**: #1A1A1A (light mode), #FFFFFF (dark mode)
- **Text (Secondary)**: #4A5568 (light mode), #A0AEC0 (dark mode)
- **Link Hover**: #667eea
- **Border/Divider**: #E2E8F0 (light mode), #2D3748 (dark mode)
- **Social Icons**: #718096 (default), #667eea (hover)
- **Logo**: Brand colors

## Components Breakdown

### Column 1: Company/Brand
- Logo (120px × 40px)
- Tagline (1-2 lines, max 200 characters)
- Optional: Mission statement

### Column 2-3: Navigation Links
- **Column 2**: Services
  - MVP Development
  - Product Strategy
  - UI/UX Design
  - Engineering
  
- **Column 3**: Company
  - About Us
  - Case Studies
  - Blog
  - Careers
  - Contact

### Column 4: Contact & Social
- Email address (linked)
- Phone number (linked)
- Office address (optional)
- Social media icons (4-6 icons)

### Bottom Bar
- Copyright notice
- Legal links (Privacy Policy, Terms of Service)
- Optional: Language selector
- Optional: Theme toggle

## States

### Link Hover
- Color transitions to brand color
- Slight left indent (2px)
- Transition duration: 0.2s

### Social Icon Hover
- Background color appears (circle)
- Icon color changes to brand color
- Scale: 1.1
- Transition duration: 0.2s

## Responsive Breakpoints
- **Desktop**: > 1024px (4 columns)
- **Tablet**: 768px - 1024px (2 columns, 2 rows)
- **Mobile**: < 768px (1 column, stacked)

## Accessibility
- Semantic `<footer>` tag
- Navigation wrapped in `<nav>` with aria-label
- Links have sufficient color contrast
- Keyboard navigable
- Focus visible states
- Email and phone links use proper href formats
- Social icons have descriptive labels

## Social Media Icons
- **Size**: 24px × 24px (icon), 40px × 40px (touch target)
- **Spacing**: 12px gap between icons
- **Common platforms**: LinkedIn, Twitter/X, GitHub, Facebook, Instagram
- **Style**: Monochrome, filled on hover

## Legal Links
- Privacy Policy
- Terms of Service
- Cookie Policy (if applicable)
- Accessibility Statement (if applicable)

## Variants

### 1. Light Theme (Default)
- Light background (#F7FAFC)
- Dark text
- Standard contrast

### 2. Dark Theme
- Dark background (#1A202C)
- Light text (#FFFFFF)
- Inverted colors

### 3. Minimal
- Single column
- Centered content
- Fewer links
- Used for simple sites

### 4. Extended
- Newsletter signup form
- Recent blog posts
- Additional columns
- Used for content-heavy sites

## Optional Elements

### Newsletter Signup
- Email input field
- Subscribe button
- Privacy note
- Placement: Above bottom bar or in right column

### Awards/Certifications
- Logo badges
- Size: 60px × 60px
- Grayscale, colored on hover

### Language Selector
- Dropdown or links
- Flag icons optional
- Current language highlighted

## Implementation Notes
- Use CSS Grid for column layout
- Stack vertically on mobile
- Optimize for accessibility
- Keep links updated
- Test all contact links
- Consider sticky footer for short pages
- Lazy load social media widgets if embedded

## Content Guidelines
- Keep link text concise and clear
- Ensure all links work (no 404s)
- Update copyright year automatically
- Provide real contact information
- Link to actual social profiles
- Keep privacy policy up to date

## Animation
- **On scroll into view**: Fade in with slight upward movement
- **Link hover**: Color transition 0.2s
- **Social icon hover**: Scale and color 0.2s
- **Stagger effect**: Columns fade in with 0.1s delay
