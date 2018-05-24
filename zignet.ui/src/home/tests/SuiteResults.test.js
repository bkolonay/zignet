import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import SuiteResults from '../SuiteResults';
jest.mock('react-router-dom');

it('renders single suite result', () => {
  const suiteResults = [{
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':1,
    'TotalFailedTests':10,
    'TotalInconclusiveTests':0,
    'SuiteEndTime':'2018-02-28T15:05:00'
  }];
  
  const div = document.createElement('div');
  ReactDOM.render(
    <SuiteResults suiteResults={suiteResults} />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('renders zero suite results', () => {
  const suiteResults = [];
  
  ReactDOM.render(
    <SuiteResults suiteResults={suiteResults} />,
    document.createElement('div')
  );
});

it('renders multiple suite results', () => {
  const suiteResults = [
    {
      'SuiteIds':[1],
      'SuiteName':'suite-name',
      'TotalPassedTests':1,
      'TotalFailedTests':10,
      'TotalInconclusiveTests':0,
      'SuiteEndTime':'2018-02-28T15:05:00'
    },
    {
      'SuiteIds':[2],
      'SuiteName':'suite-name 2',
      'TotalPassedTests':9,
      'TotalFailedTests':24,
      'TotalInconclusiveTests':3,
      'SuiteEndTime':'2019-03-28T16:19:00'
    }
  ];
  
  const div = document.createElement('div');
  ReactDOM.render(
    <SuiteResults suiteResults={suiteResults} />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('matches the previous snapshot', () => {
  const suiteResults = [
    {
      'SuiteIds':[1],
      'SuiteName':'suite-name',
      'TotalPassedTests':1,
      'TotalFailedTests':10,
      'TotalInconclusiveTests':0,
      'SuiteEndTime':'2018-02-28T15:05:00'
    },
    {
      'SuiteIds':[2],
      'SuiteName':'suite-name 2',
      'TotalPassedTests':9,
      'TotalFailedTests':24,
      'TotalInconclusiveTests':3,
      'SuiteEndTime':'2019-03-28T16:19:00'
    }
  ];

  const tree = renderer
    .create(<SuiteResults suiteResults={suiteResults} />)
    .toJSON();
  expect(tree).toMatchSnapshot();
});