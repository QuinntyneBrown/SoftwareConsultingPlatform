import { test, expect } from '@playwright/test';
import { mockLandingPageData } from './fixtures/landing-data';

test.describe('Landing Page', () => {
  test.beforeEach(async ({ page }) => {
    // Mock the API endpoint
    await page.route('**/api/landing', async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockLandingPageData),
      });
    });
  });

  test('should display the navigation header', async ({ page }) => {
    await page.goto('/');

    // Check navigation is visible
    const navigation = page.locator('sc-navigation');
    await expect(navigation).toBeVisible();

    // Check navigation links
    await expect(page.getByRole('link', { name: 'Home' })).toBeVisible();
    await expect(page.getByRole('link', { name: 'Services' })).toBeVisible();
    await expect(page.getByRole('link', { name: 'Case Studies' })).toBeVisible();
    await expect(page.getByRole('link', { name: 'About' })).toBeVisible();
    await expect(page.getByRole('link', { name: 'Contact' })).toBeVisible();
  });

  test('should display the hero section with headline and subheadline', async ({ page }) => {
    await page.goto('/');

    const hero = page.locator('sc-hero');
    await expect(hero).toBeVisible();

    // Check headline
    await expect(page.getByRole('heading', { level: 1 })).toContainText(
      'Transform Your Business with Expert Software Consulting'
    );

    // Check subheadline
    await expect(page.locator('.sc-hero__subheadline')).toContainText(
      'We deliver innovative solutions'
    );

    // Check CTA buttons
    await expect(page.getByRole('button', { name: 'Get Started' })).toBeVisible();
    await expect(page.getByRole('button', { name: 'Learn More' })).toBeVisible();
  });

  test('should display the services section', async ({ page }) => {
    await page.goto('/');

    // Check services section heading
    await expect(page.getByRole('heading', { name: 'Our Services' })).toBeVisible();

    // Check service items
    await expect(page.locator('.sc-service-item')).toHaveCount(3);

    // Check service titles
    await expect(page.getByText('Custom Development')).toBeVisible();
    await expect(page.getByText('Cloud Solutions')).toBeVisible();
    await expect(page.getByText('Security Consulting')).toBeVisible();
  });

  test('should display the value proposition section', async ({ page }) => {
    await page.goto('/');

    // Check value proposition section
    const valueProp = page.locator('sc-value-proposition');
    await expect(valueProp).toBeVisible();

    // Check section title
    await expect(page.getByRole('heading', { name: 'Why Choose Us' })).toBeVisible();

    // Check value propositions
    await expect(page.getByText('Proven Expertise')).toBeVisible();
    await expect(page.getByText('Partnership Approach')).toBeVisible();
    await expect(page.getByText('Agile Delivery')).toBeVisible();
  });

  test('should display the case studies section', async ({ page }) => {
    await page.goto('/');

    // Check case studies section heading
    await expect(page.getByRole('heading', { name: 'Featured Case Studies' })).toBeVisible();

    // Check case study items
    const caseStudies = page.locator('.sc-case-study');
    await expect(caseStudies).toHaveCount(2);

    // Check case study titles
    await expect(page.getByText('Healthcare Platform Modernization')).toBeVisible();
    await expect(page.getByText('Financial Services API Platform')).toBeVisible();

    // Check "View All" link
    await expect(page.getByRole('link', { name: /View All Case Studies/i })).toBeVisible();
  });

  test('should display the testimonials carousel', async ({ page }) => {
    await page.goto('/');

    // Check testimonials section
    const testimonialCarousel = page.locator('sc-testimonial-carousel');
    await expect(testimonialCarousel).toBeVisible();

    // Check section title
    await expect(page.getByRole('heading', { name: 'What Our Clients Say' })).toBeVisible();

    // Check at least one testimonial is visible
    await expect(page.locator('.sc-testimonial')).toBeVisible();

    // Check navigation dots (should have 2 for 2 testimonials)
    await expect(page.locator('.sc-testimonial-carousel__dot')).toHaveCount(2);
  });

  test('should navigate testimonial carousel', async ({ page }) => {
    await page.goto('/');

    // Wait for testimonial section to load
    await page.waitForSelector('.sc-testimonial-carousel');

    // Click next button if visible
    const nextButton = page.locator('.sc-testimonial-carousel__nav--next');
    if (await nextButton.isVisible()) {
      await nextButton.click();

      // Check that second dot is now active
      const dots = page.locator('.sc-testimonial-carousel__dot');
      await expect(dots.nth(1)).toHaveClass(/--active/);
    }
  });

  test('should display the CTA section', async ({ page }) => {
    await page.goto('/');

    // Check CTA section
    const ctaSection = page.locator('sc-cta-section');
    await expect(ctaSection).toBeVisible();

    // Check CTA headline
    await expect(page.getByRole('heading', { name: /Ready to Transform Your Business/i })).toBeVisible();

    // Check CTA buttons
    await expect(page.getByRole('button', { name: 'Start a Project' })).toBeVisible();
    await expect(page.getByRole('button', { name: 'Contact Us' })).toBeVisible();
  });

  test('should display the footer', async ({ page }) => {
    await page.goto('/');

    // Check footer is visible
    const footer = page.locator('sc-footer');
    await expect(footer).toBeVisible();

    // Check footer tagline
    await expect(page.locator('.sc-footer__tagline')).toContainText(
      'Transforming businesses through innovative software solutions'
    );

    // Check copyright
    await expect(page.locator('.sc-footer__copyright')).toContainText('© 2026');

    // Check legal links
    await expect(page.getByRole('link', { name: 'Privacy Policy' })).toBeVisible();
    await expect(page.getByRole('link', { name: 'Terms of Service' })).toBeVisible();
  });

  test('should be responsive on mobile', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    await page.goto('/');

    // Check that hamburger menu is visible on mobile
    const mobileToggle = page.locator('.sc-navigation__mobile-toggle');
    await expect(mobileToggle).toBeVisible();

    // Click hamburger menu
    await mobileToggle.click();

    // Check mobile menu is open
    await expect(page.locator('.sc-navigation__mobile--open')).toBeVisible();
  });

  test('should have accessible heading hierarchy', async ({ page }) => {
    await page.goto('/');

    // Check h1 exists (hero headline)
    const h1s = await page.locator('h1').all();
    expect(h1s.length).toBe(1);

    // Check multiple h2s exist for sections
    const h2s = await page.locator('h2').all();
    expect(h2s.length).toBeGreaterThanOrEqual(4);
  });

  test('should handle API errors gracefully', async ({ page }) => {
    // Override the mock to return an error
    await page.route('**/api/landing', async (route) => {
      await route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'Internal Server Error' }),
      });
    });

    await page.goto('/');

    // Page should still render with default data
    await expect(page.locator('sc-hero')).toBeVisible();
    await expect(page.getByRole('heading', { level: 1 })).toBeVisible();
  });
});
