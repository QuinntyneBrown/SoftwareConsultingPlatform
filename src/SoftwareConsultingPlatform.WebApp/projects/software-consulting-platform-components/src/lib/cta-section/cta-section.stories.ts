import type { Meta, StoryObj } from '@storybook/angular';
import { CtaSection } from './cta-section';

const meta: Meta<CtaSection> = {
  title: 'Components/CTA Section',
  component: CtaSection,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<CtaSection>;

export const Standard: Story = {
  args: {
    headline: 'Ready to Get Started?',
    subheadline: 'Join thousands of satisfied customers today.',
    variant: 'standard',
  },
};

export const Brand: Story = {
  args: {
    headline: 'Transform Your Business',
    subheadline: 'Let us help you achieve your goals.',
    variant: 'brand',
  },
};

export const Dark: Story = {
  args: {
    headline: 'Need Help?',
    subheadline: 'Contact our support team anytime.',
    variant: 'dark',
  },
};
