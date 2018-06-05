import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import TestStatusLabelNew from '../TestStatusLabelNew';
import { getTimeFromNow } from '../../common/UtcDateProvider';

jest.mock('../../common/UtcDateProvider');

it('renders when has failing from date', () => {
  const component = <TestStatusLabelNew failingFromDate={'2018-05-24T01:00:00'} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders when does not have failing from date', () => {
  const component = <TestStatusLabelNew failingFromDate={undefined} passingFromDate={'2018-06-05T01:00:00'} />;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot when has failing from date', () => {
  getTimeFromNow.mockReturnValue('10 minutes');
  const component = <TestStatusLabelNew failingFromDate={'2018-05-24T01:00:00'} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when does not have failing from date', () => {
  getTimeFromNow.mockReturnValue('4 days');
  const component = <TestStatusLabelNew failingFromDate={undefined} passingFromDate={'2018-06-05T01:00:00'} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});