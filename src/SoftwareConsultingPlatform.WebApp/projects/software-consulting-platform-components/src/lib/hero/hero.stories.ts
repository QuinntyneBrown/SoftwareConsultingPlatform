import type { Meta, StoryObj } from '@storybook/angular';
import { Hero } from './hero';

const meta: Meta<Hero> = {
  title: 'Components/Hero',
  component: Hero,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<Hero>;

export const Default: Story = {
  args: {
    headline: 'Build Something Great',
    subheadline: 'Transform your ideas into reality with our expert development team.',
    alignment: 'left',
  },
  render: (args) => ({
    props: args,
    template: \`
      <sc-hero [headline]="headline" [subheadline]="subheadline" [alignment]="alignment">
        <sc-button variant="primary">Get Started</sc-button>
        <sc-button variant="secondary">Learn More</sc-button>
      </sc-hero>
    \`,
  }),
};

export const Centered: Story = {
  args: {
    headline: 'Welcome to Our Platform',
    subheadline: 'Discover amazing possibilities.',
    alignment: 'center',
  },
};
