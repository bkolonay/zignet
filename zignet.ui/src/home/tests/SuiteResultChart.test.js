import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import SuiteResultChart from '../SuiteResultChart';
import { getTimeFromNowWithSuffix } from '../../common/UtcDateProvider';
jest.mock('react-router-dom');
jest.mock('../../common/UtcDateProvider');

it('renders with single suite and failed and passed tests', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':100,
    'TotalFailedTests':10
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders with grouped suites', () => {
  const suiteResult = {
    'SuiteIds':[1, 2],
    'SuiteName':'grouped-suite-name',
    'TotalPassedTests':200,
    'TotalFailedTests':20
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={true} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders with only passed tests', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':200
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders with only failed tests', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalFailedTests':50
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders when suite has end time', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalFailedTests':50,
    'SuiteEndTime':'2018-05-24T01:00:00'
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders with empty data', () => {
  const suiteResult = {
    'SuiteIds':[]
  };

  const component = <SuiteResultChart suiteResult={suiteResult} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('snapshot with single suite and passed and failed tests', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':100,
    'TotalFailedTests':10
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with grouped suites', () => {
  const suiteResult = {
    'SuiteIds':[1, 2],
    'SuiteName':'grouped-suite-name',
    'TotalPassedTests':200,
    'TotalFailedTests':20
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={true} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with only passed tests', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':200
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with only failed tests', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalFailedTests':50
  };

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when suite has end time', () => {
  const suiteResult = {
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalFailedTests':50,
    'SuiteEndTime':'2018-05-24T01:00:00'
  };

  getTimeFromNowWithSuffix.mockReturnValue('3 days ago');

  const component = <SuiteResultChart suiteResult={suiteResult} grouped={false} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with empty data', () => {
  const suiteResult = {
    'SuiteIds':[]
  };

  const component = <SuiteResultChart suiteResult={suiteResult} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});