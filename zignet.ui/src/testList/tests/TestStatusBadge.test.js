import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import TestStatusBadge from '../TestStatusBadge';

it('renders when has failing from date', () => {
  const component = <TestStatusBadge failingFromDate={'2018-05-24T01:00:00'} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders when does not have failing from date', () => {
  const component = <TestStatusBadge failingFromDate={undefined} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot when has failing from date', () => {
  const component = <TestStatusBadge failingFromDate={'2018-05-24T01:00:00'} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when does not have failing from date', () => {
  const component = <TestStatusBadge failingFromDate={undefined} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});