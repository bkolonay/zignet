import React from 'react';
import ReactDOM from 'react-dom';
//import renderer from 'react-test-renderer';
import TestResultRow from '../TestResultRow';

it('renders when not grouped', () => {
  const testResult = {
    TestName: 'test-name',
    SuiteName: 'suite-name',
    PassingFromDate: '2018-05-24T01:00:00',
    TestFailureDurations: []
  }

  const component = <TestResultRow testResult={testResult} grouped={false} />;
  ReactDOM.render(<table><tbody>{component}</tbody></table>,document.createElement('div'));
});

it('renders when grouped', () => {
  const testResult = {
    TestName: 'grouped-test-name',
    SuiteName: 'grouped-suite-name',
    PassingFromDate: '2018-05-24T01:00:00',
    TestFailureDurations: []
  }

  const component = <TestResultRow testResult={testResult} grouped={true} />;
  ReactDOM.render(<table><tbody>{component}</tbody></table>,document.createElement('div'));
});

// below code saved to use when possibly using enzyme for shallow rendering

// taken from: https://reactjs.org/blog/2016/11/16/react-v15.4.0.html#mocking-refs-for-snapshot-testing
// function createNodeMock(element) {
//   return {
//     offsetWidth() {209}
//   };
// }

// it('snapshot when not grouped', () => {
//   ...

//   const options = {createNodeMock};
//   const component = <TestResultRow testResult={testResult} grouped={false} />;
//   const tree = renderer.create(component, options).toJSON();
//   expect(tree).toMatchSnapshot();
// });