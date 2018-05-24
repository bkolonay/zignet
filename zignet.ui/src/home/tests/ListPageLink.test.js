import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import ListPageLink from '../ListPageLink';

it('renders when not grouped', () => {
  const component = <ListPageLink grouped={false} suiteId={1} totalTests={2} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders when not grouped', () => {
  const component = <ListPageLink grouped={true} suiteId={1} totalTests={2} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot when not grouped', () => {
  const component = <ListPageLink grouped={false} suiteId={1} totalTests={2} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped', () => {
  const component = <ListPageLink grouped={true} suiteId={1} totalTests={2} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when total tests zero', () => {
  const component = <ListPageLink grouped={false} suiteId={1} totalTests={0} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});