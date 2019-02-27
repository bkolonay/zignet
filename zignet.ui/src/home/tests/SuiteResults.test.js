import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import SuiteResults from '../SuiteResults';
jest.mock('react-router-dom');

it('renders single suite', () => {
  const suiteResults = [{
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':100,
    'TotalFailedTests':10,
    'TotalInconclusiveTests':0,
    'SuiteEndTime':'2019-02-28T15:05:00'
  }];

  const component = <SuiteResults suiteResults={suiteResults} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders zero suites', () => {
  const suiteResults = [];
  
  const component = <SuiteResults suiteResults={suiteResults} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders multiple suites', () => {
  const suiteResults = [
    {
      'SuiteIds':[1],
      'SuiteName':'suite-name',
      'TotalPassedTests':100,
      'TotalFailedTests':10,
      'TotalInconclusiveTests':0,
      'SuiteEndTime':'2019-03-28T15:05:00'
    },
    {
      'SuiteIds':[2],
      'SuiteName':'second-suite-name',
      'TotalPassedTests':0,
      'TotalFailedTests':100,
      'TotalInconclusiveTests':5,
      'SuiteEndTime':'2019-04-28T16:19:00'
    }
  ];
  
  const component = <SuiteResults suiteResults={suiteResults} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders single grouped suite', () => {
  const suiteResults = [{
    'SuiteIds':[1, 2],
    'SuiteName':'grouped-suite-name',
    'TotalPassedTests':200,
    'TotalFailedTests':100,
    'TotalInconclusiveTests':0
  }];

  const component = <SuiteResults suiteResults={suiteResults} grouped={true} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('snapshot with single suite', () => {
  const suiteResults = [{
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':100,
    'TotalFailedTests':10,
    'TotalInconclusiveTests':0
  }];

  const component = <SuiteResults suiteResults={suiteResults} grouped={false} />;

  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with multiple suites', () => {
  const suiteResults = [
    {
      'SuiteIds':[1],
      'SuiteName':'suite-name',
      'TotalPassedTests':100,
      'TotalFailedTests':10,
      'TotalInconclusiveTests':0,
      'SuiteEndTime':'2019-03-28T15:05:00'
    },
    {
      'SuiteIds':[2],
      'SuiteName':'second-suite-name',
      'TotalPassedTests':0,
      'TotalFailedTests':100,
      'TotalInconclusiveTests':5,
      'SuiteEndTime':'2019-04-28T16:19:00'
    }
  ];

  const component = <SuiteResults suiteResults={suiteResults} grouped={false} />;

  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with zero suites', () => {
  const suiteResults = [];
  const component = <SuiteResults suiteResults={suiteResults} grouped={false} />;

  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot with single grouped suite', () => {
  const suiteResults = [{
    'SuiteIds':[1, 2],
    'SuiteName':'grouped-suite-name',
    'TotalPassedTests':200,
    'TotalFailedTests':100,
    'TotalInconclusiveTests':0
  }];

  const component = <SuiteResults suiteResults={suiteResults} grouped={true} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});