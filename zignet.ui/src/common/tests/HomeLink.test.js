import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import HomeLink from '../HomeLink';

it('renders when not grouped', () => {
  const component = <HomeLink grouped={false} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders when not grouped', () => {
  const component = <HomeLink grouped={true} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot when not grouped', () => {
  const component = <HomeLink grouped={false} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped', () => {
  const component = <HomeLink grouped={true} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});