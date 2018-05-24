import React, { Component } from 'react';
import TestList from './TestList'

class TestListRouter extends Component {
  constructor(props) {
    super(props);

    this.grouped = false;
    if (this.props.queryString.indexOf('group=true') !== -1)
      this.grouped = true;
  }  

  render() {
    return (
      <TestList grouped={this.grouped} suiteId={this.props.suiteId} />
    );
  }
}

export default TestListRouter;