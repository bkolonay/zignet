import React from 'react';
import ReactDOM from 'react-dom';
import LatestTestResultsList from '../LatestTestResultsList';

it('renders when not grouped', () => {
  const testResult = {
    TestName: 'test-name',
    SuiteName: 'suite-name',
    PassingFromDate: '2018-05-24T01:00:00',
    TestFailureDurations: []
  }

  const component = <LatestTestResultsList suiteName={'suite-name'} suiteId={1} testResults={[]} grouped={false} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders when grouped', () => {
  const testResults = [{
    TestResultID: 1,
    TestName: 'test-name',
    SuiteName: 'suite-name',
    PassingFromDate: '2018-05-24T01:00:00',
    TestFailureDurations: []
  }];

  const component = <LatestTestResultsList suiteName={'suite-name'} suiteId={1} testResults={testResults} grouped={true} />;
  ReactDOM.render(component, document.createElement('div'));
});