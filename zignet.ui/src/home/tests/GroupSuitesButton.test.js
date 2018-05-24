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
  const tree = renderer
    .create(<GroupSuitesButton grouped={false} />)
    .toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped', () => {
  const tree = renderer
    .create(<GroupSuitesButton grouped={true} />)
    .toJSON();
  expect(tree).toMatchSnapshot();
});