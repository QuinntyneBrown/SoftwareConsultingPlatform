import type { Meta, StoryObj } from '@storybook/angular';
import { Button } from './button';

const meta: Meta<Button> = {
  title: 'Components/Button',
  component: Button,
  tags: ['autodocs'],
  argTypes: {
    variant: {
      control: { type: 'select' },
      options: ['primary', 'secondary', 'tertiary', 'danger', 'success'],
    },
    size: {
      control: { type: 'select' },
      options: ['small', 'medium', 'large'],
    },
    disabled: { control: 'boolean' },
    loading: { control: 'boolean' },
    fullWidth: { control: 'boolean' },
  },
};

export default meta;
type Story = StoryObj<Button>;

export const Primary: Story = {
  args: {
    variant: 'primary',
    size: 'medium',
    disabled: false,
    loading: false,
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size" [disabled]="disabled" [loading]="loading">Primary Button</sc-button>',
  }),
};

export const Secondary: Story = {
  args: {
    variant: 'secondary',
    size: 'medium',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size">Secondary Button</sc-button>',
  }),
};

export const Tertiary: Story = {
  args: {
    variant: 'tertiary',
    size: 'medium',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size">Tertiary Button</sc-button>',
  }),
};

export const Danger: Story = {
  args: {
    variant: 'danger',
    size: 'medium',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size">Delete</sc-button>',
  }),
};

export const Success: Story = {
  args: {
    variant: 'success',
    size: 'medium',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size">Success</sc-button>',
  }),
};

export const Small: Story = {
  args: {
    size: 'small',
    variant: 'primary',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size">Small</sc-button>',
  }),
};

export const Large: Story = {
  args: {
    size: 'large',
    variant: 'primary',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [size]="size">Large Button</sc-button>',
  }),
};

export const Disabled: Story = {
  args: {
    disabled: true,
    variant: 'primary',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [disabled]="disabled">Disabled</sc-button>',
  }),
};

export const Loading: Story = {
  args: {
    loading: true,
    variant: 'primary',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [loading]="loading">Loading</sc-button>',
  }),
};

export const FullWidth: Story = {
  args: {
    fullWidth: true,
    variant: 'primary',
  },
  render: (args) => ({
    props: args,
    template: '<sc-button [variant]="variant" [fullWidth]="fullWidth">Full Width Button</sc-button>',
  }),
};
