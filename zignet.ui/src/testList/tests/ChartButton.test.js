import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import ChartButton from '../ChartButton';

it('renders', () => {
  const component = <ChartButton />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot', () => {
  const component = <ChartButton />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});