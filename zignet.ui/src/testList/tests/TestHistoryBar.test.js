import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import TestHistoryBar from '../TestHistoryBar';
import { getFailureDivs } from '../HistoryBarProvider'

jest.mock('../HistoryBarProvider');

it('renders with no failures', () => {
  getFailureDivs.mockReturnValue([]);
  const testFailureDurations = [];

  const component = <TestHistoryBar testFailureDurations={testFailureDurations}/>;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders with single failure duration', () => {
  getFailureDivs.mockReturnValue([<div key={0}/>]);
  const testFailureDurations = [{
    FailureStart: '2019-04-10T01:00:00-07:00',
    FailureEnd: '2019-04-10T02:00:00-07:00'
  }];

  const component = <TestHistoryBar testFailureDurations={testFailureDurations}/>;
  ReactDOM.render(component,document.createElement('div'));
});

it('renders with multiple failure durations', () => {
  getFailureDivs.mockReturnValue([<div key={0}/>, <div key={1}/>]);
  const testFailureDurations = [{
    FailureStart: '2019-04-10T01:00:00-07:00',
    FailureEnd: '2019-04-10T02:00:00-07:00'
  },
  {
    FailureStart: '2019-04-10T05:00:00-07:00',
    FailureEnd: '2019-04-10T07:00:00-07:00'
  }];

  const component = <TestHistoryBar testFailureDurations={testFailureDurations}/>;
  ReactDOM.render(component,document.createElement('div'));
});

it('snapshot with no failures', () => {
  getFailureDivs.mockReturnValue([]);
  const testFailureDurations = [];

  const component = <TestHistoryBar testFailureDurations={testFailureDurations}/>;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();  
});

it('snapshot with single failure duration', () => {
  getFailureDivs.mockReturnValue([<div key={0} id="div0"/>]);
  const testFailureDurations = [{
    FailureStart: '2019-04-16T01:00:00-07:00',
    FailureEnd: '2019-04-16T02:00:00-07:00'
  }];

  const component = <TestHistoryBar testFailureDurations={testFailureDurations}/>;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with a multiple failure durations', () => {
  getFailureDivs.mockReturnValue([<div key={0} id="div0"/>, <div key={1} id="div1"/>]);
  const testFailureDurations = [{
    FailureStart: '2019-04-16T01:00:00-07:00',
    FailureEnd: '2019-04-16T02:00:00-07:00'
  },
  {
    FailureStart: '2019-04-16T05:00:00-07:00',
    FailureEnd: '2019-04-16T07:00:00-07:00'
  }];

  const component = <TestHistoryBar testFailureDurations={testFailureDurations}/>;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});