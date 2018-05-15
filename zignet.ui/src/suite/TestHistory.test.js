import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import TestHistory from './TestHistory';
jest.mock('react-router-dom');

// taken from: https://reactjs.org/blog/2016/11/16/react-v15.4.0.html#mocking-refs-for-snapshot-testing
function createNodeMock(element) {
  return {
    offsetWidth() {209}
  };
}

it('renders with no data', () => {

  const testFailureDurations = [];

  const div = document.createElement('div');
  ReactDOM.render(
    <TestHistory testFailureDurations={testFailureDurations}/>,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('renders with single failure duration', () => {

  const testFailureDurations = [{
    FailureStart: '2018-05-10T01:00:00-07:00',
    FailureEnd: '2018-05-10T02:00:00-07:00'
  }];

  const div = document.createElement('div');
  ReactDOM.render(
    <TestHistory testFailureDurations={testFailureDurations}/>,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('renders with multiple failure durations', () => {

  const testFailureDurations = [{
    FailureStart: '2018-05-10T01:00:00-07:00',
    FailureEnd: '2018-05-10T02:00:00-07:00'
  },
  {
    FailureStart: '2018-05-10T05:00:00-07:00',
    FailureEnd: '2018-05-10T07:00:00-07:00'
  }];

  const div = document.createElement('div');
  ReactDOM.render(
    <TestHistory testFailureDurations={testFailureDurations}/>,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('matches the previous snapshot with no data', () => {

  const testFailureDurations = [];
  const options = {createNodeMock};

  const tree = renderer
    .create(<TestHistory testFailureDurations={testFailureDurations}/>, options)
    .toJSON();
  expect(tree).toMatchSnapshot();
});

it('matches the previous snapshot with a single failure duration', () => {

  const testFailureDurations = [{
    FailureStart: '2018-05-15T01:00:00-07:00',
    FailureEnd: '2018-05-15T02:00:00-07:00'
  }];
  const options = {createNodeMock};

  const tree = renderer
    .create(<TestHistory testFailureDurations={testFailureDurations}/>, options)
    .toJSON();
  expect(tree).toMatchSnapshot();
});

it('matches the previous snapshot with a multiple failure durations', () => {

  const testFailureDurations = [{
    FailureStart: '2018-05-15T01:00:00-07:00',
    FailureEnd: '2018-05-15T02:00:00-07:00'
  },
  {
    FailureStart: '2018-05-15T05:00:00-07:00',
    FailureEnd: '2018-05-15T07:00:00-07:00'
  }];
  const options = {createNodeMock};

  const tree = renderer
    .create(<TestHistory testFailureDurations={testFailureDurations}/>, options)
    .toJSON();
  expect(tree).toMatchSnapshot();
});