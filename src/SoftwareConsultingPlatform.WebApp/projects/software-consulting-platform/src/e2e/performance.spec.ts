import { test, expect } from '@playwright/test';
import { mockLandingPageData } from './fixtures/landing-data';

test.describe('Landing Page Performance', () => {
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

  test('page should load within acceptable time', async ({ page }) => {
    const startTime = Date.now();
    await page.goto('/');

    // Wait for the hero to be visible (indicates page is loaded)
    await expect(page.locator('sc-hero')).toBeVisible();

    const loadTime = Date.now() - startTime;

    // Page should load within 5 seconds (generous limit for test environment)
    expect(loadTime).toBeLessThan(5000);
  });

  test('all critical sections should render', async ({ page }) => {
    await page.goto('/');

    // Wait for all critical sections to be visible
    await expect(page.locator('sc-navigation')).toBeVisible();
    await expect(page.locator('sc-hero')).toBeVisible();
    await expect(page.locator('sc-service-listing')).toBeVisible();
    await expect(page.locator('sc-value-proposition')).toBeVisible();
    await expect(page.locator('sc-case-study-grid')).toBeVisible();
    await expect(page.locator('sc-testimonial-carousel')).toBeVisible();
    await expect(page.locator('sc-cta-section')).toBeVisible();
    await expect(page.locator('sc-footer')).toBeVisible();
  });

  test('page should not have console errors', async ({ page }) => {
    const consoleErrors: string[] = [];

    page.on('console', (msg) => {
      if (msg.type() === 'error') {
        consoleErrors.push(msg.text());
      }
    });

    await page.goto('/');
    await page.waitForLoadState('networkidle');

    // Filter out known non-critical errors (like 404s for missing images)
    const criticalErrors = consoleErrors.filter(
      (error) => !error.includes('assets/') && !error.includes('favicon')
    );

    expect(criticalErrors).toHaveLength(0);
  });
});
