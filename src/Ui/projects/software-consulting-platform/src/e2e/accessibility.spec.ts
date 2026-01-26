import { test, expect } from '@playwright/test';
import { mockLandingPageData } from './fixtures/landing-data';

test.describe('Landing Page Accessibility', () => {
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

  test('navigation should be keyboard accessible', async ({ page }) => {
    await page.goto('/');

    // Tab through navigation links
    await page.keyboard.press('Tab');
    await page.keyboard.press('Tab');

    // Check that a navigation link has focus
    const focusedElement = await page.locator(':focus');
    await expect(focusedElement).toBeVisible();
  });

  test('buttons should have accessible names', async ({ page }) => {
    await page.goto('/');

    // Check all buttons have accessible names
    const buttons = page.getByRole('button');
    const buttonCount = await buttons.count();

    for (let i = 0; i < buttonCount; i++) {
      const button = buttons.nth(i);
      const accessibleName = await button.getAttribute('aria-label') || await button.textContent();
      expect(accessibleName?.trim().length).toBeGreaterThan(0);
    }
  });

  test('links should be distinguishable', async ({ page }) => {
    await page.goto('/');

    // Check navigation links
    const navLinks = page.locator('.sc-navigation__link');
    const navLinkCount = await navLinks.count();

    for (let i = 0; i < navLinkCount; i++) {
      const link = navLinks.nth(i);
      const text = await link.textContent();
      expect(text?.trim().length).toBeGreaterThan(0);
    }
  });

  test('mobile navigation toggle should have aria attributes', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    await page.goto('/');

    const toggle = page.locator('.sc-navigation__mobile-toggle');
    await expect(toggle).toHaveAttribute('aria-label', 'Toggle navigation menu');
    await expect(toggle).toHaveAttribute('aria-expanded', 'false');

    // Click toggle
    await toggle.click();
    await expect(toggle).toHaveAttribute('aria-expanded', 'true');
  });

  test('testimonial carousel navigation should be accessible', async ({ page }) => {
    await page.goto('/');

    // Check prev/next buttons have aria-labels
    const prevButton = page.locator('.sc-testimonial-carousel__nav--prev');
    const nextButton = page.locator('.sc-testimonial-carousel__nav--next');

    if (await prevButton.isVisible()) {
      await expect(prevButton).toHaveAttribute('aria-label', 'Previous testimonial');
      await expect(nextButton).toHaveAttribute('aria-label', 'Next testimonial');
    }

    // Check dots have aria-labels
    const dots = page.locator('.sc-testimonial-carousel__dot');
    const dotCount = await dots.count();
    for (let i = 0; i < dotCount; i++) {
      const dot = dots.nth(i);
      await expect(dot).toHaveAttribute('aria-label', `Go to testimonial ${i + 1}`);
    }
  });

  test('page should have semantic HTML structure', async ({ page }) => {
    await page.goto('/');

    // Check for main landmark
    await expect(page.locator('main')).toBeVisible();

    // Check for nav landmark
    await expect(page.locator('nav')).toBeVisible();

    // Check for footer landmark
    await expect(page.locator('footer')).toBeVisible();
  });
});
