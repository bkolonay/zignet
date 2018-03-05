import React from 'react';
import ReactDOM from 'react-dom';

import SuiteResults from './SuiteResults';
import renderer from 'react-test-renderer';

it('renders single suite result', () => {
  const suiteResults = 
    [{
      'SuiteID':1,
      'SuiteName':'suite-name',
      'TotalPassedTests':1,
      'TotalFailedTests':10,
      'TotalInconclusiveTests':0,
      'SuiteEndTime':'2018-02-28T15:05:00'
    }];
  
  ReactDOM.render(
    <SuiteResults suiteResults={suiteResults} />,
    document.createElement('div')
  );
});

it('renders zero suite results', () => {
  const suiteResults = [];
  
  ReactDOM.render(
    <SuiteResults suiteResults={suiteResults} />,
    document.createElement('div')
  );
});

it('renders multiple suite results', () => {
  const suiteResults = 
    [
      {
        'SuiteID':1,
        'SuiteName':'suite-name',
        'TotalPassedTests':1,
        'TotalFailedTests':10,
        'TotalInconclusiveTests':0,
        'SuiteEndTime':'2018-02-28T15:05:00'
      },
      {
        'SuiteID':2,
        'SuiteName':'suite-name 2',
        'TotalPassedTests':9,
        'TotalFailedTests':24,
        'TotalInconclusiveTests':3,
        'SuiteEndTime':'2019-03-28T16:19:00'
      }
    ];
  
  ReactDOM.render(
    <SuiteResults suiteResults={suiteResults} />,
    document.createElement('div')
  );
});

it('matches the previous snapshot', () => {
  const suiteResults = 
    [
      {
        'SuiteID':1,
        'SuiteName':'suite-name',
        'TotalPassedTests':1,
        'TotalFailedTests':10,
        'TotalInconclusiveTests':0,
        'SuiteEndTime':'2018-02-28T15:05:00'
      },
      {
        'SuiteID':2,
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