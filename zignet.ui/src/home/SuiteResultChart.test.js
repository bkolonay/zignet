import React from 'react';
import ReactDOM from 'react-dom';

import SuiteResultChart from './SuiteResultChart';
import renderer from 'react-test-renderer';

it('renders with passed and failed tests', () => {
  const suiteResult = {
    'SuiteID':123456789,
    'SuiteName':'suite-name',
    'TotalPassedTests':12345,
    'TotalFailedTests':678910
  };

  const div = document.createElement('div');
  ReactDOM.render(
    <SuiteResultChart key={suiteResult.SuiteID} suiteResult={suiteResult} />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('renders with empty data', () => {
  const div = document.createElement('div');
  ReactDOM.render(
    <SuiteResultChart suiteResult={{}} />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('matches the previous snapshot', () => {
  const suiteResult = {
    'SuiteID':123456789,
    'SuiteName':'suite-name',
    'TotalPassedTests':12345,
    'TotalFailedTests':678910
  };

  const tree = renderer
    .create(<SuiteResultChart key={suiteResult.SuiteID} suiteResult={suiteResult} />)
    .toJSON();
  expect(tree).toMatchSnapshot();
});