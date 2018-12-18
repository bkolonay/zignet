import React, { Component } from 'react';
import TestList from './TestList'
import { getTestResults } from '../common/api/api.js';

class TestListRouter extends Component {

  render() {
    let grouped = false;
    if (this.props.queryString.indexOf('group=true') !== -1)
      grouped = true;

    return (
      <TestList grouped={grouped} 
                getTests={() => getTestResults(this.props.suiteId, grouped)} />
    );
  }
}

export default TestListRouter;