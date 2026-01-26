import type { Meta, StoryObj } from '@storybook/angular';
import { Card } from './card';

const meta: Meta<Card> = {
  title: 'Components/Card',
  component: Card,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<Card>;

export const Default: Story = {
  args: {
    title: 'Amazing Project',
    category: 'Technology',
    description: 'This is a description of an amazing project that showcases our expertise.',
    link: '#',
    linkText: 'Read More',
  },
};

export const WithImage: Story = {
  args: {
    imageUrl: 'https://via.placeholder.com/400x225',
    imageAlt: 'Project image',
    title: 'Web Application',
    category: 'Development',
    description: 'A modern web application built with cutting-edge technologies.',
    link: '#',
  },
};

export const Minimal: Story = {
  args: {
    title: 'Simple Card',
    description: 'A minimal card design.',
    variant: 'minimal',
  },
};
