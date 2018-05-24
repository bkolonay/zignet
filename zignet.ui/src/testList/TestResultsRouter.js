import React, { Component } from 'react';
import TestResults from './TestResults'

class TestResultsRouter extends Component {
  constructor(props) {
    super(props);

    this.grouped = false;
    if (this.props.queryString.indexOf('group=true') !== -1)
      this.grouped = true;
  }  

  render() {
    return (
      <TestResults grouped={this.grouped} suiteId={this.props.suiteId} />
    );
  }
}

export default TestResultsRouter;