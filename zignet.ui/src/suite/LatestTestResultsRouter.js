import React, { Component } from 'react';
import LatestTestResults from './LatestTestResults'

class LatestTestResultsRouter extends Component {
  constructor(props) {
    super(props);

    this.grouped = false;
    if (this.props.queryString.indexOf('group=true') !== -1)
      this.grouped = true;
  }  

  render() {
    return (
      <LatestTestResults grouped={this.grouped} suiteId={this.props.suiteId} />
    );
  }
}

export default LatestTestResultsRouter;