import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import ListButton from '../ListButton';

it('renders', () => {
  const component = <ListButton />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot', () => {
  const component = <ListButton />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});