import React, { Component } from 'react';
import TestList from './TestList'
import { getTestResults } from '../common/api/api.js';

class TestListRouter extends Component {
  constructor(props) {
    super(props);

    this.debug = false;
    if (this.props.queryString.indexOf('debug=true') !== -1)
      this.debug = true;
  }    

  render() {
    return (
      <TestList getTests={() => getTestResults(this.debug)} />
    );
  }
}

export default TestListRouter;