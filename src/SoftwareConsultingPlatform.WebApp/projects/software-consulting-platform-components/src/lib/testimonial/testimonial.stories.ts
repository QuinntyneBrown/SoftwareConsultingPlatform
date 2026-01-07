import type { Meta, StoryObj } from '@storybook/angular';
import { Testimonial } from './testimonial';

const meta: Meta<Testimonial> = {
  title: 'Components/Testimonial',
  component: Testimonial,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<Testimonial>;

export const Default: Story = {
  args: {
    quote: 'Working with this team was an absolute pleasure. They delivered beyond our expectations.',
    authorName: 'Jane Doe',
    authorPosition: 'CEO',
    authorCompany: 'TechCorp',
    rating: 5,
  },
};

export const WithAvatar: Story = {
  args: {
    quote: 'Excellent service and outstanding results!',
    authorName: 'John Smith',
    authorPosition: 'CTO',
    authorCompany: 'InnovateLabs',
    avatarUrl: 'https://via.placeholder.com/64',
    rating: 5,
  },
};
