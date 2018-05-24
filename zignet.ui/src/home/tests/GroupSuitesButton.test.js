import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import GroupSuitesButton from '../GroupSuitesButton';

it('renders when not grouped', () => {
  const component = <GroupSuitesButton grouped={false} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders when not grouped', () => {
  const component = <GroupSuitesButton grouped={true} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot when not grouped', () => {
  const component = <GroupSuitesButton grouped={false} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped', () => {
  const component = <GroupSuitesButton grouped={true} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});