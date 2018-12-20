import React, { Component } from 'react';
import TestList from './TestList'
import { getTestResults } from '../common/api/api.js';
import { getFilter } from '../common/routing/SearchFilter.js'

class TestListRouter extends Component {
  render() {
    return (
      <TestList getTests={getTestResults} filter={getFilter(window.location.search)} />
    );
  }
}

export default TestListRouter;