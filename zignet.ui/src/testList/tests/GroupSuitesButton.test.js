import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import GroupSuitesButton from '../GroupSuitesButton';

it('renders when not grouped', () => {
  const component = <GroupSuitesButton grouped={false} suiteId={1} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders when not grouped', () => {
  const component = <GroupSuitesButton grouped={true} suiteId={0} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot when not grouped', () => {
  const component = <GroupSuitesButton grouped={false} suiteId={1} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped', () => {
  const component = <GroupSuitesButton grouped={true} suiteId={0} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});