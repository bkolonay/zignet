import React, { Component } from 'react';
import TestList from './TestList'
import { getTestResults } from '../api/api.js';

class TestListRouter extends Component {

  render() {
    let grouped = false;
    if (this.props.queryString.indexOf('group=true') !== -1)
      grouped = true;

    return (
      <TestList grouped={grouped} 
                suiteId={this.props.suiteId}
                getTests={() => getTestResults(this.props.suiteId, grouped)} />
    );
  }
}

export default TestListRouter;