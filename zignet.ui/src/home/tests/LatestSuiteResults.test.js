import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import Home from '../Home';
jest.mock('react-router-dom');

it.skip('renders with data', () => {
  const promise = Promise.resolve([{
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':1,
    'TotalFailedTests':10,
    'TotalInconclusiveTests':0,
    'SuiteEndTime':'2018-02-28T15:05:00'
  }]);
  ZigNetApi.mockImplementation(() => {
  	return {
  	  getLatestSuiteResults: () => {
  	    return promise;
  	  }
  	}
  });
  
  const div = document.createElement('div');
  ReactDOM.render(<LatestSuiteResults location={{ search: '' }} zigNetApi={new ZigNetApi('http://api-url/api/')}/>, div);
  return promise.then(() => {
    ReactDOM.unmountComponentAtNode(div);
  });
  // above code waiting for promise to resolve taken from: https://github.com/airbnb/enzyme/issues/346#issuecomment-304535773
});

it.skip('matches the previous snapshot', () => {
  const promise = Promise.resolve([{
    'SuiteIds':[1],
    'SuiteName':'suite-name',
    'TotalPassedTests':1,
    'TotalFailedTests':10,
    'TotalInconclusiveTests':0,
    'SuiteEndTime':'2018-02-28T15:05:00'
  }]);
  ZigNetApi.mockImplementation(() => {
    return {
      getLatestSuiteResults: () => {
        return promise;
      }
    }
  });

  const tree = renderer
    .create(<LatestSuiteResults location={{ search: '' }} zigNetApi={new ZigNetApi('http://api-url/api/')}/>)
    .toJSON();
  return promise.then(() => {
    expect(tree).toMatchSnapshot();
  });
});