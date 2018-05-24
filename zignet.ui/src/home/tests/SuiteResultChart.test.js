import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import SuiteResultChart from '../SuiteResultChart';
jest.mock('react-router-dom');

it('renders with passed and failed tests', () => {
  const suiteResult = {
    'SuiteIds':[123456789],
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
    <SuiteResultChart suiteResult={{'SuiteIds':[]}} />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

xit('matches the previous snapshot', () => {
  const suiteResult = {
    'SuiteIds':[123456789],
    'SuiteName':'suite-name',
    'TotalPassedTests':12345,
    'TotalFailedTests':678910
  };

  const tree = renderer
    .create(<SuiteResultChart key={suiteResult.SuiteID} suiteResult={suiteResult} />)
    .toJSON();
  expect(tree).toMatchSnapshot();
});